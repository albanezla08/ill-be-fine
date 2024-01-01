using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EpilogueManager : MonoBehaviour
{
    [SerializeField] private GridUtil gridScript;
    [SerializeField] private TowerManager towerManager;
    private AudioManager audioManagerScript;
    [SerializeField] private GameObject rubblePrefab;
    [SerializeField] private GameObject[] orderedTowerPrefabList;
    private Dictionary<string, GameObject> towerDictionary = new Dictionary<string, GameObject>();
    //dialogues
    [SerializeField] private List<DialogueScene> chap1Dialogue = new List<DialogueScene>();
    [SerializeField] private List<DialogueScene> chap2Dialogue = new List<DialogueScene>();
    [SerializeField] private List<DialogueScene> chap3Dialogue = new List<DialogueScene>();
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform dialoguePanel;
    [SerializeField] private Transform thanksForPlayingPanel;
    [SerializeField] private GameObject thanksText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private Transform bottomBar;
    [SerializeField] private int openingSceneIndex;
    private int currentTransitionIndex; //0 = yes/nopanel; 1 = loadnextscene;
    
    private void Start()
    {
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        Time.timeScale = 1;
        //fill the tower dictionary using the ordered tower list
        towerDictionary.Add("Watch Tower", orderedTowerPrefabList[0]);
        towerDictionary.Add("Splurge Skyscraper", orderedTowerPrefabList[1]);
        towerDictionary.Add("Coffee Column", orderedTowerPrefabList[2]);
        towerDictionary.Add("Media Machine", orderedTowerPrefabList[3]);
        //place towers from previous level here
        foreach (TowerData data in TowerManager.towersAtEndOfLevel)
        {
            TowerController towerInstance = Instantiate(towerDictionary[data.towerName], gridScript.ConvertTileToPosition(data.gridPosition), Quaternion.identity).GetComponent<TowerController>();
            gridScript.AddTowerToGrid(data.gridPosition, towerInstance.Size, towerInstance);
        }
        //place rubble from previous level
        foreach (RubbleData data in TowerManager.rubbleAtEndOfLevel)
        {
            RubbleController rubbleInst = Instantiate(rubblePrefab, gridScript.ConvertTileToPosition(data.gridPosition), Quaternion.identity).GetComponent<RubbleController>();
            rubbleInst.InitRubble(gridScript.ConvertTileToPosition(data.gridPosition), data.pileSize);
            gridScript.AddTowerToGrid(data.gridPosition, data.pileSize, rubbleInst);
        }
        //clear the static info on tower manager
        towerManager.ResetMapState();
        //start dialogue
        int worstLevel = DataStore.GetWorstLevel();
        if (worstLevel == 0)
        {
            StartNewDialogue(chap1Dialogue);
        }
        else if (worstLevel == 1)
        {
            StartNewDialogue(chap2Dialogue);
        }
        else if (worstLevel == 2)
        {
            StartNewDialogue(chap3Dialogue);
        }
    }

    private void StartNewDialogue(List<DialogueScene> dialogue)
    {
        ToggleDialoguePanel(true);
        dialogueManager.ChangeSceneList(dialogue);
        dialogueManager.StartDialogue();
    }

    private void ToggleDialoguePanel(bool turnOn)
    {
        dialoguePanel.gameObject.SetActive(turnOn);
        bottomBar.gameObject.SetActive(!turnOn);
        bottomBar.GetComponent<BottomBarController>().ToggleClicking(!turnOn);
    }

    //called by continue button
    public void AdvanceDialogue()
    {
        bool dialogueOver = false;
        dialogueManager.NextScene(out dialogueOver);
        if (dialogueOver)
        {
            if (currentTransitionIndex == 0)
            {
                ToggleDialoguePanel(false);
                thanksForPlayingPanel.gameObject.SetActive(true);
                audioManagerScript.PlaySound("Keep Looking Up");
            }
            currentTransitionIndex++;
        }
    }

    //called by button of the same name
    public void ToggleUI()
    {
        thanksText.SetActive(!thanksText.activeInHierarchy);
        playButton.SetActive(!playButton.activeInHierarchy);
    }
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(openingSceneIndex);
    }
}
