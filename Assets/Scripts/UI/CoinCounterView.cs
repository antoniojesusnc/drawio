using System;
using Coffee.UIExtensions;
using deVoid.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinCounterView : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem _particleSystem;

    [SerializeField] 
    private TextMeshProUGUI _text;
    
    [SerializeField] 
    private TextNumberAnimation _animation;

    private int _lastCoinsObtained;
    private Tween _currentAnimation;

    private void Start()
    {
        _particleSystem.Stop();
        _particleSystem.gameObject.SetActive(false);
        
        Signals.Get<OnCoinIncreasedEvent>().AddListener(OnCoinIncreased);
        InitialValue();
    }

    private void OnDisable()
    {
        _currentAnimation.Kill();
        _currentAnimation = null;
    }

    private void InitialValue()
    {
        _text.text = StatsManager.Instance.GetCoins().ToString();
    }

    private void OnCoinIncreased(int coinIncreased, Vector3 originPosition)
    {
        _lastCoinsObtained = coinIncreased;
        
        if (originPosition != Vector3.zero)
        {
            SetParticles(originPosition);
        }
        else
        {
            DoCoinAnimation();
        }
    }
    
    private void SetParticles(Vector3 originPosition)
    {
        _particleSystem.transform.position = originPosition;
        _particleSystem.gameObject.SetActive(true);
        _particleSystem.Emit(2);
    }

    public void OnAttrackParticle()
    {
        if (_currentAnimation == null)
        {
            DoCoinAnimation();
        }
        
        _particleSystem.Stop();
    }
    
    private void DoCoinAnimation()
    {
        var coins = StatsManager.Instance.GetCoins();
        if (_animation != null)
        {
            _currentAnimation = _animation.PlayEffect(_text, coins - _lastCoinsObtained, coins);
            _currentAnimation.onComplete += OnFinishCoinAnimation;
        }
    }

    private void OnFinishCoinAnimation()
    {
        Signals.Get<OnCoinAnimationFinishedEvent>().Dispatch();
    }
}
