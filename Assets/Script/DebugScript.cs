using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugScript : MonoBehaviour {
    public GameObject debugCar;
    public GameObject PauseMenu;
    List<GameObject> varholders;

    static bool isPaused = false;

    public void Start()
    {
        // initialize varholders
        varholders = new List<GameObject>();

        // get varholders in the pause debug menu
        foreach(RectTransform child in PauseMenu.GetComponentsInChildren<RectTransform>())
        {
            // ensure the 'Image' button is not selected
            if (child.name!="Image")
            {
                varholders.Add(child.gameObject);
            }
        }
    }

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
            PauseMenu.transform.localPosition = new Vector3(0, 5, 10);

            // get all variables
        }
    }

    // get all the data from the variables
    private void GetData()
    {
        foreach (GameObject holder in varholders)
        {
            debugCar.SendMessage("getVar", holder.name);
        }
    }

    private void SaveData()
    {

    }
}
