using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerGame : MonoBehaviour
{
    public void OpenNewScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}