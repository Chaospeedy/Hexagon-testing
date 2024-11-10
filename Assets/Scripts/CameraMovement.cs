using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float minCameraZoomLevel = 1f;
    [SerializeField] private float maxCameraZoomLevel = 10f;

    private float currentZoomLevel = 1f;

    private PlacementSystem placementSystem;

    private void Start(){
        GameObject placementGO = GameObject.Find("PlacementSystem");

        placementSystem = placementGO.GetComponent<PlacementSystem>();
    }

    private void Update()
    {
        Vector3 inputDirection = new Vector3(0,0,0);
        int edgeScrollSize = 30;
        
        if(Input.mousePosition.x < edgeScrollSize && transform.position.x < 5){//left side
            inputDirection.x = +1f;
        }
        if(Input.mousePosition.y < edgeScrollSize && transform.position.z < 7){//bottom side
            inputDirection.z = +1f;
        }
        if(Input.mousePosition.x > Screen.width - edgeScrollSize && transform.position.x > -5){//right side
            inputDirection.x = -1f;
        }
        if(Input.mousePosition.y > Screen.height - edgeScrollSize && transform.position.z > 1.5){//top side
            inputDirection.z = -1f;
        }

        //Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0 && currentZoomLevel > minCameraZoomLevel && !placementSystem.placementIsValid){
            inputDirection.y = -15f;
            currentZoomLevel--;
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 && currentZoomLevel < maxCameraZoomLevel && !placementSystem.placementIsValid){
            inputDirection.y = +15f;
            currentZoomLevel++;
        }

        float cameraSpeed = 5f;
        transform.position += inputDirection * cameraSpeed * Time.deltaTime;

        
    }
}
