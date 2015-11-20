using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {
    static bool isPaused = false;

	// Use this for initialization
	void Start () {
	
	}
    public void ChangeScene(string Scenename)
    {
        Application.LoadLevel(Scenename);
    }
	
    public void Settings (bool Settings)
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
    }
}
