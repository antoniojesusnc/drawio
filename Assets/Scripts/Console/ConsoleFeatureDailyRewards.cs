using MobileConsole;

[SettingCommand(name = "DrawIO", order = -1)]
public class ConsoleFeatureDailyRewards : Command
{
    [Variable(OnValueChanged = "OnDailyRewards")]
    public bool _dailyRewards = true;
    
    void OnDailyRewards()
    {
        FeatureManager.Instance.SetDailyReward(_dailyRewards);
    }
}
