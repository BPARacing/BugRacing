using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

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
}
