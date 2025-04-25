using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera _mainCamera;
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteExtent;
    public float _screenHeight { get; private set; }
    public float _screenWidth { get; private set; }

    private void Start()
    {
        _mainCamera = GameManager.MainCamera;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteExtent = spriteRenderer.bounds.extents;
        UpdateCameraBounds();
    }
    private void UpdateCameraBounds()
    {
        _screenHeight = _mainCamera.orthographicSize;
        _screenWidth = _mainCamera.aspect * _screenHeight;
    }
    public void WrapAroundScreen()
    {
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x > _screenWidth + spriteExtent.x)
        {
            pos.x = -_screenWidth - spriteExtent.x;
            wrapped = true;
        }
        else if (pos.x < -_screenWidth - spriteExtent.x)
        {
            pos.x = _screenWidth + spriteExtent.x;
            wrapped = true;
        }

        if (pos.y > _screenHeight + spriteExtent.y)
        {
            pos.y = -_screenHeight - spriteExtent.y;
            wrapped = true;
        }
        else if (pos.y < -_screenHeight - spriteExtent.y)
        {
            pos.y = _screenHeight + spriteExtent.y;
            wrapped = true;
        }

        if (wrapped)
            transform.position = pos;
    }
}
