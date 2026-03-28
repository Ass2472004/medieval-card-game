using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase { Setup, PlayerTurn, EnemyTurn, RoundEnd, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    public int handSize        = 10;
    public int roundsToWin     = 2;

    [Header("Events")]
    public UnityEvent<GamePhase> onPhaseChanged;
    public UnityEvent<int, int>  onScoreChanged;   // (playerScore, enemyScore)
    public UnityEvent<int>       onRoundWon;        // winner index
    public UnityEvent<int>       onGameOver;        // winner index

    // ── State ──────────────────────────────────────────────────
    public GamePhase CurrentPhase { get; private set; }

    public List<CardInstance>[] Hand    = new List<CardInstance>[2];
    public List<CardInstance>[] Deck    = new List<CardInstance>[2];
    public List<CardInstance>[] Discard = new List<CardInstance>[2];

    // Rows: [player/enemy][melee/ranged/siege]
    public List<CardInstance>[,] Rows = new List<CardInstance>[2, 3];

    private int[] _roundWins  = new int[2];
    private bool[] _passed    = new bool[2];

    // ── Unity lifecycle ─────────────────────────────────────────
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
    }

    // ── Public API ───────────────────────────────────────────────
    public void StartGame(List<CardData> playerDeck, List<CardData> enemyDeck)
    {
        InitCollections();
        _roundWins = new int[2];
        LoadDeck(0, playerDeck);
        LoadDeck(1, enemyDeck);
        StartRound();
    }

    public void PlayCard(int playerIndex, CardInstance card, int rowIndex)
    {
        if (CurrentPhase != (playerIndex == 0 ? GamePhase.PlayerTurn : GamePhase.EnemyTurn)) return;
        if (!Hand[playerIndex].Remove(card)) return;

        PlaceCard(playerIndex, card, rowIndex);
        ApplyAbility(card, playerIndex, rowIndex);
        RefreshScores();
        AdvanceTurn();
    }

    public void Pass(int playerIndex)
    {
        _passed[playerIndex] = true;
        if (_passed[0] && _passed[1]) EndRound();
        else AdvanceTurn();
    }

    // ── Internal ─────────────────────────────────────────────────
    void StartRound()
    {
        _passed = new bool[2];
        DrawCards(0, handSize - Hand[0].Count);
        DrawCards(1, handSize - Hand[1].Count);
        SetPhase(GamePhase.PlayerTurn);
    }

    void AdvanceTurn()
    {
        if (_passed[0] && !_passed[1]) { SetPhase(GamePhase.EnemyTurn); return; }
        if (!_passed[0] && _passed[1]) { SetPhase(GamePhase.PlayerTurn); return; }
        SetPhase(CurrentPhase == GamePhase.PlayerTurn ? GamePhase.EnemyTurn : GamePhase.PlayerTurn);
    }

    void EndRound()
    {
        SetPhase(GamePhase.RoundEnd);
        int ps = GetTotalScore(0);
        int es = GetTotalScore(1);
        int winner = ps > es ? 0 : es > ps ? 1 : -1; // -1 = draw
        if (winner >= 0) _roundWins[winner]++;
        onRoundWon?.Invoke(winner);

        ClearBoard();

        if (_roundWins[0] >= roundsToWin || _roundWins[1] >= roundsToWin)
        {
            int gameWinner = _roundWins[0] >= roundsToWin ? 0 : 1;
            SetPhase(GamePhase.GameOver);
            onGameOver?.Invoke(gameWinner);
        }
        else StartRound();
    }

    void ClearBoard()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
            {
                foreach (var c in Rows[p, r]) Discard[p].Add(c);
                Rows[p, r].Clear();
            }
    }

    void LoadDeck(int p, List<CardData> cards)
    {
        Deck[p] = cards.Select(d => new CardInstance(d, p)).OrderBy(_ => Random.value).ToList();
    }

    void DrawCards(int p, int count)
    {
        for (int i = 0; i < count && Deck[p].Count > 0; i++)
        {
            Hand[p].Add(Deck[p][0]);
            Deck[p].RemoveAt(0);
        }
    }

    void PlaceCard(int p, CardInstance card, int row)
    {
        int targetRow = card.Data.row == CardRow.Any ? row : (int)card.Data.row;
        Rows[p, targetRow].Add(card);
    }

    void ApplyAbility(CardInstance card, int owner, int row)
    {
        if (card.Data.hasSpy)
        {
            // Move card to enemy field, draw 2
            int enemy = 1 - owner;
            int r = (int)card.Data.row;
            Rows[owner, r].Remove(card);
            Rows[enemy, r].Add(card);
            DrawCards(owner, 2);
        }
        if (card.Data.hasTightBond)
        {
            int r = (int)card.Data.row;
            int count = Rows[owner, r].Count(c => c.Data == card.Data);
            if (count > 1)
                foreach (var c in Rows[owner, r].Where(c => c.Data == card.Data))
                    c.SetPower(card.Data.basePower * count);
        }
        if (card.Data.hasMorale)
        {
            int r = (int)card.Data.row;
            foreach (var c in Rows[owner, r].Where(c => c != card))
                c.AddPower(1);
        }
        if (card.Data.hasScorch)
        {
            int maxPow = AllFieldCards().Max(c => c.CurrentPower);
            foreach (var c in AllFieldCards().Where(c => c.CurrentPower == maxPow).ToList())
                foreach (var row2 in AllRows()) row2.Remove(c);
        }
        if (card.Data.hasMuster)
        {
            var copies = Deck[owner].Where(c => c.Data == card.Data).ToList();
            foreach (var c in copies) { Deck[owner].Remove(c); PlaceCard(owner, c, row); }
        }
    }

    IEnumerable<CardInstance> AllFieldCards()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
                foreach (var c in Rows[p, r]) yield return c;
    }

    IEnumerable<List<CardInstance>> AllRows()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
                yield return Rows[p, r];
    }

    public int GetRowScore(int player, int row)  => Rows[player, row].Sum(c => c.CurrentPower);
    public int GetTotalScore(int player)          => Enumerable.Range(0, 3).Sum(r => GetRowScore(player, r));

    void RefreshScores() => onScoreChanged?.Invoke(GetTotalScore(0), GetTotalScore(1));
    void SetPhase(GamePhase p) { CurrentPhase = p; onPhaseChanged?.Invoke(p); }
}
