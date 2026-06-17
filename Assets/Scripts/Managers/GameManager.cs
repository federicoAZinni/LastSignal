using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }

    //Events
    public Action OnWin;
    public Action OnLose;

    [Header("WinConditions")]
    [SerializeField] int countOfMiniGameCompletedToWin;
    int correntMiniGamesCompleted;

    [Space(5)]
    [Header("UI Refs")]
    [SerializeField] GameObject UI_WIn;
    [SerializeField] GameObject UI_Lose;



    public void MiniGameCompleted()
    {
        correntMiniGamesCompleted++;
    }
    public void OnFinalTrigger()
    {
        if (correntMiniGamesCompleted >= countOfMiniGameCompletedToWin) Win();
    }
    public void PlayerDead()
    {
        Lose();
    }


    private void Win()
    {
        OnWin?.Invoke();
        UI_WIn.SetActive(true);
    }

    private void Lose()
    {
        OnLose?.Invoke();
        UI_Lose.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
