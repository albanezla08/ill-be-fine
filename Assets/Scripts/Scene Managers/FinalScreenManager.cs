using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    [SerializeField] private int menuSceneIndex;
    public void PlayAgainPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneIndex);
    }
}
