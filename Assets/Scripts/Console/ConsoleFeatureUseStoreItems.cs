using MobileConsole;
using UnityEngine;

[SettingCommand(name = "DrawIO", order = -1)]
public class ConsoleFeatureUseStoreItems : Command
{
    [Variable(OnValueChanged = "OnUseStoreItems")]
    public bool _useStoreItems = true;
    
    void OnUseStoreItems()
    {
        FeatureManager.Instance.SetUseStoreItems(_useStoreItems);
    }
}
