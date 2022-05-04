using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public Action GameWinAction;
    public Action GameLoseAction;


    private void Awake() 
    {
        instance = this;
    }

    //Game win durumu.
    public void GameWin()
    {
        GameplayUIManager.instance.OpenGameEndPanel(1);
        GameWinAction?.Invoke();
    }

    //Game lose durumu.
    public void GameLose()
    {
        GameplayUIManager.instance.OpenGameEndPanel(1.5f);
        GameLoseAction?.Invoke();  
    }

}
