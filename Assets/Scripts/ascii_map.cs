using UnityEngine;

public static class ascii_map
{
    //Tabla de densidad: el primer carácter es "más denso" (oscuro), el último "menos denso" (claro)
    //private const string DensityTable = "@%#*+=-:. ";

    public static char[,] MapToAscii(CellData[,] grid, int columns, int rows, float intensity, string densityTable, float alphaThreshold = 0.15f)
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


                float adjustedLuminance = ApplyIntensity(cell.luminance, intensity);
                result[col, row] = GetCharacterForLuminance(adjustedLuminance, densityTable);
            }
        }

        return result;
    }

    private static char GetCharacterForLuminance(float luminance, string densityTable)
    {
        //luminance va de 0.0 (oscuro) a 1.0 (claro)
        //Invertimos el índice porque el primer carácter de la tabla es el más "denso" (oscuro)
        int index = Mathf.RoundToInt(luminance * (densityTable.Length - 1));
        //index = densityTable.Length - 1 - index; //invierte: luminancia baja -> carácter denso

        return densityTable[index];
    }

    public static class GridSizeCalculator
    {
        //Proporción típica alto/ancho de un carácter monoespaciado (ajustable según la fuente que uses)
        private const float CharacterAspectRatio = 2.0f;

        public static (int columns, int rows) CalculateGridSize(Texture2D texture, int desiredColumns)
        {
            float imageAspect = (float)texture.width / texture.height;

            int columns = desiredColumns;
            int rows = Mathf.RoundToInt(columns / imageAspect / CharacterAspectRatio);

            return (columns, rows);
        }
    }

    public static float ApplyIntensity(float luminance, float intensity)
    {
        //intensity = 1.0 → sin cambio
        //intensity > 1.0 → más contraste (los oscuros se oscurecen más, los claros se aclaran más)
        //intensity < 1.0 → menos contraste (todo tiende al gris medio)
        return Mathf.Pow(luminance, intensity);
    }

    public static string ReduceDensityLevels(string fullTable, int levels)
    {
        if (levels >= fullTable.Length) return fullTable; //no hay nada que reducir
        if (levels <= 1) return fullTable[0].ToString(); //si hay un solo nivel, devolvemos el primer carácter (el más denso)

        char[] reduced = new char[levels];
        for (int i = 0; i < levels; i++)
        {
            //Distribuye los índices uniformemente a lo largo de la tabla original
            int sourceIndex = Mathf.RoundToInt(i * (fullTable.Length - 1) / (float)(levels - 1));
            reduced[i] = fullTable[sourceIndex];
        }

        return new string(reduced);
    }

}