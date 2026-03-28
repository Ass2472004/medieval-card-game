using UnityEngine;

public enum CardType { Unit, Special, Weather, Leader }
public enum CardRow  { Melee, Ranged, Siege, Any }
public enum CardFaction { Kingdom, Empire, Elves, Undead, Neutral }

[CreateAssetMenu(fileName = "NewCard", menuName = "MedievalCards/Card")]
public class CardData : ScriptableObject
{
    [Header("Identity")]
    public string cardName;
    [TextArea] public string description;
    public Sprite artwork;
    public CardFaction faction;

    [Header("Stats")]
    public CardType  type;
    public CardRow   row;
    public int       basePower;
    public bool      isHero;          // heroes immune to weather/special

    [Header("Abilities")]
    public bool hasMuster;            // summons all copies from deck/discard
    public bool hasSpy;               // goes to enemy row, draw 2 cards
    public bool hasMedic;             // revive a unit from discard
    public bool hasTightBond;         // doubles power if same card in row
    public bool hasMorale;            // +1 to all other units in row
    public bool hasScorch;            // destroys highest-power unit on field
    public bool hasDecoy;             // swap with unit on field, return to hand
    public bool hasCommanderHorn;     // doubles power of all units in row
}
