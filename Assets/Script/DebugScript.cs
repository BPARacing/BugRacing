using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {
    public GameObject debugCar;
    public GameObject PauseMenu;

    static bool isPaused = false;

    public void PauseUnpause()
    {
        //if paused
        if (isPaused)
        {
            PauseMenu.transform.localPosition = new Vector3(0, -1000, 10);
            isPaused = false;
        }
        //if not pause
        else
        {
            isPaused = true;
            // bring up the menu
            // TODO: animate it
            PauseMenu.transform.localPosition = new Vector3(0, 0, 10);

            // get all variables
        }
    }
}
