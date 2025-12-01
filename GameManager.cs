using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Core Stats")]
    [Range(0, 100)] public int time = 50;
    [Range(0, 100)] public int money = 50;
    [Range(0, 100)] public int energy = 50;
    [Range(0, 100)] public int relationships = 50;

    [Header("Game Over Settings")]
    public int minStat = 0;
    public int maxStat = 100;

    [Header("Progress & Hints")]
    public int cardsResolved = 0;
    public int startingHints = 3;
    public int hintsRemaining;

    private int defaultTime = 50;
    private int defaultMoney = 50;
    private int defaultEnergy = 50;
    private int defaultRelationships = 50;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeRun();
    }

    public void InitializeRun()
    {
        time = defaultTime;
        money = defaultMoney;
        energy = defaultEnergy;
        relationships = defaultRelationships;

        cardsResolved = 0;
        hintsRemaining = startingHints;
    }

    public void RegisterCardResolved()
    {
        cardsResolved++;
        Debug.Log($"Cards resolved: {cardsResolved}");
    }

    public void ApplyStatChanges(int dTime, int dMoney, int dEnergy, int dRelationships)
    {
        time = Mathf.Clamp(time + dTime, minStat, maxStat);
        money = Mathf.Clamp(money + dMoney, minStat, maxStat);
        energy = Mathf.Clamp(energy + dEnergy, minStat, maxStat);
        relationships = Mathf.Clamp(relationships + dRelationships, minStat, maxStat);

        CheckGameOver();
    }

    private void CheckGameOver()
    {
        bool isDead =
            time <= minStat ||
            money <= minStat ||
            energy <= minStat ||
            relationships <= minStat;

        if (isDead)
        {
            Debug.Log("Game Over: Stat reached minimum");
            GameOverManager.Instance?.ShowGameOver();
        }
    }
}
