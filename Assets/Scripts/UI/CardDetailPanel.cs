using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Large card preview shown when hovering a card (bottom-left corner).
/// </summary>
public class CardDetailPanel : MonoBehaviour
{
    public Image    artworkImage;
    public TMP_Text cardName;
    public TMP_Text cardPower;
    public TMP_Text cardDescription;
    public TMP_Text cardAbility;
    public TMP_Text cardFaction;
    public Image    factionBar;

    [Header("Faction Colors")]
    public Color kingdomColor = new Color(0.2f, 0.4f, 0.8f);
    public Color empireColor  = new Color(0.7f, 0.1f, 0.1f);
    public Color undeadColor  = new Color(0.3f, 0.1f, 0.5f);
    public Color elvesColor   = new Color(0.1f, 0.5f, 0.2f);
    public Color neutralColor = new Color(0.5f, 0.4f, 0.1f);

    void Awake() => gameObject.SetActive(false);

    public void Show(CardInstance card)
    {
        if (card == null) return;
        gameObject.SetActive(true);

        var d = card.Data;
        if (cardName)        cardName.text        = d.cardName;
        if (cardPower)       cardPower.text        = d.type == CardType.Weather || d.type == CardType.Special
                                                     ? "—" : card.CurrentPower.ToString();
        if (cardDescription) cardDescription.text = d.description;
        if (cardAbility)     cardAbility.text      = BuildAbilityText(d);
        if (cardFaction)     cardFaction.text      = $"{d.faction}  ·  {d.type}  ·  {RowLabel(d.row)}";
        if (artworkImage && d.artwork) artworkImage.sprite = d.artwork;

        if (factionBar) factionBar.color = d.faction switch
        {
            CardFaction.Kingdom => kingdomColor,
            CardFaction.Empire  => empireColor,
            CardFaction.Undead  => undeadColor,
            CardFaction.Elves   => elvesColor,
            _                   => neutralColor,
        };
    }

    public void Hide() => gameObject.SetActive(false);

    string BuildAbilityText(CardData d)
    {
        var parts = new System.Collections.Generic.List<string>();
        if (d.isHero)           parts.Add("Héroe (inmune al clima)");
        if (d.hasSpy)           parts.Add("Espía: se juega en la fila enemiga, robas 2 cartas");
        if (d.hasMedic)         parts.Add("Médico: revive una unidad del descarte");
        if (d.hasMuster)        parts.Add("Convocatoria: invoca todas las copias del mazo");
        if (d.hasTightBond)     parts.Add("Vínculo: se multiplica por cada copia en la fila");
        if (d.hasMorale)        parts.Add("Moral: +1 a todos los aliados de la fila");
        if (d.hasScorch)        parts.Add("Chamusquina: destruye la unidad más fuerte del campo");
        if (d.hasDecoy)         parts.Add("Señuelo: intercambia con una unidad del campo");
        if (d.hasCommanderHorn) parts.Add("Cuerno: dobla la fuerza de toda la fila");
        return parts.Count > 0 ? string.Join("\n", parts) : "Sin habilidad especial";
    }

    string RowLabel(CardRow r) => r switch
    {
        CardRow.Melee  => "Cuerpo a cuerpo",
        CardRow.Ranged => "Distancia",
        CardRow.Siege  => "Asedio",
        _              => "Cualquier fila",
    };
}
