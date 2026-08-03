using UnityEngine;
using UnityEngine.UI;

public class controllers : MonoBehaviour
{
    //sliders
    public Slider intensitySlider;
    public Slider resolutionSlider;

    [Space(10)]
    public Upload_Image imageLoader;


    void Start()
    {
        intensitySlider.onValueChanged.AddListener(OnIntensityChanged);
        resolutionSlider.onValueChanged.AddListener(OnResolutionChanged);
    }

    void OnIntensityChanged(float value)
    {
        imageLoader.RecalculateAscii(value);
    }

    void OnResolutionChanged(float value)
    {
        imageLoader.RecalculateGrid((int)value);
    }
}
