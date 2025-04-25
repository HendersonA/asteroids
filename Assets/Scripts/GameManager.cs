using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids
{
    public class GameManager : Singleton<GameManager>
    {
        public static Action[] GameEvents;
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
        protected override void Awake()
        {
            base.Awake();
            SetEventsList();
            ObjectPool.Instance.Clear();
        }
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Application.Quit();
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }
        public static void SetEventsList()
        {
            GameEvents = new Action[Enum.GetValues(typeof(GameEvent)).Length];
        }
    }
    [System.Serializable]
    public enum GameEvent
    {
        GameStart,
        GameFail,
    }
}