using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class StartMenu : MonoBehaviour
{

    //Start Game? Move to Options? or Exit
    public void LoadObject(int id)
    {
        switch (id)
        {
            case 0: //Start Game
                SceneManager.LoadScene(id);
                break;
            case 1: //options menu??
                SceneManager.LoadScene(id);
                break;
            case 2: //Exit
                Application.Quit();
                break;
        }
       


    }









}
