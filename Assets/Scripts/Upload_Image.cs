using UnityEngine;
using UnityEngine.UI;
using SFB;

public class ImageLoader : MonoBehaviour
{
    public RawImage image;

    public void OpenExplorer()
    {
        //Filtro de extensiones válidas
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        //Abre el diálogo nativo del sistema operativo
        var paths = StandaloneFileBrowser.OpenFilePanel("Seleccionar imagen", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            LoadImage(paths[0]);
        }
    }

    void LoadImage(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2); //el tamaño se ajusta solo al cargar
        if (texture.LoadImage(fileData))
        {
            image.texture = texture;
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen: " + path);
        }
    }
}