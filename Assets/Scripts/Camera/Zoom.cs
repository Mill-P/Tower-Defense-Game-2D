using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    
    private float zoomSize = 55.0f;

    float zoomSpeed = 5.0f;

    private float originalSize = 0f;

    private Camera thisCamera;

    public int ZoomMin = 30;
    public int ZoomMax = 100;

    // Use this for initialization
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        originalSize = thisCamera.orthographicSize;
    }
    /// <summary>
    /// scrolling with mouse wheel in and out
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoomSize > ZoomMin)
        {
            zoomSize -= 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoomSize < ZoomMax)
        {
            zoomSize += 1;
        }
        if (zoomSize != thisCamera.orthographicSize)
        {
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize,
            zoomSize, Time.deltaTime * zoomSpeed);
        }
    }
}
