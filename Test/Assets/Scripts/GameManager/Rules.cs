using System;
using UnityEngine;

public class Rules : MonoBehaviour
{
    [SerializeField]
    int countEnemiesToDestroy = 1;
    int CurrentCountEnemiesDestroed;

    [SerializeField]
    int countEnemiesInBaseForLoss;
    int currentCountEnmiesInBase;

    Action callBackForWin, callBackForLoss;
    

    public void initeRules(Action win, Action loss) 
    {
        CurrentCountEnemiesDestroed = 0;
        currentCountEnmiesInBase = 0;
        callBackForWin = win;
        callBackForLoss = loss;
    }

    public void aEnemyDeath() 
    {
        CurrentCountEnemiesDestroed++;
        if (CurrentCountEnemiesDestroed >= countEnemiesToDestroy) 
        {
            callBackForWin.Invoke();
        }
    }

    public void aEnemyTouchBase() 
    {
        currentCountEnmiesInBase++;
        if (currentCountEnmiesInBase >= countEnemiesInBaseForLoss) 
        {
            callBackForLoss.Invoke();
        }
    }
}
