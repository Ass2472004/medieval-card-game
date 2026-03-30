using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using DG.Tweening;   // DOTween for animations (add to Packages/manifest.json)

public class CardView : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    public Image    artworkImage;
    public Image    cardFrame;
    public TMP_Text nameText;
    public TMP_Text powerText;
    public Image    factionIcon;
    public Image    typeIcon;
    public Image    heroGlow;
    public Image    frozenOverlay;
    public Image    selectedHighlight;
    public Image    abilityIcon;
    public TMP_Text abilityText;

    [Header("Faction Colors")]
    public Color kingdomColor = new Color(0.2f, 0.4f, 0.8f);
    public Color empireColor  = new Color(0.7f, 0.1f, 0.1f);
    public Color undeadColor  = new Color(0.3f, 0.1f, 0.5f);
    public Color elvesColor   = new Color(0.1f, 0.5f, 0.2f);
    public Color neutralColor = new Color(0.5f, 0.4f, 0.1f);

    [Header("Animation")]
    public float hoverLift    = 30f;
    public float hoverScale   = 1.12f;
    public float animDuration = 0.15f;

    // Events
    public Action<CardView>     OnCardClicked;
    public Action<CardInstance> OnCardHovered;
    public Action               OnCardUnhovered;

    private CardInstance _card;
    private bool         _isInHand;
    private Vector3      _basePos;
    private bool         _isHovered;

    // ── Init ────────────────────────────────────────────────────────────────
    public void Init(CardInstance card, bool isInHand)
    {
        _card    = card;
        _isInHand = isInHand;
        _basePos  = transform.localPosition;
        Refresh();
    }

    public void Refresh()
    {
        if (_card == null) return;
        var d = _card.Data;

        // Text
        if (nameText)  nameText.text  = d.cardName;
        if (powerText) powerText.text = d.type == CardType.Weather || d.type == CardType.Special
                                        ? "—" : _card.CurrentPower.ToString();

        // Artwork
        if (artworkImage && d.artwork) artworkImage.sprite = d.artwork;

        // Frame color by faction
        if (cardFrame) cardFrame.color = FactionColor(d.faction);

        // Hero glow
        if (heroGlow) heroGlow.gameObject.SetActive(d.isHero);

        // Frozen
        if (frozenOverlay) frozenOverlay.gameObject.SetActive(_card.IsFrozen);

        // Power color: buffed=green, debuffed=red, normal=white
        if (powerText)
        {
            powerText.color = _card.CurrentPower > d.basePower ? Color.green :
                              _card.CurrentPower < d.basePower ? Color.red   : Color.white;
        }

        // Ability label
        if (abilityText) abilityText.text = GetAbilityLabel(d);
    }

    string GetAbilityLabel(CardData d)
    {
        if (d.isHero)            return "Héroe";
        if (d.hasSpy)            return "Espía";
        if (d.hasMedic)          return "Médico";
        if (d.hasMuster)         return "Convocatoria";
        if (d.hasTightBond)      return "Vínculo";
        if (d.hasMorale)         return "Moral";
        if (d.hasScorch)         return "Chamusquina";
        if (d.hasDecoy)          return "Señuelo";
        if (d.hasCommanderHorn)  return "Cuerno";
        return d.type == CardType.Weather ? "Clima"   :
               d.type == CardType.Special ? "Especial" : "";
    }

    Color FactionColor(CardFaction f) => f switch
    {
        CardFaction.Kingdom => kingdomColor,
        CardFaction.Empire  => empireColor,
        CardFaction.Undead  => undeadColor,
        CardFaction.Elves   => elvesColor,
        _                   => neutralColor,
    };

    // ── Interaction ─────────────────────────────────────────────────────────
    public void OnPointerClick(PointerEventData _)
    {
        if (_isInHand) OnCardClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData _)
    {
        _isHovered = true;
        OnCardHovered?.Invoke(_card);

        if (_isInHand)
        {
            transform.DOLocalMoveY(_basePos.y + hoverLift, animDuration).SetEase(Ease.OutQuad);
            transform.DOScale(hoverScale, animDuration).SetEase(Ease.OutQuad);
            transform.SetAsLastSibling();
        }
    }

    public void OnPointerExit(PointerEventData _)
    {
        _isHovered = false;
        OnCardUnhovered?.Invoke();

        if (_isInHand)
        {
            transform.DOLocalMoveY(_basePos.y, animDuration).SetEase(Ease.OutQuad);
            transform.DOScale(1f, animDuration).SetEase(Ease.OutQuad);
        }
    }

    public void SetSelected(bool sel)
    {
        if (selectedHighlight) selectedHighlight.gameObject.SetActive(sel);
    }

    public CardInstance GetCard() => _card;
}
