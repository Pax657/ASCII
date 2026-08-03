using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class controllers : MonoBehaviour
{
    [Header("Sliders")]
    public Slider intensitySlider;
    public Slider resolutionSlider;

    [Space(10)]
    [Header("Textos")]
    public TextMeshProUGUI intensityValueText;
    public TextMeshProUGUI resolutionValueText;

    [Space(10)]
    [Header("Imagen")]
    public Upload_Image imageLoader;


    void Start()
    {
        intensitySlider.onValueChanged.AddListener(OnIntensityChanged);
        resolutionSlider.onValueChanged.AddListener(OnResolutionChanged);
    }

    void OnIntensityChanged(float value)
    {
        intensityValueText.text = $"Iluminación: {value:F2}";
        imageLoader.RecalculateAscii(value);
    }

    void OnResolutionChanged(float value)
    {
        resolutionValueText.text = $"Resolución: {(int)value} px";
        imageLoader.RecalculateGrid((int)value);
    }
}
