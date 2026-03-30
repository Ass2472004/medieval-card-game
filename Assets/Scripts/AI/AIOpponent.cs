using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Gwent-style AI with three difficulty levels.
/// </summary>
public class AIOpponent : MonoBehaviour
{
    public enum Difficulty { Easy, Normal, Hard }

    [Header("Config")]
    public Difficulty difficulty = Difficulty.Normal;
    public float thinkMin = 0.8f;
    public float thinkMax = 1.8f;

    private GameManager _gm;

    void Awake() => _gm = GetComponent<GameManager>();

    public void TakeTurn()
    {
        if (_gm.CurrentPhase != GamePhase.EnemyTurn) return;
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(Random.Range(thinkMin, thinkMax));

        var hand = _gm.Hand[1];

        if (hand.Count == 0 || ShouldPass())
        {
            _gm.EnemyPass();
            yield break;
        }

        var (card, row) = PickBestPlay(hand);
        if (card == null) { _gm.EnemyPass(); yield break; }

        if (card.Data.type == CardType.Weather || card.Data.type == CardType.Special)
            _gm.PlaySpecialCard(card);
        else
            _gm.EnemyPlayCard(card, row);
    }

    (CardInstance card, int row) PickBestPlay(List<CardInstance> hand)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:   return PickRandom(hand);
            case Difficulty.Normal: return PickSmart(hand);
            case Difficulty.Hard:   return PickOptimal(hand);
            default:                return PickSmart(hand);
        }
    }

    // ── Easy: random ─────────────────────────────────────────────────────────
    (CardInstance, int) PickRandom(List<CardInstance> hand)
    {
        var card = hand[Random.Range(0, hand.Count)];
        return (card, Random.Range(0, 3));
    }

    // ── Normal: prefer high-power, use spies, avoid passing ─────────────────
    (CardInstance, int) PickSmart(List<CardInstance> hand)
    {
        // Priority 1: Spy if we're losing
        int myScore = _gm.GetTotalScore(1), theirScore = _gm.GetTotalScore(0);
        if (myScore < theirScore)
        {
            var spy = hand.FirstOrDefault(c => c.Data.hasSpy);
            if (spy != null) return (spy, PreferredRow(spy));
        }

        // Priority 2: Scorch if enemy has a big unit
        var allEnemy = AllEnemyCards();
        if (allEnemy.Any() && allEnemy.Max(c => c.CurrentPower) >= 7)
        {
            var scorch = hand.FirstOrDefault(c => c.Data.hasScorch || c.Data.type == CardType.Special && c.Data.hasScorch);
            if (scorch != null) return (scorch, PreferredRow(scorch));
        }

        // Priority 3: Medic / Muster if available
        var muster = hand.FirstOrDefault(c => c.Data.hasMuster);
        if (muster != null) return (muster, PreferredRow(muster));

        // Priority 4: Highest power unit
        var best = hand.Where(c => c.Data.type == CardType.Unit)
                       .OrderByDescending(c => c.Data.basePower)
                       .FirstOrDefault();
        if (best != null) return (best, PreferredRow(best));

        // Fallback: any card
        return (hand[0], 0);
    }

    // ── Hard: minimax-lite, considers tight bond and weather synergies ────────
    (CardInstance, int) PickOptimal(List<CardInstance> hand)
    {
        CardInstance bestCard = null;
        int          bestRow  = 0;
        int          bestGain = int.MinValue;

        foreach (var card in hand)
        {
            for (int row = 0; row < 3; row++)
            {
                if (card.Data.row != CardRow.Any && (int)card.Data.row != row) continue;

                int gain = EvaluatePlay(card, row);
                if (gain > bestGain) { bestGain = bestCard == null ? 0 : gain; bestCard = card; bestRow = row; }
            }
        }

        return bestCard != null ? (bestCard, bestRow) : PickSmart(hand);
    }

    int EvaluatePlay(CardInstance card, int row)
    {
        int score = card.Data.basePower;

        if (card.Data.hasSpy)
        {
            // Spy draws 2 cards — worth about 6 power in card advantage
            score += 6 - card.Data.basePower;
        }

        if (card.Data.hasTightBond)
        {
            int existing = _gm.Rows[1, row].Count(c => c.Data == card.Data);
            if (existing > 0) score *= (existing + 1);
        }

        if (card.Data.hasMorale)
            score += _gm.Rows[1, row].Count;

        if (card.Data.hasMuster)
        {
            int copies = _gm.Deck[1].Count(c => c.Data == card.Data);
            score += copies * card.Data.basePower;
        }

        if (card.Data.hasScorch)
        {
            var maxEnemy = AllEnemyCards().Any() ? AllEnemyCards().Max(c => c.CurrentPower) : 0;
            score += maxEnemy;
        }

        return score;
    }

    // ── Pass logic ───────────────────────────────────────────────────────────
    bool ShouldPass()
    {
        int myScore    = _gm.GetTotalScore(1);
        int theirScore = _gm.GetTotalScore(0);

        // If winning and enemy has passed, always pass
        if (_gm.Hand[1].Count <= 1) return true;

        switch (difficulty)
        {
            case Difficulty.Easy:
                // Pass randomly with 15% chance each turn
                return Random.value < 0.15f;

            case Difficulty.Normal:
                // Pass if winning by more than 8 and have few cards left
                return myScore > theirScore + 8 && _gm.Hand[1].Count <= 3;

            case Difficulty.Hard:
                // Pass if winning and it's not worth risking the round
                return myScore > theirScore + 5 &&
                       (_gm.Hand[1].Count <= 2 || _gm.Hand[0].Count <= 1);

            default: return false;
        }
    }

    int PreferredRow(CardInstance card) =>
        card.Data.row == CardRow.Any ? Random.Range(0, 3) : (int)card.Data.row;

    IEnumerable<CardInstance> AllEnemyCards()
    {
        for (int r = 0; r < 3; r++)
            foreach (var c in _gm.Rows[0, r]) yield return c;
    }
}
