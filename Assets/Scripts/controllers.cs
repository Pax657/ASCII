using UnityEngine;
using UnityEngine.UI;

public class controllers : MonoBehaviour
{
    public Slider intensitySlider;
    public Upload_Image imageLoader; // referencia a tu script principal

    void Start()
    {
        intensitySlider.onValueChanged.AddListener(OnIntensityChanged);
    }

    void OnIntensityChanged(float value)
    {
        imageLoader.RecalculateAscii(value);
    }
}
