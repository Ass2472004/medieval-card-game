using UnityEngine;

/// <summary>
/// Entry point — attach to the same GameObject as GameManager and AIOpponent.
/// Builds decks and starts the game on Awake.
/// </summary>
public class GameStarter : MonoBehaviour
{
    public DeckBuilder deckBuilder;

    void Start()
    {
        if (deckBuilder == null) { Debug.LogError("DeckBuilder not assigned!"); return; }

        var playerDeck = deckBuilder.BuildPlayerDeck();
        var enemyDeck  = deckBuilder.BuildEnemyDeck();

        GameManager.Instance.StartGame(playerDeck, enemyDeck);
    }
}
