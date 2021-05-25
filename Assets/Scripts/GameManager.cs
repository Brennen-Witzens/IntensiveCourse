using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoSingleton<GameManager>
{

    public override void Init()
    {
        base.Init();

        IsGameOver = false;
        
    }


    public bool IsGameOver { get; private set; }
    public bool StartedGame { get; private set; }


    public void GameOver()
    {
        IsGameOver = true;
    }

    public void StartGame()
    {
        StartedGame = true;
        SpawnManager.Instance.StartSpawning();
    }


    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }


    }



    private void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }


}
