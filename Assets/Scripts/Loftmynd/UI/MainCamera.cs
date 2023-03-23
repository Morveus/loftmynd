using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loftmynd.UI
{
    public class MainCamera : MonoBehaviour
    {

        public float moveSpeed = 1f;
        public float pinchSpeed = 0.01f;
        public float minZoom = 2f;
        public float maxZoom = 40f;

        public float mouseMoveSpeedMultiplier = 20;

        // To compensate for the mouse much slower move speed
        private float effectiveMoveSpeed;
        private float effectivePinchSpeed;

        private Camera mainCamera;
        private Vector2 touchPrevPos;

        void Start()
        {
            mainCamera = GetComponent<Camera>();
        }

        void Update()
        {
            effectiveMoveSpeed = moveSpeed;
            effectivePinchSpeed = pinchSpeed;
            if (Input.touchSupported && Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            else
            {
                effectiveMoveSpeed = moveSpeed * mouseMoveSpeedMultiplier;
                effectivePinchSpeed = pinchSpeed * 20;
                HandleMouseInput();
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                MoveCamera(touch.deltaPosition);
            }
            else if (Input.touchCount == 2)
            {
                PinchZoom();
            }
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                MoveCamera(mouseDelta);
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                float newY = mainCamera.transform.position.y - Input.mouseScrollDelta.y * effectivePinchSpeed;
                newY = Mathf.Clamp(newY, minZoom, maxZoom);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, newY, mainCamera.transform.position.z);
            }
        }

        private void MoveCamera(Vector2 deltaPosition)
        {
            float zoomFactor = (mainCamera.transform.position.y - minZoom) / (maxZoom - minZoom);
            float scaledMoveSpeed = effectiveMoveSpeed * (1f + zoomFactor);

            mainCamera.transform.Translate(-deltaPosition.x * scaledMoveSpeed * Time.deltaTime, -deltaPosition.y * scaledMoveSpeed * Time.deltaTime, 0);
        }

        private void PinchZoom()
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float prevTouchDeltaMagnitude = (touch0.position - touch1.position - touch0.deltaPosition + touch1.deltaPosition).magnitude;
                float touchDeltaMagnitude = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMagnitude - touchDeltaMagnitude;

                float newY = mainCamera.transform.position.y + deltaMagnitudeDiff * effectivePinchSpeed;
                newY = Mathf.Clamp(newY, minZoom, maxZoom);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, newY, mainCamera.transform.position.z);
            }
        }
    }
}
