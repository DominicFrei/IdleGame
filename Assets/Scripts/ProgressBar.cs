using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider = default;
    [SerializeField] private Gradient gradient = default;
    [SerializeField] private Image fillImage = default;

    public void SetProgressPercentage(float progressPercentage)
    {
        slider.value = progressPercentage;
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }
}
