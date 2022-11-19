using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject optionsMenu = null;

    private bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {      
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pauseMenu.SetActive(isPaused);
            optionsMenu.SetActive(false);
        } 
    }

    public bool IsPaused() { return isPaused; }

    public void Play()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Play();
    }
}
