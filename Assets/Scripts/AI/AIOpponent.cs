using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Simple AI: plays the highest-power card available.
/// Passes if it's losing significantly (Gwent-like strategy).
/// </summary>
public class AIOpponent : MonoBehaviour
{
    public float thinkDelay = 1.2f;

    public void TakeTurn()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkDelay);

        var gm   = GameManager.Instance;
        var hand = gm.Hand[1];

        if (hand.Count == 0 || ShouldPass(gm))
        {
            gm.Pass(1);
            yield break;
        }

        // Pick best card: spy first, then highest power
        CardInstance chosen = hand.FirstOrDefault(c => c.Data.hasSpy)
                           ?? hand.OrderByDescending(c => c.CurrentPower).First();

        int row = chosen.Data.row == CardRow.Any ? Random.Range(0, 3) : (int)chosen.Data.row;
        gm.PlayCard(1, chosen, row);
    }

    bool ShouldPass(GameManager gm)
    {
        int myScore    = gm.GetTotalScore(1);
        int theirScore = gm.GetTotalScore(0);

        // Pass if winning by enough and enemy already passed
        return myScore > theirScore + 10;
    }
}
