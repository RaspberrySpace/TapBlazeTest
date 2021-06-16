using UnityEngine;
using TMPro;

public class PrizeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    [SerializeField]
    private string formatString = @"{0}";

    public void SetupDisplay(Prize prize)
    {
        textField.text = string.Format(formatString, prize.count);
    }
}
