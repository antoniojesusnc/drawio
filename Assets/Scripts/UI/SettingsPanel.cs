using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour 
{
    public Image m_VibrationButton;
    public Sprite m_VibrationOnSprite;
    public Sprite m_VibrationOffSprite;
    public Animator m_BarAnim;

    // Cache
    private MobileHapticManager m_Haptic;

    // Buffer
    private bool m_PanelVisible;

    private bool Vibration
    {
        get
        {
            return (MobileHapticManager.s_Vibrate);
        }
        set
        {
            MobileHapticManager.s_Vibrate = value;
            PlayerPrefs.SetInt(Constants.c_VibrationSave, value ? 1 : 0); // Converting bool to int
        }
    }

    private void Awake()
    {
        m_Haptic = MobileHapticManager.Instance;
        Vibration = PlayerPrefs.GetInt(Constants.c_VibrationSave, 1) == 1; // Converting int to bool

        m_PanelVisible = false;

        m_BarAnim.SetBool("Visible", m_PanelVisible);
    }

    public void ClickVibrateButton()
    {
        Vibration = !Vibration;
        RefreshButtonsVisual();

        if (Vibration)
            m_Haptic.Vibrate(MobileHapticManager.E_FeedBackType.ImpactHeavy);
    }

    public void ClickSettingsButton()
    {
        m_PanelVisible = !m_PanelVisible;
        m_BarAnim.SetBool("Visible", m_PanelVisible);
    }

    private void RefreshButtonsVisual()
    {
        m_VibrationButton.sprite = Vibration ? m_VibrationOnSprite : m_VibrationOffSprite;
    }
}
