using UnityEngine;

public class PrizePivot : MonoBehaviour
{
    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private Transform prizeParent;

    public void SpawnPrize(Prize prize)
    {
        if(prizeParent.childCount > 0)
        {
            Destroy(prizeParent.GetChild(0).gameObject);
        }
        var spawnedObject = Instantiate(prize.displayPrefab, prizeParent);
        spawnedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        spawnedObject.transform.localPosition = Vector3.zero;
        var display = spawnedObject.GetComponent<PrizeDisplay>();
        if(display != null)
        {
            display.SetupDisplay(prize);
        } 
    }

    public void SetRotation(float angle)
    {
        pivot.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}
