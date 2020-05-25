using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Hosting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayTestMap()
    {
        Loader.Load(Loader.Scene.TestScene);
    }

    public void PlayLvl1()
    {
        Loader.Load(Loader.Scene.LevelScene);
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }
}
