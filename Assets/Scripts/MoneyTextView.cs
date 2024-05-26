using TMPro;
using UnityEngine;

public class MoneyTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private DataSO _dataSO;

    private void Start()
    {
        _dataSO.OnCoinsChangedEvent += ChangeText;
    }

    private void ChangeText(int newValue)
    {
        _textField.text = "Money = " + newValue;
    }

    private void OnDestroy()
    {
        _dataSO.OnCoinsChangedEvent -= ChangeText;
    }
}
