using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConclusionDialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private RectTransform dialoguePanel;
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private CharacterSpeakers[] charactersInScene;
    INPUTS dialogueControls;
    private int dialogueIndex;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < charactersInScene.Length; i++)
        {
            charactersInScene[i].SetSprites();
        }
        dialogueManager.StartDialogue();
        /*dialogueControls = new INPUTS();
        dialogueControls.Enable();
        dialogueControls.DialogueControls.NextLine.performed += AdvanceDialogue;*/
    }

    public void AdvanceDialogue()
    {
        bool isDialogueOver;
        dialogueManager.NextScene(out isDialogueOver);
        if (isDialogueOver)
        {
            dialogueManager.EndDialogue();
            DialogueEnd();
        }
    }
    private void AdvanceDialogue(InputAction.CallbackContext ctx)
    {
        bool isDialogueOver;
        dialogueManager.NextScene(out isDialogueOver);
        if (isDialogueOver)
        {
            dialogueManager.EndDialogue();
            dialogueControls.DialogueControls.NextLine.performed -= AdvanceDialogue;
        }
    }

    private void DialogueEnd()
    {
        SetDialoguePanel(false);
        if (dialogueIndex == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        dialogueIndex++;
    }

    private void SetDialoguePanel(bool isTurnOn)
    {
        dialoguePanel.Find("Name").gameObject.SetActive(isTurnOn);
        dialoguePanel.Find("Dialogue").gameObject.SetActive(isTurnOn);
        dialoguePanel.Find("Continue Button").gameObject.SetActive(isTurnOn);
    }
}
