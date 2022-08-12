using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassMapSize : MonoBehaviour
{
   
    public int mapSize;
    public Button smallMapButton;
    public Button mediumMapButton;
    public Button bigMapButton;

    public void setMapSize(int size) {
        this.mapSize = size;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
