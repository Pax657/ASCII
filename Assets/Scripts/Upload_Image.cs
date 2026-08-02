using SFB;
using UnityEngine;
using UnityEngine.UI;
using static ascii_map;

public class Upload_Image : MonoBehaviour
{
    public RawImage image;
    public AspectRatioFitter aspectFitter;
    public ascii_Renderer asciiRenderer;

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

            var (columns, rows) = GridSizeCalculator.CalculateGridSize(texture, 80);

            //Prueba rápida: grilla de 120x90
            CellData[,] grid = luminancia.Sample(texture, columns, rows);

            //Debug simple: imprime la grilla como números redondeados
            PrintLuminanceGrid(grid, columns, rows);

            //Mapeamos la grilla a caracteres ASCII
            char[,] asciiGrid = ascii_map.MapToAscii(grid, columns, rows);

            //Debug simple: imprime la grilla ASCII
            PrintAsciiGrid(asciiGrid, columns, rows);

            asciiRenderer.Render(asciiGrid, columns, rows);
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

    void PrintAsciiGrid(char[,] grid, int columns, int rows)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                sb.Append(grid[col, row]);
            }
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
}