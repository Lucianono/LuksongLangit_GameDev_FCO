using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private float startTime;

    void Start()
    {
        
        startTime = Time.time;

    }

    void Update()
    {
        
        gameOverText.text = timerText.text;
    }

    public void restartButton()
    {
        SceneManager.LoadScene(1);

    }

    public void quitButton()
    {
        Application.Quit();
    }





}
