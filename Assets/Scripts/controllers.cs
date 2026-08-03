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
    public Slider densitySlider;
    //public Slider thresholdSlider;

    [Header("Dropdown")]
    public TMP_Dropdown charsetDropdown;
    public TMP_Dropdown fontDropdown;

    [Space(10)]
    [Header("Textos")]
    public TextMeshProUGUI intensityValueText;
    public TextMeshProUGUI resolutionValueText;
    public TextMeshProUGUI densityValueText;
    //public TextMeshProUGUI thresholdValueText;

    [Space(10)]
    [Header("Imagen")]
    public Upload_Image imageLoader;

    [Space(10)]
    public List<opcionesChar> charsets = new List<opcionesChar>
    {
        new opcionesChar ("ASCII Clásico", "@%#*+=-:. "),
        new opcionesChar ("Cuadros", "█▓▒░ " ),
        new opcionesChar ("Simplificado", "#. "),
    };

    [Space(10)]
    public TextMeshProUGUI asciiDisplay;
    public List<TMP_FontAsset> availableFonts;

    void Start()
    {
        intensitySlider.onValueChanged.AddListener(OnIntensityChanged);
        resolutionSlider.onValueChanged.AddListener(OnResolutionChanged);
        densitySlider.onValueChanged.AddListener(onDensityChanged);
        //thresholdSlider.onValueChanged.AddListener(OnThresholdChanged);

        //Configurar el Dropdown de charsets
        charsetDropdown.ClearOptions();
        List<string> namesChar = charsets.ConvertAll(c => c.displayName);
        charsetDropdown.AddOptions(namesChar);

        charsetDropdown.onValueChanged.AddListener(OnASCIIChanged);

        //Configurar el Dropdown de fuentes
        fontDropdown.ClearOptions();
        List<string> namesFont = availableFonts.ConvertAll(f => f.name);
        fontDropdown.AddOptions(namesFont);

        fontDropdown.onValueChanged.AddListener(OnFontChanged);
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

    void OnASCIIChanged(int index)
    {
        string newTable = charsets[index].densityTable;
        imageLoader.OnCharsetChanged(newTable);

        // Ajusta el rango del slider de niveles según el charset elegido
        densitySlider.maxValue = newTable.Length;

        if (densitySlider.value > newTable.Length)
        {
            densitySlider.value = newTable.Length;
        }
    }

    void OnFontChanged(int index)
    {
        asciiDisplay.font = availableFonts[index];
    }

    void onDensityChanged(float value)
    {
        int levels = Mathf.RoundToInt(value);
        densityValueText.text = $"Escala de Gris: {levels}";
        imageLoader.OnDensityLevelsChanged(levels);
    }

    /*
        void OnThresholdChanged(float value)
        {
            thresholdValueText.text = $"Umbral: {value:F2}";
            imageLoader.RecalculateAlphaThreshold(value);
        }
    */
}
