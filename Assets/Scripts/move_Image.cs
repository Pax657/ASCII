using UnityEngine;
using UnityEngine.EventSystems;

public class ImagePanZoom : MonoBehaviour, IDragHandler, IScrollHandler
{
    public RectTransform imageRect; // el RectTransform del RawImage
    public bool panModeEnabled = false; // controlado por tu botón lateral

    public float zoomSpeed = 0.1f;
    public float minZoom = 1f;
    public float maxZoom = 4f;

    private float currentZoom = 1f;

    public void TogglePanMode()
    {
        panModeEnabled = !panModeEnabled;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!panModeEnabled) return;

        // Mueve la imagen según el arrastre del mouse
        imageRect.anchoredPosition += eventData.delta;

        ClampPosition();
    }

    public void OnScroll(PointerEventData eventData)
    {
        // El zoom puede funcionar siempre, sin necesidad del modo pan
        float scrollDelta = eventData.scrollDelta.y;
        currentZoom = Mathf.Clamp(currentZoom + scrollDelta * zoomSpeed, minZoom, maxZoom);

        imageRect.localScale = new Vector3(currentZoom, currentZoom, 1f);

        ClampPosition();
    }

    void ClampPosition()
    {
        // Evita que arrastres la imagen tan lejos que quede un hueco vacío en el viewport
        RectTransform viewport = imageRect.parent as RectTransform;

        float maxX = (imageRect.rect.width * currentZoom - viewport.rect.width) / 2f;
        float maxY = (imageRect.rect.height * currentZoom - viewport.rect.height) / 2f;

        maxX = Mathf.Max(maxX, 0);
        maxY = Mathf.Max(maxY, 0);

        Vector2 pos = imageRect.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
        imageRect.anchoredPosition = pos;
    }
}