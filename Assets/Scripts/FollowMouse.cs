using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public RectTransform imageRect;
    public Canvas canvas;

    void Awake()
    {
        if (imageRect == null)
        {
            imageRect = GetComponent<RectTransform>();
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // Make the top-left corner follow the mouse
        imageRect.pivot = new Vector2(0f, 1f);
    }

    void Update()
    {
        if (Mouse.current == null || imageRect == null || canvas == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        imageRect.anchoredPosition = localPoint;
    }
}