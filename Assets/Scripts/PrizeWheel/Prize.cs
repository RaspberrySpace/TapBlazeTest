using UnityEngine;

[CreateAssetMenu(fileName = "Prize", menuName = "PrizeWheel/Prize")]
public class Prize : ScriptableObject
{
    public string id;
    public GameObject displayPrefab;
    public int count;
}
