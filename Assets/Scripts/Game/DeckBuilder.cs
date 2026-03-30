using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Builds starter decks from the CardDatabase for both player and enemy.
/// </summary>
public class DeckBuilder : MonoBehaviour
{
    public CardDatabase database;

    [Header("Starter Decks")]
    public CardFaction playerFaction = CardFaction.Kingdom;
    public CardFaction enemyFaction  = CardFaction.Empire;

    public List<CardData> BuildPlayerDeck() => BuildDeck(playerFaction);
    public List<CardData> BuildEnemyDeck()  => BuildDeck(enemyFaction);

    List<CardData> BuildDeck(CardFaction faction)
    {
        if (database == null) { Debug.LogError("CardDatabase not assigned!"); return new(); }

        var deck = new List<CardData>();

        // Faction cards (up to 20)
        var factionCards = database.allCards
            .Where(c => c.faction == faction)
            .OrderByDescending(c => c.basePower)
            .Take(20)
            .ToList();
        deck.AddRange(factionCards);

        // Neutral cards (up to 6)
        var neutralCards = database.allCards
            .Where(c => c.faction == CardFaction.Neutral)
            .OrderBy(_ => Random.value)
            .Take(6)
            .ToList();
        deck.AddRange(neutralCards);

        // Fill to 25 with faction duplicates if needed
        while (deck.Count < 25 && factionCards.Count > 0)
        {
            var extra = factionCards[Random.Range(0, factionCards.Count)];
            if (deck.Count(c => c == extra) < 3) // max 3 copies
                deck.Add(extra);
        }

        return deck.OrderBy(_ => Random.value).ToList();
    }
}
