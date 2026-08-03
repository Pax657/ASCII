using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    //Resolución
    private bool hasPendingResolutionChange;
    private float pendingResolutionValue; //Valor pendiente de resolución para aplicar después del retraso
    private float resolutionDebounceTimer;
    //Densidad
    private bool hasPendingDensityChange;
    private float pendingDensityValue;
    private float densityDebounceTimer;
    //Intensidad
    public float intensityDebounceDelay = 0.05f;
    private bool hasPendingIntensityChange;
    private float pendingIntensityValue;
    private float intensityDebounceTimer;

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

    private void Update()
    {
        if (hasPendingResolutionChange) //Si tiene un cambio pendiente, espera antes de aplicarlo
        {
            resolutionDebounceTimer -= Time.deltaTime;
            if(resolutionDebounceTimer <= 0) //Cuando se cumple el tiempo de espera, se aplica el cambio de resolución
            {
                imageLoader.RecalculateGrid((int)pendingResolutionValue); //Aplica el cambio pendiente de resolución
                hasPendingResolutionChange = false;
            }
        }

        if (hasPendingDensityChange)
        {
            densityDebounceTimer -= Time.deltaTime;
            if (densityDebounceTimer <= 0f)
            {
                imageLoader.OnDensityLevelsChanged((int)pendingDensityValue);
                hasPendingDensityChange = false;
            }
        }

        if (hasPendingIntensityChange)
        {
            intensityDebounceTimer -= Time.deltaTime;
            if (intensityDebounceTimer <= 0f)
            {
                imageLoader.RecalculateAscii(pendingIntensityValue);
                hasPendingIntensityChange = false;
            }
        }
    }

    void OnIntensityChanged(float value)
    {
        intensityValueText.text = $"Iluminación: {value:F2}";

        pendingIntensityValue = value;
        hasPendingIntensityChange = true;
        intensityDebounceTimer = intensityDebounceDelay;
    }

    void OnResolutionChanged(float value)
    {
        resolutionValueText.text = $"Resolución: {(int)value} px";

        pendingResolutionValue = value; //Almacena el valor pendiente de resolución
        hasPendingResolutionChange = true;
        resolutionDebounceTimer = debounceDelay; //Reinicia el temporizador de debounce
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

        pendingDensityValue = levels;
        hasPendingDensityChange = true;
        densityDebounceTimer = debounceDelay;
    }

    /*
        void OnThresholdChanged(float value)
        {
            thresholdValueText.text = $"Umbral: {value:F2}";
            imageLoader.RecalculateAlphaThreshold(value);
        }
    */
}
