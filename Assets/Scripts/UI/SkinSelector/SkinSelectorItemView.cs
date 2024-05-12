using UnityEngine;

public class SkinSelectorItemView : MonoBehaviour
{
    private SkinSelectorView _skinSelectorView;
    private SkinData _skinData;

    [SerializeField]
    private BrushMainMenu _brush;
    
    public void SetData(SkinSelectorView skinSelectorView, SkinData skinData)
    {
        _skinData = skinData;
        _skinSelectorView = skinSelectorView;

        SetBrush();
    }

    private void SetBrush()
    {
        _brush.Set(_skinData);
    }

    public void OnClick()
    {
        _skinSelectorView.OnSelectConfig(_skinData);
    }

    public void SetView(bool visibility)
    {
        _brush.gameObject.SetActive(visibility);
    }
}
