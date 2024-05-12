using MobileConsole;

[SettingCommand(name = "DrawIO", order = -1)]
public class ConsoleFeatureUseSkinSelector : Command
{
    [Variable(OnValueChanged = "OnUseSkinSelector")]
    public bool _useSkinSelector = true;
    
    void OnUseSkinSelector()
    {
        FeatureManager.Instance.SetUseSkinSelector(_useSkinSelector);
    }
}
