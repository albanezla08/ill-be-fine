using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] CutsceneManager cutsceneManager;
    [SerializeField] int nextSceneIndex;
    INPUTS sceneInputs;

    private void Start()
    {
        Time.timeScale = 1;
        cutsceneManager.StartCutscene();
        dialogueManager.StartDialogue();
        sceneInputs = new INPUTS();
        sceneInputs.Enable();
        //sceneInputs.DialogueControls.NextLine.performed += AdvanceCutscene;
    }

    //pressed by button
    public void AdvanceCutscene()
    {
        bool cutsceneOver;
        bool dialogueOver;
        cutsceneManager.CheckForTransition(out cutsceneOver);
        dialogueManager.NextScene(out dialogueOver);
        if (cutsceneOver && SceneUtility.GetScenePathByBuildIndex(nextSceneIndex) != null)
        {
            StartCoroutine(FadeOutOfScene());
        }
    }
    private void AdvanceCutscene(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        bool cutsceneOver;
        bool dialogueOver;
        cutsceneManager.CheckForTransition(out cutsceneOver);
        dialogueManager.NextScene(out dialogueOver);
        if (cutsceneOver && SceneUtility.GetScenePathByBuildIndex(nextSceneIndex) != null)
        {
            StartCoroutine(FadeOutOfScene());
        }
    }

    private IEnumerator FadeOutOfScene()
    {
        dialogueManager.ClosePanel();
        sceneInputs.DialogueControls.NextLine.performed -= AdvanceCutscene;
        sceneInputs.Disable();
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);

    }

}
