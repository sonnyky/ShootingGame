using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PowerUpScriptableObject", order = 1)]
public class PowerUpScriptableObject : ScriptableObject
{
    public string powerUpTarget;
    public string powerUpType;
}