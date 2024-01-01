using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueScene
{
    [TextArea(3,10)] public string text;
    public CharacterSpeakers character;
    public CharacterSpeakers.Attitudes attitude;
    public string speakerPosition;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Transform dialoguePanel;
    TextMeshProUGUI speechText;
    TextMeshProUGUI nameTag;
    Image nameTagImage;
    Image speaker1Sprite;
    Image speaker2Sprite;
    [SerializeField] Color notSpeakingColor;
    [SerializeField] List<DialogueScene> sceneList = new List<DialogueScene>();
    Queue<DialogueScene> sceneQueue = new Queue<DialogueScene>();
    private AudioManager audioManagerScript;

    private void Awake()
    {
        speechText = dialoguePanel.Find("Dialogue").GetComponent<TextMeshProUGUI>();
        nameTag = dialoguePanel.Find("Name").GetComponent<TextMeshProUGUI>();
        nameTagImage = dialoguePanel.Find("Name Tag Image").GetComponent<Image>();
        speaker1Sprite = dialoguePanel.Find("Left Speaker").GetComponent<Image>();
        speaker2Sprite = dialoguePanel.Find("Right Speaker").GetComponent<Image>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    //called by Scene Manager
    public void StartDialogue()
    {
        sceneQueue.Clear();
        foreach (DialogueScene scene in sceneList)
        {
            sceneQueue.Enqueue(scene);
        }
        NextScene();
    }

    public void NextScene()
    {
        audioManagerScript.PlaySound("Continue Button");
        DialogueScene currentScene = sceneQueue.Dequeue();
        //where scene is set
        speechText.text = currentScene.text;
        nameTag.text = "";
        if (currentScene.character != null)
        {
            nameTagImage.sprite = currentScene.character.characterNameTag;
            nameTagImage.gameObject.SetActive(true);
            nameTag.gameObject.SetActive(false);
            /*if (currentScene.character.characterNameTag == null) //if no special nametag
            {
                nameTag.text = currentScene.character.name;
                nameTag.gameObject.SetActive(true);
                nameTagImage.gameObject.SetActive(false);
            }
            else
            {
                nameTagImage.sprite = currentScene.character.characterNameTag;
                nameTagImage.gameObject.SetActive(true);
                nameTag.gameObject.SetActive(false);
            }*/
            /*if (currentScene.speakerPosition == "Right")
            {
                speaker2Sprite.sprite = currentScene.character.GetSprite(currentScene.attitude.ToString());
            }
            else
            {
                speaker1Sprite.sprite = currentScene.character.GetSprite(currentScene.attitude.ToString());
            }*/
            if (currentScene.speakerPosition == "Right")
            {
                speaker2Sprite.color = new Color(1, 1, 1);
                speaker1Sprite.color = notSpeakingColor;
            }
            else
            {
                speaker1Sprite.color = new Color(1, 1, 1);
                speaker2Sprite.color = notSpeakingColor;
            }
        }
        else
        {
            nameTag.text = "Narrator";
        }
    }
    public void NextScene(out bool dialogueEnd)
    {
        dialogueEnd = sceneQueue.Count <= 0;
        if (dialogueEnd)
        {
            return;
        }
        NextScene();
    }

    public void EndDialogue()
    {
        Debug.Log("text done");
    }

    public void OpenPanel()
    {
        dialoguePanel.gameObject.SetActive(true);
        /*speechText.gameObject.SetActive(true);
        nameTag.gameObject.SetActive(true);
        speaker1Sprite.gameObject.SetActive(true);
        speaker2Sprite.gameObject.SetActive(true);*/
    }
    public void ClosePanel()
    {
        dialoguePanel.gameObject.SetActive(false);
    }

    public void ChangeSceneList(List<DialogueScene> newList)
    {
        sceneList = newList;
    }
}
