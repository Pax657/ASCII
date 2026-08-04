using UnityEngine;
using UnityEngine.UI;

public class colorPickerBackground : MonoBehaviour
{
    public Image image;

    public void onColorChange(Color newColor)
    {
        image.color = newColor;
    }
}
