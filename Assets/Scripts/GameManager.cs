using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Transform PlayerTransform
    {
        get
        {
            if (!_playerTransform) _playerTransform = GameObject.FindWithTag("Player").transform;
            return _playerTransform;
        }
        set { _playerTransform = value; }
    }
    private static Transform _playerTransform; 
    public static Camera MainCamera
    {
        get
        {
            if (!_mainCamera) _mainCamera = Camera.main;
            return _mainCamera;
        }
        set { _mainCamera = value; }
    }
    private static Camera _mainCamera;
}
