using UnityEngine;
using TMPro;

public class fps_Counter : MonoBehaviour
{

    public TextMeshProUGUI fpsText; //Referencia al componente TextMeshProUGUI donde se mostrará el FPS
    private float deltaTime; //Variable para almacenar el tiempo entre frames

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f; //Calculamos un promedio suavizado del tiempo entre frames
        float fps = 1.0f / deltaTime; //Calculamos los FPS como el inverso del tiempo entre frames
        fpsText.text = $"{fps:F0} FPS"; //Actualizamos el texto del componente TextMeshProUGUI con los FPS redondeados a cero decimales
    }
}
