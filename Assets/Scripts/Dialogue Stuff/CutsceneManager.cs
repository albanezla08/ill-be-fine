using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class AudioTrigger
{
    public int frameTrigger;
    public string soundTitle;
}

public class CutsceneManager : MonoBehaviour
{
    
    [SerializeField] private Image displayedFrame;
    [SerializeField] private Sprite[] cutsceneFrames;
    [SerializeField] private string[] transitionTypes;
    [SerializeField] private int[] framesToTransitionOn;
    [SerializeField] private AudioTrigger[] audioTriggers;
    /*INPUTS cutsceneControls;*/
    private int frameIndex = 0;
    private int transitionIndex = 0;
    private int audioIndex = 0;
    private int clickCount = 0;

    private AudioManager audioManagerScript;

    public void StartCutscene()
    {
        /*cutsceneControls = new INPUTS();
        cutsceneControls.DialogueControls.NextLine.performed += ctx => CheckForTransition();
        cutsceneControls.DialogueControls.Enable();*/
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        NextFrame();
    }

    private void NextFrame()
    {
        if (frameIndex >= cutsceneFrames.Length)
        {
            return;
        }
        displayedFrame.sprite = cutsceneFrames[frameIndex];
        StartCoroutine(FadeIn());
        frameIndex++;
    }

    //called by the current scene's manager
    public void CheckForTransition(out bool cutsceneEnd)
    {
        cutsceneEnd = transitionIndex >= framesToTransitionOn.Length;
        if (cutsceneEnd)
        {
            return;
        }
        if (clickCount == framesToTransitionOn[transitionIndex])
        {
            if (transitionTypes[frameIndex-1].Equals("Zoom Out"))
            {
                StartCoroutine(ZoomOut());
            }
            else
            {
                StartCoroutine(FadeOut());
            }
            transitionIndex++;
            cutsceneEnd = transitionIndex >= framesToTransitionOn.Length;
        }
        if (audioIndex < audioTriggers.Length && clickCount == audioTriggers[audioIndex].frameTrigger)
        {
            audioManagerScript.PlaySound(audioTriggers[audioIndex].soundTitle);
            audioIndex++;
        }
        clickCount++;
    }

    private IEnumerator FadeOut()
    {
        while (displayedFrame.color.a > 0)
        {
            Color tempColor = displayedFrame.color;
            tempColor.a -= 0.01f;
            displayedFrame.color = tempColor;
            yield return null;
        }
        NextFrame();
    }

    private IEnumerator ZoomOut()
    {
        while (displayedFrame.rectTransform.localScale.y > 0.4f)
        {
            Vector3 temp = displayedFrame.rectTransform.localScale;
            temp.x -= 0.002f;
            temp.y -= 0.002f;
            displayedFrame.rectTransform.localScale = temp;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        while (displayedFrame.color.a < 1)
        {
            Color tempColor = displayedFrame.color;
            tempColor.a += 0.01f;
            displayedFrame.color = tempColor;
            yield return null;
        }
    }
}
