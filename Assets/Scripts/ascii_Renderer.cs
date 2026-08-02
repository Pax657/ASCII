using UnityEngine;
using TMPro;

public class AsciiRenderer : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    //Queremos pasar deñ char[,] a string para mostrarlo en el cuadro de texto. Esto es un método auxiliar para convertir la grilla ASCII a un string.
    public void Render(char[,] asciiGrid, int columns, int rows)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                sb.Append(asciiGrid[col, row]);
            }
            sb.Append('\n'); //Nueva línea al final de cada fila
        }

        displayText.text = sb.ToString();  //mostramos el string en el cuadro de texto
    }
}