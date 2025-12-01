using UnityEngine;

[CreateAssetMenu(fileName = "NewLifeEvent", menuName = "ReignsClone/LifeEvent")]
public class LifeEventData : ScriptableObject
{
    public string eventName;
    [TextArea] public string eventDescription;

    public int timeDelta;
    public int moneyDelta;
    public int energyDelta;
    public int relationshipsDelta;
}
