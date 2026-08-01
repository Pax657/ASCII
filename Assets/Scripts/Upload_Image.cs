using UnityEngine;
using UnityEngine.UI;
using SFB;

public class ImageLoader : MonoBehaviour
{
    public RawImage image;
    public AspectRatioFitter aspectFitter;

    public void OpenExplorer()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        var paths = StandaloneFileBrowser.OpenFilePanel("Seleccionar imagen", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            LoadImage(paths[0]);
        }
    }

    void LoadImage(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(fileData))
        {
            image.texture = texture;

            // Calcula la proporción real de la imagen y se la pasa al fitter
            float ratio = (float)texture.width / texture.height;
            aspectFitter.aspectRatio = ratio;
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen: " + path);
        }
    }
}