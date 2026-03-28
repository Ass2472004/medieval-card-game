using UnityEngine;

/// <summary>
/// Runtime instance of a card on the board or in hand.
/// </summary>
public class CardInstance
{
    public CardData Data        { get; private set; }
    public int      CurrentPower { get; private set; }
    public bool     IsFrozen    { get; set; }   // weather debuff
    public int      OwnerIndex  { get; private set; } // 0=player, 1=enemy

    public CardInstance(CardData data, int ownerIndex)
    {
        Data         = data;
        CurrentPower = data.basePower;
        OwnerIndex   = ownerIndex;
    }

    public void SetPower(int value) => CurrentPower = Mathf.Max(0, value);
    public void AddPower(int delta)  => SetPower(CurrentPower + delta);

    public void ApplyWeather()
    {
        if (Data.isHero) return;
        IsFrozen = true;
        SetPower(1);
    }

    public void RemoveWeather()
    {
        if (!IsFrozen) return;
        IsFrozen = false;
        SetPower(Data.basePower);
    }
}
