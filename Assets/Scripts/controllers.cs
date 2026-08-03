using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class controllers : MonoBehaviour
{
    [Header("Sliders")]
    public Slider iluminacionSlider;
    public Slider resolutionSlider;
    public Slider densitySlider;
    //public Slider thresholdSlider;

    [Header("Dropdown")]
    public TMP_Dropdown charsetDropdown;
    public TMP_Dropdown fontDropdown;

    [Space(10)]
    [Header("Textos")]
    public TextMeshProUGUI iluminacionValueText;
    public TextMeshProUGUI resolutionValueText;
    public TextMeshProUGUI densityValueText;
    //public TextMeshProUGUI thresholdValueText;

    [Space(10)]
    [Header("Imagen")]
    public Upload_Image imageLoader;

    [Space(10)]
    [Header("Opciones de caracteres")]
    public List<opcionesChar> charsets = new List<opcionesChar>
    {
        new opcionesChar ("ASCII Clásico", "@%#*+=-:. "),
        new opcionesChar ("Cuadros", "█▓▒░ " ),
        new opcionesChar ("Simplificado", "#. "),
    };

    [Space(10)]
    [Header("Opciones de fuente")]
    public TextMeshProUGUI asciiDisplay;
    public List<TMP_FontAsset> availableFonts;

    [Header("Debounce")] //Permite mejorar el rendimiento al evitar recalcular la imagen demasiadas veces mientras se mueve el slider.
                         //Espera antes de aplicar el recalculo
    public float debounceDelay = 0.15f; //Tiempo de espera en segundos antes de aplicar los cambios
    public float iluminacionDebounceDelay = 0.05f;

    //Variables privadas para manejar el debounce
    private debouncedValue<float> iluminacionDebounced;
    private debouncedValue<int> densityDebounce;
    private debouncedValue<int> resolutionDebounce;

    void Start()
    {
        resolutionDebounce = new debouncedValue<int>(debounceDelay, value => imageLoader.RecalculateGrid((int)value));
        densityDebounce = new debouncedValue<int>(debounceDelay, value => imageLoader.OnDensityLevelsChanged(value));
        iluminacionDebounced = new debouncedValue<float>(iluminacionDebounceDelay, value => imageLoader.RecalculateAscii(value));

        iluminacionSlider.onValueChanged.AddListener(OnIluminacionChanged);
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

    private void Update()
    {
        resolutionDebounce.Tick(Time.deltaTime);
        densityDebounce.Tick(Time.deltaTime);
        iluminacionDebounced.Tick(Time.deltaTime);

    }

    void OnIluminacionChanged(float value)
    {
        iluminacionValueText.text = $"Iluminación: {value:F2}";
        iluminacionDebounced.SetValue(value);
    }

    void OnResolutionChanged(float value)
    {
        resolutionValueText.text = $"Resolución: {(int)value} px";
        resolutionDebounce.SetValue((int)value);
    }

    void onDensityChanged(float value)
    {
        int levels = Mathf.RoundToInt(value);
        densityValueText.text = $"Escala de Gris: {levels}";
        densityDebounce.SetValue(levels);
    }

    void OnASCIIChanged(int index)
    {
        string newTable = charsets[index].densityTable;
        imageLoader.OnCharsetChanged(newTable);

        //Ajusta el rango del slider de niveles según el charset elegido
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

    /*
        void OnThresholdChanged(float value)
        {
            thresholdValueText.text = $"Umbral: {value:F2}";
            imageLoader.RecalculateAlphaThreshold(value);
        }
    */
}
