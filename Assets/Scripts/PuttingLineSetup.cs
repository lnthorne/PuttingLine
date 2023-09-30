using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PuttingLineSetup : MonoBehaviour
{
    public GameObject markerPrefab; // A prefab for visualizing the start and end points
    public ARRaycastManager arRaycastManager;

    public Vector3? startPoint = null;
    public Vector3? endPoint = null;

    private GameObject startPointMarker = null;
    private GameObject endPointMarker = null;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Debug.Log("We have hit something");

                    // Set either the start or end point, and instantiate visual markers
                    if (startPoint == null)
                    {
                        startPoint = hitPose.position;
                        startPointMarker = Instantiate(markerPrefab, startPoint.Value, Quaternion.identity);
                    }
                    else if (endPoint == null)
                    {
                        endPoint = hitPose.position;
                        endPointMarker = Instantiate(markerPrefab, endPoint.Value, Quaternion.identity);
                    }
                }
            }
        }
    }

    // Additional methods to reset or adjust points can be added here
    public void ResetPoints()
    {
        if (startPointMarker != null)
            Destroy(startPointMarker);

        if (endPointMarker != null)
            Destroy(endPointMarker);

        startPoint = null;
        endPoint = null;
    }
}
