using UnityEngine;
using TMPro;

public class ascii_Renderer : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    //Queremos pasar de char[,] a string para mostrarlo en el cuadro de texto. Esto es un método auxiliar para convertir la grilla ASCII a un string.
    public void Render(char[,] asciiGrid, int columns, int rows)
    {
        //stringbuilder sirve para construir strings de manera eficiente, especialmente cuando se concatenan muchas partes
        //en este caso, vamos a construir el string que representa la grilla ASCII
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                sb.Append(asciiGrid[col, row]); //Agregamos el carácter correspondiente a la posición (col, row) al StringBuilder
            }
            sb.Append('\n'); //Nueva línea al final de cada fila
        }

        displayText.text = sb.ToString();  //mostramos el string en el cuadro de texto
    }
}