using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanel : MonoBehaviour
{
    public GameObject buildingPanel;
    public GameObject unitPanel;

    void Start()
    {
        //buildingPanel = GameObject.Find("UI").transform.Find("Panel").transform.Find("BuildingsBackgroundPanel").gameObject;
        //unitPanel = GameObject.Find("UI").transform.Find("Panel").transform.Find("UnitsBackgroundPanel").gameObject;
    }

    public void switchPanel() {
        if (buildingPanel.activeInHierarchy)
        {
            buildingPanel.SetActive(false);
            unitPanel.SetActive(true);
        }
        else
        {
            unitPanel.SetActive(false);
            buildingPanel.SetActive(true);
        }
    }
}
