using DG.Tweening;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TextNumberAnimation", menuName = "Data/AnimationEffects/TextNumberAnimation", order = 1)]

public class TextNumberAnimation : GenericDotweenConfig<TextMeshProUGUI, int, int>
{
    [field: SerializeField]
    public float Duration { get; private set; }
    
    [field: SerializeField]
    public Ease _ease;

    public override void PlayEffect(TextMeshProUGUI text, int initialNumber, int endNumber)
    {
        DOTween.To(() => initialNumber, (number) => text.text = number.ToString(), endNumber, Duration).SetEase(_ease);
    }
}