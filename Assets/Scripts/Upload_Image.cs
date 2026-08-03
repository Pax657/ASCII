using SFB;
using UnityEngine;
using UnityEngine.UI;
using static ascii_map;

public class Upload_Image : MonoBehaviour
{
    public RawImage image;
    public AspectRatioFitter aspectFitter;
    public ascii_Renderer asciiRenderer;

    //valores privados necesarios para cambiar datos sin tener que volver a cargar la imagen
    private Texture2D currentTexture;
    private CellData[,] currentGrid;
    private int currentColumns;
    private int currentRows;
    private float currentIntensity = 1f;

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
        if(image.texture != null) // Liberar la textura anterior si existe
        {
            Destroy(image.texture);
        }

        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);

        if (texture.LoadImage(fileData))
        {
            image.texture = texture;
            currentTexture = texture;

            float ratio = (float)texture.width / texture.height;
            aspectFitter.aspectRatio = ratio;
            RecalculateGrid(80);

            var (columns, rows) = GridSizeCalculator.CalculateGridSize(texture, 80);

            currentGrid = luminancia.Sample(texture, columns, rows);
            currentColumns = columns;
            currentRows = rows;

            //Prueba rápida: grilla de 120x90
            //CellData[,] grid = luminancia.Sample(texture, columns, rows);

            //Debug simple: imprime la grilla como números redondeados
            //PrintLuminanceGrid(currentGrid, currentColumns, currentRows);

            //Mapeamos la grilla a caracteres ASCII
            char[,] asciiGrid = ascii_map.MapToAscii(currentGrid, currentColumns, currentRows, currentIntensity);

            //Debug simple: imprime la grilla ASCII
            //PrintAsciiGrid(asciiGrid, currentColumns, currentRows);

            asciiRenderer.Render(asciiGrid, currentColumns, currentRows);
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

    public void RecalculateAscii(float intensity)
    {
        currentIntensity = intensity;
        char[,] asciiGrid = ascii_map.MapToAscii(currentGrid, currentColumns, currentRows, currentIntensity, 0.15f);
        asciiRenderer.Render(asciiGrid, currentColumns, currentRows);
    }

    public void RecalculateGrid(int columns)
    {
        //Recalcular la grilla y el ASCII solo si hay una textura cargada
        if (currentTexture != null)
        {
            //Calcular el nuevo tamaño de la grilla basado en la nueva cantidad de columnas
            var (newColumns, newRows) = GridSizeCalculator.CalculateGridSize(currentTexture, columns);
            currentGrid = luminancia.Sample(currentTexture, newColumns, newRows);
            currentColumns = newColumns;
            currentRows = newRows;
            
            RecalculateAscii(currentIntensity); //Recalcular ASCII con la intensidad actual
        }
    } 
}