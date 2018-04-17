using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseUI;
    public GameObject StartUI;
    private bool Paused = false;

    // Use this for initialization
    void Start() {

        PauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        //if press pause, pause?unpause the game

        if (Input.GetButtonDown("Pause"))
        {
            Paused = !Paused;
        }
        //if paused, set pause menu active and turn time to 0
        if (Paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        //if not pauset, set the pause menu to false and unpause the time (set back to one)
        if (!Paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }


    public void Resume ()
    {
        Paused = false;
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

  

    public void Exit ()

    {
        Application.Quit();
    }

    public void ControlsUI()

    {
        Application.LoadLevel(1);
    }

    public void StartGame()

    {
        StartUI.SetActive(false);
    }

    public void MainMenu()

    {
        StartUI.SetActive(true);

        PauseUI.SetActive(false);
      
    }
}



