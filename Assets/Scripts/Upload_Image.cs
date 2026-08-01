using UnityEngine;
using UnityEngine.UI;
using SFB;

public class Upload_Image : MonoBehaviour
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

            float ratio = (float)texture.width / texture.height;
            aspectFitter.aspectRatio = ratio;

            //Prueba rápida: grilla de 40x25
            CellData[,] luminance = luminancia.Sample(texture, 40, 25);

            //Debug simple: imprime la grilla como números redondeados
            PrintLuminanceGrid(luminance, 40, 25);
        }
    }

    void PrintLuminanceGrid(CellData[,] grid, int columns, int rows)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                CellData cell = grid[col, row];
                sb.Append(cell.luminance.ToString("F1"))
                  .Append("/")
                  .Append(cell.alpha.ToString("F1"))
                  .Append(" ");
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
}