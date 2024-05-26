using UnityEngine;

[CreateAssetMenu]
public class DataSO : ScriptableObject
{
    public delegate void CoinsChangedDelegate(int newValue);
    public event CoinsChangedDelegate OnCoinsChangedEvent;

    private int _coinsAmount;
    
    public int CoinsAmount
    {
        get { return _coinsAmount; }
    }

    private void OnEnable()
    {
        _coinsAmount = 200;
    }

    public void Remove20Coins()
    {
        if (_coinsAmount == 0)
        {
            return;
        }

        _coinsAmount -= 20;
        if (_coinsAmount < 0)
        {
            _coinsAmount = 0;
        }

        OnCoinsChangedEvent?.Invoke(_coinsAmount);
    }

    public void AddCoins(int amount)
    {
        if (_coinsAmount == 0)
        {
            return;
        }

        _coinsAmount += amount;

        OnCoinsChangedEvent?.Invoke(_coinsAmount);
    }
}
