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

    [Header("Debounce")] //Sirve para evitar que se recalculen los valores de la imagen cada vez que se mueve el slider, sino que espera un tiempo para hacerlo
    public float debounceDelay = 0.15f; //Tiempo de espera en segundos antes de aplicar los cambios
    private bool hasPendingResolutionChange;
    private float pendingResolutionValue; //Valor pendiente de resolución para aplicar después del retraso
    private float resolutionDebounceTimer;

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
    }

    void OnIntensityChanged(float value)
    {
        intensityValueText.text = $"Iluminación: {value:F2}";
        imageLoader.RecalculateAscii(value);
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
