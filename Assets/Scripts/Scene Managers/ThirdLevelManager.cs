using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdLevelManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Core coreScript;
    [SerializeField] private GridUtil gridScript;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private GameObject rubblePrefab;
    [SerializeField] private GameObject[] orderedTowerPrefabList;
    private Dictionary<string, GameObject> towerDictionary = new Dictionary<string, GameObject>();
    //dialogues
    [SerializeField] private List<DialogueScene> introDialogue = new List<DialogueScene>();
    [SerializeField] private List<DialogueScene> victoryDialogue = new List<DialogueScene>();
    [SerializeField] private List<DialogueScene> lossDialogue = new List<DialogueScene>();
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform dialoguePanel;
    [SerializeField] private Transform bottomBar;
    [SerializeField] private int nextSceneIndex;
    private int currentTransitionIndex; //0 = yes/nopanel; 1 = loadnextscene;

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            currentTransitionIndex = 1;
            GameWon();
        }*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            DataStore.ToggleClickBoxes();
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        //fill the tower dictionary using the ordered tower list
        towerDictionary.Add("Watch Tower", orderedTowerPrefabList[0]);
        towerDictionary.Add("Splurge Skyscraper", orderedTowerPrefabList[1]);
        towerDictionary.Add("Coffee Column", orderedTowerPrefabList[2]);
        towerDictionary.Add("Social Turret", orderedTowerPrefabList[3]);
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
        StartNewDialogue(introDialogue);
    }

    private void StartGame()
    {
        enemySpawner.StartSpawning();
        coreScript.SetEnergy(700);
        if (!DataStore.prevLevelWon)
        {
            coreScript.SetEnergy(800);
        }
        coreScript.StartPassiveEnergyGain();
        ToggleDialoguePanel(false);
    }

    public void GameLost()
    {
        GameOver(lossDialogue);
        DataStore.prevLevelWon = false;
    }

    public void GameWon()
    {
        GameOver(victoryDialogue);
        DataStore.prevLevelWon = true;
    }

    public void GameOver(List<DialogueScene> conditionedDialogue)
    {
        ToggleDialoguePanel(true);
        Time.timeScale = 0;
        towerManager.LogMapState();
        DataStore.chap3EndHealth = coreScript.Health;
        StartNewDialogue(conditionedDialogue);
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
                StartGame();
            }
            else if (currentTransitionIndex == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
            }
            currentTransitionIndex++;
        }
    }
}
