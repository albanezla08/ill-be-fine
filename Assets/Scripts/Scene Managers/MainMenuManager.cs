using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditsPanel;
    [SerializeField] private int startSceneIndex;
    private AudioManager audioManagerScript;

    private void Start()
    {
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        audioManagerScript.PlaySound("Keep Looking Up");
    }

    public void PlayButtonPressed()
    {
        audioManagerScript.StopSound("Keep Looking Up");
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneIndex);
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }
}
