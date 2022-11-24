using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameFinishControl : MonoBehaviour
{
    [SerializeField] private GameObject text_victory = null;
    [SerializeField] private GameObject text_loser = null;

    [SerializeField] private GameObject button_victory = null;
    [SerializeField] private GameObject button_loser = null;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void Winner()
    {
        text_loser.SetActive(false);
        text_victory.SetActive(true);

        button_loser.SetActive(false);
        button_victory.SetActive(true);
    }

    public void Loser()
    {
        text_loser.SetActive(true);
        text_victory.SetActive(false);

        button_loser.SetActive(true);
        button_victory.SetActive(false);
    }

    public void desPause()
    {
        Time.timeScale = 1;
    }

    public void Retry()
    {
        Replay();
    }

    public void Next()
    {
        int size = SceneManager.sceneCountInBuildSettings;
        int current_index = SceneManager.GetActiveScene().buildIndex;
        if (current_index >= size) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(current_index+1);
        desPause();
    }

    public void Replay()
    {
        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
        desPause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        desPause();
    }
}
