using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public Action GameWinAction;
    public Action GameLoseAction;

    private bool gameLost = false;
    private bool gameWon = false;


    private void Awake() 
    {

        instance = this;
    }

    

    //Game win durumu.
    public void GameWin()
    {
        if(gameLost || gameWon) return;
        gameWon = true;
        GameplayUIManager.instance.OpenGameEndPanel(1);
        GameWinAction?.Invoke();
    }

    //Game lose durumu.
    public void GameLose()
    {
        if(gameLost || gameWon) return;
        gameLost = true;
        GameplayUIManager.instance.OpenGameEndPanel(1.5f);
        GameLoseAction?.Invoke();  
    }

}
