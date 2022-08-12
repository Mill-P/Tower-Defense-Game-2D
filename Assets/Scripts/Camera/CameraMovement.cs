using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{

    public float dragSpeed = 0.5f;
    public float dragTreshold = 0.2f;

    private float cameraLimitLeft = 0;
    private float cameraLimitRight = 200;
    private float cameraLimitTop = 100;
    private float cameraLimitBottom = 0;

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    private MapManager mapManager;

    /// <summary>
    /// Camera movement using right click and dragging
    /// </summary>

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 cameraPosition = Camera.main.transform.position;
            if (!isDragging)
            {
                this.lastMousePosition = mousePosition;
                isDragging = true;
            }
            else
            {
                float xDifference = (this.lastMousePosition.x - mousePosition.x) * this.dragSpeed;
                float yDifference = (this.lastMousePosition.y - mousePosition.y) * this.dragSpeed;
                if ((Mathf.Abs(this.lastMousePosition.x - mousePosition.x) < this.dragTreshold
                    && Mathf.Abs(this.lastMousePosition.y - mousePosition.y) < this.dragTreshold)
                    || cameraPosition.x + xDifference < cameraLimitLeft
                    || cameraPosition.x + xDifference > cameraLimitRight
                    || cameraPosition.y + yDifference > cameraLimitTop
                    || cameraPosition.y + yDifference < cameraLimitBottom)
                {
                    this.lastMousePosition = mousePosition;
                    return;
                }
                Camera.main.transform.Translate(xDifference, yDifference, 0);
                this.lastMousePosition = mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

    }

    void Start()
    {
        this.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        Camera.main.transform.Translate(this.mapManager.mapSize / 2 * this.mapManager.tileSize, this.mapManager.mapSize / 2 * this.mapManager.tileSize, 0);

        this.mapManager.mapManagerReady += this.mapManagerReadyEvent;
    }

    private void mapManagerReadyEvent()
    {
        this.cameraLimitLeft = this.mapManager.getMapTileAtGridIndex(0, 0).position.x;
        this.cameraLimitRight = this.mapManager.getMapTileAtGridIndex(this.mapManager.mapSize - 1, this.mapManager.mapSize - 1).position.x;
        this.cameraLimitBottom = this.mapManager.getMapTileAtGridIndex(0, 0).position.y;
        this.cameraLimitTop = this.mapManager.getMapTileAtGridIndex(this.mapManager.mapSize - 1, this.mapManager.mapSize - 1).position.y;
    }

}
