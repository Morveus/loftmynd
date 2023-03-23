using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loftmynd.UI
{
    public class Clickable : MonoBehaviour
    {
        public GameObject clickTarget;
        private Vector3 lastCameraPosition;
        public float moveThreshold = 0.1f;

        private void Start()
        {
            lastCameraPosition = Camera.main.transform.position;
        }

        private void HandleClick()
        {
            if (clickTarget == null)
                return;

            float cameraMoveDistance = Vector3.Distance(lastCameraPosition, Camera.main.transform.position);

            if (cameraMoveDistance > moveThreshold)
                return;

            // If it's a light
            if (clickTarget.GetComponent<Light>() != null)
            {
                clickTarget.SetActive(!clickTarget.activeSelf);
                return;
            }

            // Last option
            clickTarget.SetActive(!clickTarget.activeSelf);
        }

        private void OnMouseUp()
        {
            HandleClick();
        }

        private void Update()
        {
            // Touch input for mobile devices
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.Equals(gameObject))
                        {
                            HandleClick();
                        }
                    }
                }
            }

            // Update the last camera position only when the camera is not moving
            if (Camera.main.transform.position != lastCameraPosition && !Input.GetMouseButton(0))
            {
                lastCameraPosition = Camera.main.transform.position;
            }
        }
    }
}