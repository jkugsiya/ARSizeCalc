using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Required component to raycast on the detected plane.
[RequireComponent(typeof(ARRaycastManager))]
public class SpawnLines : MonoBehaviour
{
    //The start line prefab
    public GameObject startLinePrefab;

    //gameobject to keep track of start line
    private GameObject startLine;

    //end line prefab
    public GameObject endLinePrefab;

    //gameobject to keep track of end line
    private GameObject endLine;

    //to track touch position
    private Vector2 touchPosition;
    
    //object to keep track of attached ARRayCastManager
    private ARRaycastManager raycastManager;

    //to keep track of all the hits by the raycast on the detected plane
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //the text element to show size.
    public TextMeshProUGUI sizeText;

    //This method calls at the scene initialization
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    //Method to get the position of touch by the raycast
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }


    //This method calls every frame
    void Update()
    {
        //If no touch occurance then do nothing
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        //If there is a touch event then cast a ray and calculate the position to instantiate the gameobject.
        if(raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if(startLine == null)
            {
                startLine = Instantiate(startLinePrefab, hitPose.position, Quaternion.Euler(0, 0, 0));
                endLine = Instantiate(endLinePrefab, hitPose.position + new Vector3(0,0,0.2f), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                startLine.transform.position = new Vector3(startLine.transform.position.x,
                    startLine.transform.position.y
                    ,hitPose.position.z);
                sizeText.text = "Size: " + (Math.Abs(startLine.transform.position.z - endLine.transform.position.z ) * 100);
            }
        }
        
    }
}
