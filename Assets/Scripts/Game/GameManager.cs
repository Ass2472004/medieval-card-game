using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase { Setup, Mulligan, PlayerTurn, EnemyTurn, RoundEnd, GameOver }
public enum WeatherRow  { None = -1, Melee = 0, Ranged = 1, Siege = 2, All = 3 }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    public int handSize      = 10;
    public int mulliganCount = 2;
    public int roundsToWin   = 2;

    [Header("Events")]
    public UnityEvent<GamePhase>     onPhaseChanged;
    public UnityEvent<int, int>      onScoreChanged;    // playerTotal, enemyTotal
    public UnityEvent<int, int, int> onRowScoreChanged; // player, row, score
    public UnityEvent<int>           onRoundWon;        // winner 0/1, -1=draw
    public UnityEvent<int>           onGameOver;
    public UnityEvent<CardInstance>  onCardPlayed;
    public UnityEvent<WeatherRow>    onWeatherChanged;
    public UnityEvent                onHandChanged;
    public UnityEvent<string>        onMessage;

    // ── State ──────────────────────────────────────────────────────────────
    public GamePhase CurrentPhase { get; private set; }

    public List<CardInstance>[]   Hand    = new List<CardInstance>[2];
    public List<CardInstance>[]   Deck    = new List<CardInstance>[2];
    public List<CardInstance>[]   Discard = new List<CardInstance>[2];
    public List<CardInstance>[,]  Rows    = new List<CardInstance>[2, 3];
    public bool[]                 WeatherActive = new bool[3]; // melee/ranged/siege

    public int[] RoundWins  = new int[2];
    public int   CurrentRound { get; private set; } = 1;

    private bool[] _passed = new bool[2];
    private CardInstance _selectedCard;

    // ── Unity lifecycle ─────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        InitCollections();
    }

    void InitCollections()
    {
        for (int i = 0; i < 2; i++)
        {
            Hand[i]    = new List<CardInstance>();
            Deck[i]    = new List<CardInstance>();
            Discard[i] = new List<CardInstance>();
            for (int r = 0; r < 3; r++)
                Rows[i, r] = new List<CardInstance>();
        }
        WeatherActive = new bool[3];
    }

    // ── Public API ───────────────────────────────────────────────────────────
    public void StartGame(List<CardData> playerDeck, List<CardData> enemyDeck)
    {
        InitCollections();
        RoundWins    = new int[2];
        CurrentRound = 1;

        LoadDeck(0, playerDeck);
        LoadDeck(1, enemyDeck);

        DrawCards(0, handSize);
        DrawCards(1, handSize);

        SetPhase(GamePhase.PlayerTurn);
        onMessage?.Invoke("¡Que empiece la batalla!");
    }

    public void SelectCard(CardInstance card)
    {
        if (CurrentPhase != GamePhase.PlayerTurn) return;
        _selectedCard = card;
    }

    public CardInstance GetSelectedCard() => _selectedCard;

    public void PlayCardToRow(int rowIndex)
    {
        if (CurrentPhase != GamePhase.PlayerTurn) return;
        if (_selectedCard == null) return;
        if (!Hand[0].Contains(_selectedCard)) return;

        StartCoroutine(PlayCardRoutine(0, _selectedCard, rowIndex));
        _selectedCard = null;
    }

    public void PlaySpecialCard(CardInstance card)
    {
        if (CurrentPhase != GamePhase.PlayerTurn) return;
        if (!Hand[0].Contains(card)) return;
        StartCoroutine(PlayCardRoutine(0, card, -1));
        _selectedCard = null;
    }

    public void PlayerPass()
    {
        if (CurrentPhase != GamePhase.PlayerTurn) return;
        _passed[0] = true;
        onMessage?.Invoke("Pasas el turno.");
        if (_passed[1]) { StartCoroutine(EndRoundRoutine()); return; }
        SetPhase(GamePhase.EnemyTurn);
        GetComponent<AIOpponent>()?.TakeTurn();
    }

    // Called by AI
    public void EnemyPlayCard(CardInstance card, int rowIndex)
    {
        if (CurrentPhase != GamePhase.EnemyTurn) return;
        StartCoroutine(PlayCardRoutine(1, card, rowIndex));
    }

    public void EnemyPass()
    {
        _passed[1] = true;
        onMessage?.Invoke("El enemigo pasa.");
        if (_passed[0]) { StartCoroutine(EndRoundRoutine()); return; }
        SetPhase(GamePhase.PlayerTurn);
    }

    // ── Internal ─────────────────────────────────────────────────────────────
    IEnumerator PlayCardRoutine(int player, CardInstance card, int rowIndex)
    {
        Hand[player].Remove(card);
        onHandChanged?.Invoke();

        // Weather cards
        if (card.Data.type == CardType.Weather)
        {
            ApplyWeather(card, rowIndex);
            Discard[player].Add(card);
            onCardPlayed?.Invoke(card);
            yield return new WaitForSeconds(0.6f);
        }
        // Special (non-unit)
        else if (card.Data.type == CardType.Special)
        {
            ApplySpecial(card, player);
            Discard[player].Add(card);
            onCardPlayed?.Invoke(card);
            yield return new WaitForSeconds(0.6f);
        }
        // Unit
        else
        {
            int targetRow = ResolveRow(card, rowIndex);

            if (card.Data.hasSpy)
            {
                int enemy = 1 - player;
                Rows[enemy, targetRow].Add(card);
                ApplyWeatherToCard(card, targetRow);
                DrawCards(player, 2);
                onMessage?.Invoke(player == 0 ? "¡Espia! Robas 2 cartas." : "El enemigo juega un espia.");
            }
            else
            {
                Rows[player, targetRow].Add(card);
                ApplyWeatherToCard(card, targetRow);
            }

            onCardPlayed?.Invoke(card);
            yield return new WaitForSeconds(0.3f);

            ApplyUnitAbilities(card, player, targetRow);
            yield return new WaitForSeconds(0.3f);
        }

        RefreshAllScores();
        onHandChanged?.Invoke();

        if (player == 0) { SetPhase(GamePhase.EnemyTurn); GetComponent<AIOpponent>()?.TakeTurn(); }
        else             { if (!_passed[0]) SetPhase(GamePhase.PlayerTurn); }
    }

    void ApplyWeather(CardInstance card, int row)
    {
        // Commander's Horn on weather card = clear weather
        if (card.Data.hasCommanderHorn) { ClearWeather(); return; }

        // Determine which row the weather hits based on CardRow
        int weatherRow = card.Data.row == CardRow.Any ? -1 : (int)card.Data.row;
        if (weatherRow < 0 || weatherRow > 2) return;

        WeatherActive[weatherRow] = true;
        onWeatherChanged?.Invoke((WeatherRow)weatherRow);

        // Apply to all cards in both players' affected rows
        for (int p = 0; p < 2; p++)
            foreach (var c in Rows[p, weatherRow])
                c.ApplyWeather();
    }

    void ClearWeather()
    {
        WeatherActive = new bool[3];
        onWeatherChanged?.Invoke(WeatherRow.None);
        foreach (var c in AllFieldCards()) c.RemoveWeather();
    }

    void ApplyWeatherToCard(CardInstance card, int row)
    {
        if (WeatherActive[row]) card.ApplyWeather();
    }

    void ApplySpecial(CardInstance card, int player)
    {
        if (card.Data.hasScorch)   ApplyScorch(player);
        if (card.Data.hasMedic)    onMessage?.Invoke("Elige una carta del descarte para revivir.");
        if (card.Data.hasDecoy)    onMessage?.Invoke("Elige una unidad del campo para intercambiar.");
        if (card.Data.hasCommanderHorn) ClearWeather();
    }

    void ApplyUnitAbilities(CardInstance card, int player, int row)
    {
        if (card.Data.hasMuster)
        {
            var copies = Deck[player].Where(c => c.Data == card.Data).ToList();
            foreach (var c in copies)
            {
                Deck[player].Remove(c);
                Rows[player, row].Add(c);
                ApplyWeatherToCard(c, row);
            }
            if (copies.Count > 0)
                onMessage?.Invoke($"Convocatoria: {copies.Count} aliados llegan al campo.");
        }

        if (card.Data.hasTightBond)
        {
            var bonds = Rows[player, row].Where(c => c.Data == card.Data).ToList();
            if (bonds.Count > 1)
                foreach (var c in bonds) c.SetPower(card.Data.basePower * bonds.Count);
        }

        if (card.Data.hasMorale)
            foreach (var c in Rows[player, row].Where(c => c != card)) c.AddPower(1);

        if (card.Data.hasCommanderHorn)
            foreach (var c in Rows[player, row].Where(c => c != card)) c.SetPower(c.CurrentPower * 2);

        if (card.Data.hasScorch) ApplyScorch(player);
    }

    void ApplyScorch(int player)
    {
        var allCards = AllFieldCards().ToList();
        if (!allCards.Any()) return;
        int maxPower = allCards.Max(c => c.CurrentPower);
        if (maxPower <= 0) return;

        var toDestroy = allCards.Where(c => c.CurrentPower == maxPower).ToList();
        foreach (var c in toDestroy)
        {
            for (int p = 0; p < 2; p++)
                for (int r = 0; r < 3; r++)
                    if (Rows[p, r].Remove(c)) Discard[p].Add(c);
        }
        onMessage?.Invoke($"¡Chamusquina! {toDestroy.Count} unidad(es) destruida(s).");
    }

    IEnumerator EndRoundRoutine()
    {
        SetPhase(GamePhase.RoundEnd);
        yield return new WaitForSeconds(1.5f);

        int ps = GetTotalScore(0), es = GetTotalScore(1);
        int winner = ps > es ? 0 : es > ps ? 1 : -1;
        if (winner >= 0) RoundWins[winner]++;
        onRoundWon?.Invoke(winner);

        yield return new WaitForSeconds(2f);

        ClearBoard();
        CurrentRound++;

        if (RoundWins[0] >= roundsToWin || RoundWins[1] >= roundsToWin ||
            (RoundWins[0] + RoundWins[1]) >= 3)
        {
            int gameWinner = RoundWins[0] > RoundWins[1] ? 0 :
                             RoundWins[1] > RoundWins[0] ? 1 : -1;
            SetPhase(GamePhase.GameOver);
            onGameOver?.Invoke(gameWinner);
            yield break;
        }

        _passed    = new bool[2];
        WeatherActive = new bool[3];
        onWeatherChanged?.Invoke(WeatherRow.None);

        DrawCards(0, Mathf.Min(2, handSize - Hand[0].Count));
        DrawCards(1, Mathf.Min(2, handSize - Hand[1].Count));

        SetPhase(GamePhase.PlayerTurn);
        onMessage?.Invoke($"Ronda {CurrentRound} — ¡A batallar!");
    }

    void ClearBoard()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
            {
                Discard[p].AddRange(Rows[p, r]);
                Rows[p, r].Clear();
            }
    }

    void LoadDeck(int p, List<CardData> cards)
    {
        Deck[p] = cards.Select(d => new CardInstance(d, p))
                       .OrderBy(_ => Random.value).ToList();
    }

    void DrawCards(int p, int count)
    {
        for (int i = 0; i < count && Deck[p].Count > 0; i++)
        {
            Hand[p].Add(Deck[p][0]);
            Deck[p].RemoveAt(0);
        }
    }

    int ResolveRow(CardInstance card, int preferred)
    {
        if (card.Data.row == CardRow.Any) return Mathf.Clamp(preferred, 0, 2);
        return (int)card.Data.row;
    }

    IEnumerable<CardInstance> AllFieldCards()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
                foreach (var c in Rows[p, r]) yield return c;
    }

    public int GetRowScore(int player, int row) => Rows[player, row].Sum(c => c.CurrentPower);
    public int GetTotalScore(int player)         => Enumerable.Range(0, 3).Sum(r => GetRowScore(player, r));

    void RefreshAllScores()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
                onRowScoreChanged?.Invoke(p, r, GetRowScore(p, r));
        onScoreChanged?.Invoke(GetTotalScore(0), GetTotalScore(1));
    }

    void SetPhase(GamePhase p) { CurrentPhase = p; onPhaseChanged?.Invoke(p); }
}
