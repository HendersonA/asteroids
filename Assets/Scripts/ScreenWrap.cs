using UnityEngine;

public class ScreenWrap: MonoBehaviour
{
    public float _screenWidth { get; private set; }
    public float _screenHeight { get; private set; }
    private float _objectRadius;
    private Camera _mainCamera;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }
    private void Start()
    {
        _mainCamera = Camera.main;
        _screenHeight = _mainCamera.orthographicSize;
        _screenWidth = _screenHeight * _mainCamera.aspect;
        if (_renderer != null)
            _objectRadius = Mathf.Max(_renderer.bounds.extents.x, _renderer.bounds.extents.y);
        else
            _objectRadius = 0.5f;
    }
    public void ChangePosition()
    {
        Vector3 position = transform.position;
        position.x = Wrap(position.x, -_screenWidth - _objectRadius, _screenWidth + _objectRadius);
        position.y = Wrap(position.y, -_screenHeight - _objectRadius, _screenHeight + _objectRadius);
        transform.position = position;
    }
    private float Wrap(float value, float min, float max)
    {
        if (value > max)
            value = min;
        else if (value < min)
            value = max;
        return value;
    }
}