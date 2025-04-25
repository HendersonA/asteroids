using UnityEngine;

namespace Asteroids
{
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
        private void UpdateCameraBounds()
        {
            _screenHeight = _mainCamera.orthographicSize;
            _screenWidth = _mainCamera.aspect * _screenHeight;
        }
        public void WrapAroundScreen()
        {
            Vector3 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);
            bool wrapped = false;

            if (viewportPos.x > 1)
            {
                viewportPos.x = 0;
                wrapped = true;
            }
            else if (viewportPos.x < 0)
            {
                viewportPos.x = 1;
                wrapped = true;
            }

            if (viewportPos.y > 1)
            {
                viewportPos.y = 0;
                wrapped = true;
            }
            else if (viewportPos.y < 0)
            {
                viewportPos.y = 1;
                wrapped = true;
            }

            if (wrapped)
            {
                Vector3 newPos = _mainCamera.ViewportToWorldPoint(viewportPos);
                newPos.z = transform.position.z;
                transform.position = newPos;
            }
        }
    }
}