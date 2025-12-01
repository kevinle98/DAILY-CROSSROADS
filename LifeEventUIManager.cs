using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LifeEventUIManager : MonoBehaviour
{
    public static LifeEventUIManager Instance;

    [Header("Panels")]
    public GameObject lifeEventUI;
    public GameObject gameUI;

    [Header("Text")]
    public TextMeshProUGUI descriptionText;

    [Header("Buttons")]
    public Button[] eventButtons;
    public Button continueButton;

    private LifeEventData[] selectedEvents = new LifeEventData[4];
    private bool hasChosen = false;

    private void Awake()
    {
        Instance = this;
        lifeEventUI.SetActive(false);
    }

    public void ShowLifeEventChoices()
    {
        if (GameOverManager.Instance != null &&
            GameOverManager.Instance.gameOverPanel.activeSelf)
        {
            return;
        }

        hasChosen = false;

        gameUI.SetActive(false);
        lifeEventUI.SetActive(true);

        descriptionText.text = "Choose one and see what happens.";
        continueButton.interactable = false;

        for (int i = 0; i < eventButtons.Length; i++)
        {
            int index = i;

            selectedEvents[i] = CardManager.Instance.lifeEvents[
                Random.Range(0, CardManager.Instance.lifeEvents.Count)
            ];

            var button = eventButtons[i];
            button.interactable = true;

            var btnText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = "?";

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnLifeEventButtonClicked(index));
        }
    }

    private void OnLifeEventButtonClicked(int index)
    {
        if (hasChosen) return;
        hasChosen = true;

        LifeEventData chosen = selectedEvents[index];

        GameManager.Instance.ApplyStatChanges(
            chosen.timeDelta,
            chosen.moneyDelta,
            chosen.energyDelta,
            chosen.relationshipsDelta
        );

        var chosenButton = eventButtons[index];
        var chosenText = chosenButton.GetComponentInChildren<TextMeshProUGUI>();
        if (chosenText != null)
            chosenText.text = chosen.eventName;

        for (int i = 0; i < eventButtons.Length; i++)
        {
            eventButtons[i].interactable = false;
        }

        descriptionText.text = chosen.eventDescription;

        continueButton.interactable = true;
    }

    public void OnContinueButtonPressed()
    {
        if (!hasChosen) return;

        lifeEventUI.SetActive(false);

        if (GameOverManager.Instance != null &&
            GameOverManager.Instance.gameOverPanel.activeSelf)
        {
            return;
        }

        GameManager.Instance.RegisterCardResolved();
        UIManager.Instance.OnCardResolved();

        gameUI.SetActive(true);
        CardManager.Instance.DrawNextCard();
    }

}
