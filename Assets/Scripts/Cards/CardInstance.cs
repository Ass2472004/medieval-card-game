using UnityEngine;

public class CardInstance
{
    public CardData Data         { get; private set; }
    public int      CurrentPower { get; private set; }
    public bool     IsFrozen     { get; private set; }
    public int      OwnerIndex   { get; private set; }

    private int _powerBeforeWeather;

    public CardInstance(CardData data, int ownerIndex)
    {
        Data          = data;
        CurrentPower  = data.basePower;
        OwnerIndex    = ownerIndex;
    }

    public void SetPower(int value) => CurrentPower = Mathf.Max(0, value);
    public void AddPower(int delta) => SetPower(CurrentPower + delta);

    public void ApplyWeather()
    {
        if (Data.isHero || IsFrozen) return;
        _powerBeforeWeather = CurrentPower;
        IsFrozen            = true;
        CurrentPower        = 1;
    }

    public void RemoveWeather()
    {
        if (!IsFrozen) return;
        IsFrozen     = false;
        CurrentPower = _powerBeforeWeather > 0 ? _powerBeforeWeather : Data.basePower;
    }

    public override string ToString() => $"{Data.cardName} [{CurrentPower}]";
}
