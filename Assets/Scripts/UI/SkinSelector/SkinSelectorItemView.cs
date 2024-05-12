using deVoid.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class SkinSelectorItemView : MonoBehaviour
{
    private SkinSelectorView _skinSelectorView;
    private SkinData _skinData;

    [SerializeField]
    private BrushMainMenu _brush;
    
    [SerializeField]
    private GameObject _costGameObject;
    [SerializeField]
    private TextMeshProUGUI _costText;

    private bool _isUnlock;
    
    public void SetData(SkinSelectorView skinSelectorView, SkinData skinData)
    {
        _skinData = skinData;
        _skinSelectorView = skinSelectorView;
        
        CheckPrice();
        
        Signals.Get<OnPurchaseSkinEvent>().AddListener(OnPurchaseSkin);
        CheckIfUnblock();
        
        SetBrush();
    }

    private void CheckPrice()
    {
        _costText.text = _skinData.Cost.ToString();
        if (!StatsManager.Instance.CanPurchase(_skinData))
        {
            _costText.color = Color.red;
        }
        else
        {
            _costText.color = Color.white;
        }
    }

    private void OnPurchaseSkin(SkinData skinData)
    {
        if (_skinData == skinData)
        {
            CheckIfUnblock();
        }
    }

    private void CheckIfUnblock()
    {
        if (_skinData.Cost <= 0 || StatsManager.Instance.IsPurchased(_skinData) || FeatureManager.Instance.UseStoreItems)
        {
            Unlock();
        }
    }

    public void Unlock()
    {
        _costGameObject.SetActive(false);
        _isUnlock = true;
    }

    private void SetBrush()
    {
        _brush.Set(_skinData);
    }

    public void OnClick()
    {
        _skinSelectorView.OnSelectConfig(_skinData);
    }
    
    public void OnClickInPurchase()
    {
        if (StatsManager.Instance.CanPurchase(_skinData))
        {
            StatsManager.Instance.Purchase(_skinData);
        }
    }

    public void SetView(bool visibility)
    {
        _brush.gameObject.SetActive(visibility);
        CheckPrice();
    }
}
