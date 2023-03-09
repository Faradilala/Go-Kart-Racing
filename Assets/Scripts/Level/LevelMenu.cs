using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void Easy()
    {
        SceneManager.LoadScene("SampleScane");
    }

    public void Medium()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Hard()
    {
        SceneManager.LoadScene("Level3");
    }

    public void KembaliMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
