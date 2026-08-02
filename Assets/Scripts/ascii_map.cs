using UnityEngine;

public static class ascii_map
{
    //Tabla de densidad: el primer carácter es "más denso" (oscuro), el último "menos denso" (claro)
    private const string DensityTable = "@%#*+=-:. ";

    public static char[,] MapToAscii(CellData[,] grid, int columns, int rows, float alphaThreshold = 0.15f)
    {
        char[,] result = new char[columns, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                CellData cell = grid[col, row];

                //Si la celda es mayormente transparente, no dibujamos ningún carácter
                if (cell.alpha < alphaThreshold)
                {
                    result[col, row] = ' ';
                    continue;
                }

                result[col, row] = GetCharacterForLuminance(cell.luminance);
            }
        }

        return result;
    }

    private static char GetCharacterForLuminance(float luminance)
    {
        //luminance va de 0.0 (oscuro) a 1.0 (claro)
        //Invertimos el índice porque el primer carácter de la tabla es el más "denso" (oscuro)
        int index = Mathf.RoundToInt(luminance * (DensityTable.Length - 1));
        //index = DensityTable.Length - 1 - index; // invierte: luminancia baja -> carácter denso

        return DensityTable[index];
    }

    public static class GridSizeCalculator
    {
        // Proporción típica alto/ancho de un carácter monoespaciado (ajustable según la fuente que uses)
        private const float CharacterAspectRatio = 2.0f;

        public static (int columns, int rows) CalculateGridSize(Texture2D texture, int desiredColumns)
        {
            float imageAspect = (float)texture.width / texture.height;

            int columns = desiredColumns;
            int rows = Mathf.RoundToInt(columns / imageAspect / CharacterAspectRatio);

            return (columns, rows);
        }
    }
}