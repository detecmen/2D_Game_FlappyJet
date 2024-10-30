using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    public GameObject astronault;
    public GameObject astronaultDog;

    public GameObject dog_Thought;
    public TMP_Text dog_Thought_text;

    
    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown,
        Play
    }

    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }

    void Awake()
    {

        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;

    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;

    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.Play);
        OnGameStarted();
        score = 0;
        gameOver = false;

    }
    void OnPlayerDied()
    {
        AudioManager.Instance.OnGameFinished();
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("highscore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("highscore", score);

        }
        SetPageState(PageState.GameOver);
    }
    void OnPlayerScored()
    {

        score += 1;
        scoreText.text = score.ToString();
        if (score % 10 == 0)
        {
            {
                Dog_speech();
            }
        }


    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:

                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                astronault.SetActive(true);
                astronaultDog.SetActive(false);
                dog_Thought.SetActive(false);
                break;
            case PageState.Start:

                astronault.SetActive(true);
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                astronaultDog.SetActive(false);
                dog_Thought.SetActive(false);

                break;
            case PageState.GameOver:

                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                astronault.SetActive(false);
                astronaultDog.SetActive(false);
                dog_Thought.SetActive(false);
                break;
            case PageState.Countdown:

                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                astronault.SetActive(false);
                astronaultDog.SetActive(true);
                dog_Thought.SetActive(false);
                break;
            case PageState.Play:

                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                astronault.SetActive(false);
                astronaultDog.SetActive(true);
                dog_Thought.SetActive(false);
                break;

        }
    }

    public void ConfirmedGameOver()
    {
        //replay buton
        OnGameOverConfirmed();
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
    public void StartGame()
    {
        AudioManager.Instance.PlayClick();
        SetPageState(PageState.Countdown);
        AudioManager.Instance.OnGameStarted();
    }
    public void Dog_speech()
    {

        dog_Thought.SetActive(true);
        dog_Thought_text.text = "Amazing!";
        StartCoroutine(Hide_Dog_Thought());

    }

    IEnumerator Hide_Dog_Thought()
    {
        yield return new WaitForSeconds(2f);
        dog_Thought.SetActive(false);
    }
    public int Score
    {
        get { return score; }
    }
    

}
