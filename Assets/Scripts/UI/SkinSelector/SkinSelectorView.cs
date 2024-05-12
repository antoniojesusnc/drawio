using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectorView : View<SkinSelectorView>
{
    [Header("SkinSelectorView")]
    [SerializeField]
    private SkinSelectorViewConfig _config;
    
    [SerializeField]
    private Transform _scrollParent;
    
    [SerializeField]
    private Image _backButton;
    
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;
    
    [SerializeField]
    private BrushMainMenu _brush;

    private List<SkinSelectorItemView> _itemsView = new List<SkinSelectorItemView>();
    
    protected void Start()
    {
        InitBrush();
        GenerateBrushItems();
        
        // the size spend one frame in calculate
        DOVirtual.DelayedCall(0.1f, GridAutoSize);
    }

    private void GridAutoSize()
    {
        var size = _scrollParent.GetComponent<RectTransform>().rect.size;

        var cellSize = (size.y - _gridLayoutGroup.padding.top - _gridLayoutGroup.padding.bottom -
                     _gridLayoutGroup.spacing.y*(_config.ElementsPerColum-1)) / _config.ElementsPerColum;
        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _gridLayoutGroup.constraintCount = _config.ElementsPerColum;
    }

    private void InitBrush()
    {
        _brush.Set(GameManager.Instance.m_Skins[GameManager.Instance.m_PlayerSkinID]);
        _brush.gameObject.SetActive(false);
    }

    private void GenerateBrushItems()
    {
        for (int i = 0; i < _config.SkinSelectorItems.Count; i++)
        {
            var itemView = GetItemView();
            itemView.SetData(this, _config.SkinSelectorItems[i]);
            itemView.SetView(false);
            _itemsView.Add(itemView);
        }
    }

    private SkinSelectorItemView GetItemView()
    {
        return GameObject.Instantiate(_config.SkinSelectorItemViewPrefab, _scrollParent);
    }

    public void OnBackButton()
    {
        if (m_GameManager.currentPhase == GamePhase.SKIN_SELECTOR)
            m_GameManager.ChangePhase(GamePhase.MAIN_MENU);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.MAIN_MENU:
                if (m_Visible)
                    Transition(false);
                break;
            case GamePhase.SKIN_SELECTOR:
                if (!m_Visible)
                    Transition(true);
                break;
        }
    }

    public override void OnBeginTransition(bool _InOrOut)
    {
        if (!_InOrOut)
        {
            _brush.gameObject.SetActive(false);
            _itemsView.ForEach(itemView => itemView.SetView(false));
        }
    }
    public override void OnFinishTransition(bool _InOrOut)
    {
        if(_InOrOut)
        {
            _brush.gameObject.SetActive(true);
            _itemsView.ForEach(itemView => itemView.SetView(true));
        }
    }

    public void OnSelectConfig(SkinData skinData)
    {
        var skinId = FindSkinID(skinData);
        GameManager.Instance.m_PlayerSkinID = skinId;
        _brush.Set(skinData);
        StatsManager.Instance.FavoriteSkin = skinId;
        GameManager.Instance.SetColor(GameManager.Instance.ComputeCurrentPlayerColor(true, 0));
    }

    private int FindSkinID(SkinData skinData)
    {
        return GameManager.Instance.m_Skins.FindIndex(skin => skin.Brush == skinData.Brush
                                                              && skin.Color == skinData.Color);
    }
}