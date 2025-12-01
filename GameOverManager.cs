using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public GameObject gameOverPanel;
    public GameObject gameUI;
    public TMPro.TextMeshProUGUI summaryText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameUI != null)
            gameUI.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        UIManager.Instance?.UpdateStatsUI();

        if (summaryText != null)
            summaryText.text = BuildSummaryText();
    }

    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InitializeRun();
        }

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void ReturnToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InitializeRun();
        }

        SceneManager.LoadScene("MainMenu");
    }

    private string BuildSummaryText()
    {
        var gm = GameManager.Instance;
        if (gm == null) return "";

        int time = gm.time;
        int money = gm.money;
        int energy = gm.energy;
        int relationships = gm.relationships;

        const int lowThreshold = 30;
        const int highThreshold = 70;

        var lines = new System.Collections.Generic.List<string>();

        // Time
        if (time <= lowThreshold)
            lines.Add("Time slipped away faster than you could hold onto it.");
        else if (time >= highThreshold)
            lines.Add("A careful guard over your schedule kept your days tightly controlled.");
        else
            lines.Add("You managed your time with some balance, even if imperfectly.");

        // Money
        if (money <= lowThreshold)
            lines.Add("Finances were often an afterthought, taking a back seat to other concerns.");
        else if (money >= highThreshold)
            lines.Add("Resources stayed well stocked, thanks to a strong focus on earning and saving.");
        else
            lines.Add("Money played a steady, functional role without dominating your decisions.");

        // Energy
        if (energy <= lowThreshold)
            lines.Add("Your energy drained faster than it could be restored, leaving little room for recovery.");
        else if (energy >= highThreshold)
            lines.Add("Plenty of rest and self-preservation kept burnout safely at bay.");
        else
            lines.Add("You pushed yourself when necessary while still finding time to recharge.");

        // Relationships
        if (relationships <= lowThreshold)
            lines.Add("Connections with others weakened, overshadowed by competing priorities.");
        else if (relationships >= highThreshold)
            lines.Add("A strong investment in others strengthened your social ties.");
        else
            lines.Add("Social bonds remained present, though not always at the center of your focus.");

        bool allMid =
            time > lowThreshold && time < highThreshold &&
            money > lowThreshold && money < highThreshold &&
            energy > lowThreshold && energy < highThreshold &&
            relationships > lowThreshold && relationships < highThreshold;

        if (lines.Count < 16 && allMid)
            lines.Add("A steady balance across every area defined your overall approach.");

        return string.Join(" ", lines);
    }

}
