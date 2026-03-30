using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Master UI controller. Wires all board elements together.
/// Attach to a "Board" GameObject that is a child of the Canvas.
/// </summary>
public class BoardUI : MonoBehaviour
{
    [Header("Player Hand")]
    public Transform playerHandContainer;
    public GameObject cardViewPrefab;

    [Header("Enemy Hand")]
    public Transform enemyHandContainer;
    public GameObject cardBackPrefab;

    [Header("Rows — Player (0=Melee,1=Ranged,2=Siege)")]
    public Transform[] playerRowContainers = new Transform[3];
    public TMP_Text[]  playerRowScores     = new TMP_Text[3];

    [Header("Rows — Enemy")]
    public Transform[] enemyRowContainers = new Transform[3];
    public TMP_Text[]  enemyRowScores     = new TMP_Text[3];

    [Header("Weather overlays (one per row)")]
    public GameObject[] weatherOverlays = new GameObject[3];

    [Header("Scores & Rounds")]
    public TMP_Text playerTotalScore;
    public TMP_Text enemyTotalScore;
    public Image[]  playerGems = new Image[2];  // round win gems
    public Image[]  enemyGems  = new Image[2];
    public Color    gemActiveColor   = Color.yellow;
    public Color    gemInactiveColor = Color.gray;

    [Header("Buttons")]
    public Button passButton;
    public Button endTurnHint;

    [Header("Panels")]
    public GameObject roundEndPanel;
    public TMP_Text   roundEndText;
    public GameObject gameOverPanel;
    public TMP_Text   gameOverText;
    public GameObject messagePanel;
    public TMP_Text   messageText;

    [Header("Card Detail")]
    public CardDetailPanel cardDetailPanel;

    // ── Row click zones (invisible buttons over each row) ──────────────────
    [Header("Player Row Click Zones")]
    public Button[] playerRowButtons = new Button[3];

    private GameManager _gm;
    private List<CardView> _handViews = new();
    private int _pendingRow = -1;

    // ══════════════════════════════════════════════════════════════════════════
    void Start()
    {
        _gm = GameManager.Instance;

        // Wire events
        _gm.onHandChanged.AddListener(RefreshHand);
        _gm.onCardPlayed.AddListener(OnCardPlayed);
        _gm.onScoreChanged.AddListener(RefreshTotals);
        _gm.onRowScoreChanged.AddListener(RefreshRowScore);
        _gm.onRoundWon.AddListener(ShowRoundEnd);
        _gm.onGameOver.AddListener(ShowGameOver);
        _gm.onWeatherChanged.AddListener(RefreshWeather);
        _gm.onMessage.AddListener(ShowMessage);
        _gm.onPhaseChanged.AddListener(OnPhaseChanged);

        // Pass button
        passButton.onClick.AddListener(_gm.PlayerPass);

        // Row buttons
        for (int r = 0; r < 3; r++)
        {
            int row = r; // capture for lambda
            playerRowButtons[r].onClick.AddListener(() => OnRowClicked(row));
        }

        // Hide panels
        roundEndPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        messagePanel.SetActive(false);

        RefreshGems();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Hand
    // ══════════════════════════════════════════════════════════════════════════
    void RefreshHand()
    {
        // Clear old views
        foreach (var cv in _handViews) if (cv != null) Destroy(cv.gameObject);
        _handViews.Clear();

        foreach (Transform child in playerHandContainer) Destroy(child.gameObject);

        // Rebuild player hand
        foreach (var card in _gm.Hand[0])
        {
            var go = Instantiate(cardViewPrefab, playerHandContainer);
            var cv = go.GetComponent<CardView>();
            cv.Init(card, true);
            cv.OnCardClicked  += OnPlayerCardClicked;
            cv.OnCardHovered  += cardDetailPanel.Show;
            cv.OnCardUnhovered += cardDetailPanel.Hide;
            _handViews.Add(cv);
        }

        // Enemy hand (face-down backs)
        foreach (Transform child in enemyHandContainer) Destroy(child.gameObject);
        for (int i = 0; i < _gm.Hand[1].Count; i++)
            Instantiate(cardBackPrefab, enemyHandContainer);

        // Refresh all field rows too
        RefreshAllRows();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Field rows
    // ══════════════════════════════════════════════════════════════════════════
    void RefreshAllRows()
    {
        for (int p = 0; p < 2; p++)
            for (int r = 0; r < 3; r++)
                RefreshRow(p, r);
    }

    void RefreshRow(int player, int row)
    {
        var container = player == 0 ? playerRowContainers[row] : enemyRowContainers[row];
        foreach (Transform child in container) Destroy(child.gameObject);

        foreach (var card in _gm.Rows[player, row])
        {
            var go = Instantiate(cardViewPrefab, container);
            var cv = go.GetComponent<CardView>();
            cv.Init(card, false);
            cv.OnCardHovered   += cardDetailPanel.Show;
            cv.OnCardUnhovered += cardDetailPanel.Hide;
        }
    }

    void OnCardPlayed(CardInstance card)
    {
        RefreshAllRows();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Card selection & row targeting
    // ══════════════════════════════════════════════════════════════════════════
    void OnPlayerCardClicked(CardView cv)
    {
        if (_gm.CurrentPhase != GamePhase.PlayerTurn) return;

        var card = cv.GetCard();

        // Weather / Special → play immediately
        if (card.Data.type == CardType.Weather || card.Data.type == CardType.Special)
        {
            _gm.PlaySpecialCard(card);
            ClearRowHighlights();
            return;
        }

        // Unit with fixed row → highlight that row only
        _gm.SelectCard(card);
        HighlightRows(card.Data.row);

        ShowMessage($"Seleccionada: {card.Data.cardName}. Elige una fila.");
    }

    void OnRowClicked(int row)
    {
        if (_gm.GetSelectedCard() == null) return;
        _gm.PlayCardToRow(row);
        ClearRowHighlights();
    }

    void HighlightRows(CardRow allowed)
    {
        for (int r = 0; r < 3; r++)
        {
            bool active = allowed == CardRow.Any || (int)allowed == r;
            playerRowButtons[r].interactable = active;
            // Visual tint
            var img = playerRowButtons[r].GetComponent<Image>();
            if (img) img.color = active ? new Color(1, 1, 0, 0.15f) : new Color(0, 0, 0, 0);
        }
    }

    void ClearRowHighlights()
    {
        foreach (var btn in playerRowButtons)
        {
            btn.interactable = false;
            var img = btn.GetComponent<Image>();
            if (img) img.color = Color.clear;
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Scores
    // ══════════════════════════════════════════════════════════════════════════
    void RefreshRowScore(int player, int row, int score)
    {
        var texts = player == 0 ? playerRowScores : enemyRowScores;
        if (texts[row] != null) texts[row].text = score.ToString();
        RefreshRow(player, row);
    }

    void RefreshTotals(int playerScore, int enemyScore)
    {
        if (playerTotalScore) playerTotalScore.text = playerScore.ToString();
        if (enemyTotalScore)  enemyTotalScore.text  = enemyScore.ToString();

        // Color: green if winning, red if losing
        bool winning = playerScore >= enemyScore;
        if (playerTotalScore) playerTotalScore.color = winning ? Color.green : Color.red;
        if (enemyTotalScore)  enemyTotalScore.color  = winning ? Color.red  : Color.green;
    }

    void RefreshGems()
    {
        for (int i = 0; i < 2; i++)
        {
            if (playerGems[i]) playerGems[i].color = i < _gm.RoundWins[0] ? gemActiveColor : gemInactiveColor;
            if (enemyGems[i])  enemyGems[i].color  = i < _gm.RoundWins[1] ? gemActiveColor : gemInactiveColor;
        }
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Weather
    // ══════════════════════════════════════════════════════════════════════════
    void RefreshWeather(WeatherRow wr)
    {
        for (int r = 0; r < 3; r++)
            if (weatherOverlays[r]) weatherOverlays[r].SetActive(_gm.WeatherActive[r]);
        RefreshAllRows();
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Phase
    // ══════════════════════════════════════════════════════════════════════════
    void OnPhaseChanged(GamePhase phase)
    {
        passButton.interactable = phase == GamePhase.PlayerTurn;
        ClearRowHighlights();

        if (phase == GamePhase.PlayerTurn)
            ShowMessage("Tu turno.");
        else if (phase == GamePhase.EnemyTurn)
            ShowMessage("Turno del enemigo...");
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Round end / Game over
    // ══════════════════════════════════════════════════════════════════════════
    void ShowRoundEnd(int winner)
    {
        RefreshGems();
        roundEndPanel.SetActive(true);
        string msg = winner == 0 ? "¡Ganaste la ronda!"
                   : winner == 1 ? "El enemigo gana la ronda."
                   : "¡Empate!";
        if (roundEndText) roundEndText.text = msg;
        StartCoroutine(HideAfter(roundEndPanel, 2.5f));
    }

    void ShowGameOver(int winner)
    {
        gameOverPanel.SetActive(true);
        string msg = winner == 0 ? "¡VICTORIA!\nEl mundo Nahkor te reconoce."
                   : winner == 1 ? "DERROTA\nLas sombras te engullen."
                   : "EMPATE\nNinguno de los dos merece la victoria.";
        if (gameOverText) gameOverText.text = msg;
    }

    void ShowMessage(string msg)
    {
        if (!messagePanel || !messageText) return;
        messageText.text = msg;
        messagePanel.SetActive(true);
        StopCoroutine(nameof(HideMessageRoutine));
        StartCoroutine(nameof(HideMessageRoutine));
    }

    IEnumerator HideMessageRoutine()
    {
        yield return new WaitForSeconds(3f);
        if (messagePanel) messagePanel.SetActive(false);
    }

    IEnumerator HideAfter(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        go.SetActive(false);
    }
}
