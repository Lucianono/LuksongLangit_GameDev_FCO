using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public Text gameOverText;
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameOverText.text = "Your time is  " + elapsedTime.ToString("F2") + " minutes";
    }

    public void restartButton()
    {
        SceneManager.LoadScene(0);

    }

    public void quitButton()
    {
        Application.Quit();
    }





}
