using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ReignsClone/Card")]
public class CardData : ScriptableObject
{
    [Header("Card Info")]
    public string cardID;
    public string title;
    [TextArea] public string bodyText;

    [Header("Left Choice")]
    public string leftChoiceText;
    public int leftTimeDelta;
    public int leftMoneyDelta;
    public int leftEnergyDelta;
    public int leftRelationshipsDelta;

    [Header("Right Choice")]
    public string rightChoiceText;
    public int rightTimeDelta;
    public int rightMoneyDelta;
    public int rightEnergyDelta;
    public int rightRelationshipsDelta;

    [Header("Basic Conditions (Optional)")]
    public bool oneTimeUse = false;
    public bool used = false;

    public int minMoneyRequired = 0;
    public int minEnergyRequired = 0;

    public bool IsAvailable()
    {
        var gm = GameManager.Instance;
        if (gm == null) return false;

        if (used && oneTimeUse) return false;

        if (gm.money < minMoneyRequired) return false;
        if (gm.energy < minEnergyRequired) return false;

        return true;
    }

    public void MarkUsed()
    {
        if (oneTimeUse) used = true;
    }
}
