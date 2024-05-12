using MobileConsole;

[SettingCommand(name = "DrawIO", order = -1)]
public class ConsoleFeatureBrushCollider : Command
{
    [Variable(OnValueChanged = "OnBrushCollider")]
    public bool _onBrushCollider = true;
    
    void OnBrushCollider()
    {
        FeatureManager.Instance.SetBrushCollider(_onBrushCollider);
    }
}
