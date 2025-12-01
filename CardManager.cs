using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("Card Pool")]
    public List<CardData> allCards;

    [Header("Repeat Settings")]
    [SerializeField] private int recentHistorySize = 15;

    private readonly System.Collections.Generic.Queue<CardData> recentCards =
        new System.Collections.Generic.Queue<CardData>();

    private CardData _currentCard;

    public float lifeEventChance = 0.2f;

    public List<LifeEventData> lifeEvents;

    private bool RollForLifeEvent()
    {
        return Random.value <= lifeEventChance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        DrawNextCard();
    }

    public void DrawNextCard()
    {
        var availableCards = allCards
            .Where(c => c.IsAvailable() && !recentCards.Contains(c))
            .ToList();

        if (availableCards.Count == 0)
        {
            availableCards = allCards
                .Where(c => c.IsAvailable())
                .ToList();
        }

        if (availableCards.Count == 0)
        {
            Debug.LogWarning("No available cards to draw.");
            return;
        }

        int idx = Random.Range(0, availableCards.Count);
        _currentCard = availableCards[idx];

        RegisterCardShown(_currentCard);

        UIManager.Instance.UpdateCardUI(_currentCard);
        UIManager.Instance.UpdateStatsUI();
    }

    private void RegisterCardShown(CardData card)
    {
        if (card == null) return;

        recentCards.Enqueue(card);

        if (recentCards.Count > recentHistorySize)
        {
            recentCards.Dequeue();
        }
    }
    public void ChooseLeft()
    {
        if (_currentCard == null) return;

        var gm = GameManager.Instance;

        gm.ApplyStatChanges(
            _currentCard.leftTimeDelta,
            _currentCard.leftMoneyDelta,
            _currentCard.leftEnergyDelta,
            _currentCard.leftRelationshipsDelta
        );

        if (GameOverManager.Instance != null &&
            GameOverManager.Instance.gameOverPanel.activeSelf)
        {
            return;
        }

        _currentCard.MarkUsed();

        gm.RegisterCardResolved();
        UIManager.Instance.OnCardResolved();

        if (RollForLifeEvent())
        {
            LifeEventUIManager.Instance.ShowLifeEventChoices();
            return;
        }

        DrawNextCard();
    }

    public void ChooseRight()
    {
        if (_currentCard == null) return;

        var gm = GameManager.Instance;

        gm.ApplyStatChanges(
            _currentCard.rightTimeDelta,
            _currentCard.rightMoneyDelta,
            _currentCard.rightEnergyDelta,
            _currentCard.rightRelationshipsDelta
        );

        if (GameOverManager.Instance != null &&
            GameOverManager.Instance.gameOverPanel.activeSelf)
        {
            return;
        }

        _currentCard.MarkUsed();

        gm.RegisterCardResolved();
        UIManager.Instance.OnCardResolved();

        if (RollForLifeEvent())
        {
            LifeEventUIManager.Instance.ShowLifeEventChoices();
            return;
        }

        DrawNextCard();
    }


}
