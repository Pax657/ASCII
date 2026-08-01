using UnityEngine;

public struct CellData //manejamos el alpha y la luminancia de cada celda por separado
{
    public float luminance;
    public float alpha;
}

public static class luminancia
{
    public static CellData[,] Sample(Texture2D texture, int columns, int rows)
    {
        Color32[] pixels = texture.GetPixels32(); //es más rápido que GetPixels() y nos da acceso a los valores de color en bytes
        int texWidth = texture.width;
        int texHeight = texture.height;

        CellData[,] grid = new CellData[columns, rows];

        float cellWidth = (float)texWidth / columns;
        float cellHeight = (float)texHeight / rows;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int startX = Mathf.FloorToInt(col * cellWidth); //coordenadas de la celda
                int endX = Mathf.FloorToInt((col + 1) * cellWidth);
                int startY = Mathf.FloorToInt(row * cellHeight);
                int endY = Mathf.FloorToInt((row + 1) * cellHeight);

                float sumLum = 0f;
                float sumAlpha = 0f;
                int count = 0;

                for (int y = startY; y < endY && y < texHeight; y++)
                {
                    for (int x = startX; x < endX && x < texWidth; x++)
                    {
                        Color32 pixel = pixels[y * texWidth + x]; //accedemos al pixel en la posición (x, y)
                        sumLum += GetLuminance(pixel); //calculamos la luminancia del pixel
                        sumAlpha += pixel.a / 255f; //normalizamos el valor alpha a [0, 1]
                        count++; //contamos los pixels procesados
                    }
                }

                grid[col, row] = new CellData
                {
                    luminance = count > 0 ? sumLum / count : 0f, //promediamos la luminancia y el alpha de la celda
                    alpha = count > 0 ? sumAlpha / count : 0f 
                };
            }
        }

        return grid;
    }

    private static float GetLuminance(Color32 pixel) //El ojo humano percive ciertos colores más vibrantes (el verde sobretodo), por eso se usa esta fórmula para calcular la luminancia percibida
    {
        float r = pixel.r / 255f;
        float g = pixel.g / 255f;
        float b = pixel.b / 255f;

        return 0.299f * r + 0.587f * g + 0.114f * b;
    }
}