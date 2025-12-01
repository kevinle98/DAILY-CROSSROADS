using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Card UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI leftButtonText;
    public TextMeshProUGUI rightButtonText;

    [Header("Stat Sliders")]
    public Slider timeSlider;
    public Slider moneySlider;
    public Slider energySlider;
    public Slider relationshipsSlider;

    public Image timeFill;
    public Image moneyFill;
    public Image energyFill;
    public Image relationshipsFill;

    [Header("Stat Colors")]
    public Color timeVisibleColor;
    public Color moneyVisibleColor;
    public Color energyVisibleColor;
    public Color relationshipsVisibleColor;
    public Color hiddenColor = Color.gray;

    [Header("Hints UI")]
    public Button hintButton;
    public TextMeshProUGUI hintCountText;

    private bool hintActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (timeVisibleColor == default)
            timeVisibleColor = new Color32(53, 114, 162, 255);

        if (moneyVisibleColor == default)
            moneyVisibleColor = new Color32(77, 142, 56, 255);

        if (energyVisibleColor == default)
            energyVisibleColor = new Color32(211, 175, 33, 255);

        if (relationshipsVisibleColor == default)
            relationshipsVisibleColor = new Color32(165, 39, 40, 255);
    }

    public TextMeshProUGUI cardCountText;

    public void UpdateCardCountUI()
    {
        var gm = GameManager.Instance;
        if (gm == null || cardCountText == null) return;

        cardCountText.text = $"Score: {gm.cardsResolved}";
    }

    public void UpdateCardUI(CardData card)
    {
        if (card == null) return;

        titleText.text = card.title;
        bodyText.text = card.bodyText;
        leftButtonText.text = card.leftChoiceText;
        rightButtonText.text = card.rightChoiceText;

        UpdateStatsUI();
        UpdateHintUI();
        UpdateCardCountUI();
    }

    public void UpdateStatsUI()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        bool isGameOver = GameOverManager.Instance != null &&
                          GameOverManager.Instance.gameOverPanel.activeSelf;

        bool showReal = gm.cardsResolved < 5 || hintActive || isGameOver;

        float displayedTime;
        float displayedMoney;
        float displayedEnergy;
        float displayedRelationships;

        if (showReal)
        {
            displayedTime = gm.time;
            displayedMoney = gm.money;
            displayedEnergy = gm.energy;
            displayedRelationships = gm.relationships;

            SetSliderColorsVisible();
        }
        else
        {
            displayedTime = 50f;
            displayedMoney = 50f;
            displayedEnergy = 50f;
            displayedRelationships = 50f;

            SetSliderColorsHidden();
        }

        timeSlider.value = displayedTime;
        moneySlider.value = displayedMoney;
        energySlider.value = displayedEnergy;
        relationshipsSlider.value = displayedRelationships;
    }


    private void SetSliderColorsVisible()
    {
        if (timeFill != null) timeFill.color = timeVisibleColor;
        if (moneyFill != null) moneyFill.color = moneyVisibleColor;
        if (energyFill != null) energyFill.color = energyVisibleColor;
        if (relationshipsFill != null) relationshipsFill.color = relationshipsVisibleColor;
    }

    private void SetSliderColorsHidden()
    {
        if (timeFill != null) timeFill.color = hiddenColor;
        if (moneyFill != null) moneyFill.color = hiddenColor;
        if (energyFill != null) energyFill.color = hiddenColor;
        if (relationshipsFill != null) relationshipsFill.color = hiddenColor;
    }

    public void UpdateHintUI()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        if (hintCountText != null)
            hintCountText.text = $"Hints: {gm.hintsRemaining}";

        bool isGameOver = GameOverManager.Instance != null &&
                          GameOverManager.Instance.gameOverPanel.activeSelf;

        bool canUseHint = gm.cardsResolved >= 5 &&
                          gm.hintsRemaining > 0 &&
                          !hintActive &&
                          !isGameOver;

        if (hintButton != null)
            hintButton.interactable = canUseHint;
    }

    public void OnHintButtonClicked()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        if (gm.cardsResolved < 5) return;
        if (gm.hintsRemaining <= 0) return;
        if (hintActive) return;

        gm.hintsRemaining--;
        hintActive = true;

        UpdateStatsUI();
        UpdateHintUI();
    }

    public void OnCardResolved()
    {
        hintActive = false;
        UpdateStatsUI();
        UpdateHintUI();
        UpdateCardCountUI();
    }
}
