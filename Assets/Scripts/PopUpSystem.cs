using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public GameObject PopUpBoxBuild;
    public GameObject PopUpBoxEnd;
    public GameObject PopUpBoxGold;
    public GameObject PopUpBoxUnit;


    void Start()
    {
                
    }

    public void openPopUpBoxBuild()
    {
        PopUpBoxBuild.SetActive(true);        
    }
    public void closePopUpBoxBuild()
    {
        PopUpBoxBuild.SetActive(false);
    }

    public void openPopUpBoxGold()
    {
        PopUpBoxGold.SetActive(true);
    }
    public void closePopUpBoxGold()
    {
        PopUpBoxGold.SetActive(false);
    }

    public void openPopUpBoxEnd()
    {
        PopUpBoxEnd.SetActive(true);        
    }
    public void closePopUpBoxEnd()
    {
        PopUpBoxEnd.SetActive(false);
    }

    public void openPopUpBoxUnit()
    {
        PopUpBoxUnit.SetActive(true);
    }
    public void closePopUpBoxUnit()
    {
        PopUpBoxUnit.SetActive(false);
    }

    public void closeAll()
    {
        closePopUpBoxBuild();
        closePopUpBoxEnd();
        closePopUpBoxGold();
        closePopUpBoxUnit();
    }

}
