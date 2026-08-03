using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class controllers : MonoBehaviour
{
    [Header("Sliders")]
    public Slider intensitySlider;
    public Slider resolutionSlider;
    //public Slider thresholdSlider;

    [Header("Dropdown")]
    public TMP_Dropdown charsetDropdown;

    [Space(10)]
    [Header("Textos")]
    public TextMeshProUGUI intensityValueText;
    public TextMeshProUGUI resolutionValueText;
    //public TextMeshProUGUI thresholdValueText;

    [Space(10)]
    [Header("Imagen")]
    public Upload_Image imageLoader;

    public List<opcionesChar> charsets = new List<opcionesChar>
    {
        new opcionesChar ("ASCII Clásico", "@%#*+=-:. "),
        new opcionesChar ("Cuadros", "█▓▒░ " ),
        new opcionesChar ("Simplificado", "#. "),
    };


    void Start()
    {
        intensitySlider.onValueChanged.AddListener(OnIntensityChanged);
        resolutionSlider.onValueChanged.AddListener(OnResolutionChanged);
        //thresholdSlider.onValueChanged.AddListener(OnThresholdChanged);

        charsetDropdown.ClearOptions();
        List<string> names = charsets.ConvertAll(c => c.displayName);
        charsetDropdown.AddOptions(names);

        charsetDropdown.onValueChanged.AddListener(OnDropDownChanged);
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

    void OnDropDownChanged(int index)
    {
        imageLoader.OnCharsetChanged(charsets[index].densityTable);
    }
/*
    void OnThresholdChanged(float value)
    {
        thresholdValueText.text = $"Umbral: {value:F2}";
        imageLoader.RecalculateAlphaThreshold(value);
    }
*/
}
