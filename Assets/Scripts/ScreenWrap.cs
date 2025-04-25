using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera _mainCamera;
    public float _screenHeight { get; private set; }
    public float _screenWidth { get; private set; }

    private void Start()
    {
        _mainCamera = GameManager.MainCamera;

        UpdateCameraBounds();
    }
    private void Update()
    {
        if (CameraSizeChanged())
            UpdateCameraBounds();

        WrapAroundScreen();
    }
    private bool CameraSizeChanged()
    {
        return _screenHeight != _mainCamera.orthographicSize;
    }

    private void UpdateCameraBounds()
    {
        _screenHeight = _mainCamera.orthographicSize;
        _screenWidth = _mainCamera.aspect * _screenHeight;
    }
    private void WrapAroundScreen()
    {
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x > _screenWidth)
        {
            pos.x = -_screenWidth;
            wrapped = true;
        }
        else if (pos.x < -_screenWidth)
        {
            pos.x = _screenWidth;
            wrapped = true;
        }

        if (pos.y > _screenHeight)
        {
            pos.y = -_screenHeight;
            wrapped = true;
        }
        else if (pos.y < -_screenHeight)
        {
            pos.y = _screenHeight;
            wrapped = true;
        }

        if (wrapped)
            transform.position = pos;
    }
}
