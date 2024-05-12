using deVoid.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinCounterView : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _text;
    
    [SerializeField] 
    private TextNumberAnimation _animation;

    private void Start()
    {
        Signals.Get<OnCoinIncreasedEvent>().AddListener(OnCoinIncreased);
        InitialValue();
    }

    private void InitialValue()
    {
        _text.text = StatsManager.Instance.GetCoins().ToString();
    }

    private void OnCoinIncreased(int coinIncreased)
    {
        var coins = StatsManager.Instance.GetCoins();
        if (_animation != null)
        {
            _animation.PlayEffect(_text, coins - coinIncreased, coins);
        }
    }
}
