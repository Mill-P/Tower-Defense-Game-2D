using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Events : MonoBehaviour
{
    public void SizeSelect()
    {
        SceneManager.LoadScene("Map_Size_Menu");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene_New");
    }
}
