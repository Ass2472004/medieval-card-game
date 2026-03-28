using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public Image    artworkImage;
    public TMP_Text nameText;
    public TMP_Text powerText;
    public Image    factionBadge;
    public Image    heroGlow;
    public GameObject frozenOverlay;

    [Header("Animation")]
    public float hoverScale   = 1.15f;
    public float animSpeed    = 8f;

    private CardInstance _card;
    private bool         _isInHand;
    private Vector3      _baseScale;

    public System.Action<CardView> OnCardClicked;

    void Awake() => _baseScale = transform.localScale;

    void Update()
    {
        // smooth scale animation
        transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, Time.deltaTime * animSpeed);
    }

    public void Init(CardInstance card, bool isInHand)
    {
        _card    = card;
        _isInHand = isInHand;
        Refresh();
    }

    public void Refresh()
    {
        if (_card == null) return;
        nameText.text  = _card.Data.cardName;
        powerText.text = _card.CurrentPower.ToString();
        if (_card.Data.artwork != null) artworkImage.sprite = _card.Data.artwork;
        if (heroGlow  != null) heroGlow.enabled    = _card.Data.isHero;
        if (frozenOverlay != null) frozenOverlay.SetActive(_card.IsFrozen);
    }

    public void OnPointerClick(PointerEventData _)
    {
        if (_isInHand) OnCardClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (_isInHand) _baseScale = transform.localScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData _)
    {
        _baseScale = Vector3.one;
    }

    public CardInstance GetCard() => _card;
}
