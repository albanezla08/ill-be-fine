using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstLevelManager : MonoBehaviour
{
    public static bool tutorialActive;
    public static bool upkeepAllowed = true;
    //public static Action onTutorialTowerPlaced; //called by towerMarker when tutTower is placed
    //public static Action onUpkeepPressed; //called by bottomBar when upkeep pressed
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private Core coreScript;
    [SerializeField] private GameObject yesOrNoPanel;
    private TowerController tutTowerScript;
    //tutorial arrows
    [SerializeField] private GameObject towerUIArrow;
    [SerializeField] private GameObject towerWorldArrow;
    [SerializeField] private GameObject towerPlaceMat;
    [SerializeField] private GameObject upkeepUIArrow;
    [SerializeField] private GameObject upkeepWorldArro;
    //dialogues
    [SerializeField] private List<DialogueScene> wantTutorialDialogue;
    [SerializeField] private List<DialogueScene> placeTowerDialogue;
    [SerializeField] private List<DialogueScene> upkeepTowerDialogue;
    [SerializeField] private List<DialogueScene> concludeTutorialDialogue;
    [SerializeField] private List<DialogueScene> victoryDialogue = new List<DialogueScene>();
    [SerializeField] private List<DialogueScene> lossDialogue = new List<DialogueScene>();
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform dialoguePanel;
    [SerializeField] private Transform bottomBar;
    private bool gameEnded;
    [SerializeField] private int finalSceneIndex;
    private int currentTransitionIndex; //0 = yes/nopanel; 1 = loadnextscene;
    
    private void Start()
    {
        Time.timeScale = 1;
        /*enemySpawner.StartSpawning();*/
        StartNewDialogue(wantTutorialDialogue);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            currentTransitionIndex = 4;
            GameWon();
        }*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            DataStore.ToggleClickBoxes();
        }
    }

    public void YesPressed()
    {
        yesOrNoPanel.SetActive(false);
        tutorialActive = true;
        upkeepAllowed = false;
        /*onTutorialTowerPlaced += TowerPlacedResponse;
        onUpkeepPressed = UpkeepPressedResponse;*/
        coreScript.SetEnergy(300);
        StartNewDialogue(placeTowerDialogue);
        enemySpawner.TutorialSpawn();
    }
    public void NoPressed()
    {
        yesOrNoPanel.SetActive(false);
        currentTransitionIndex = 4; //set to last piece of dialogue
        enemySpawner.StartSpawning();
        coreScript.StartPassiveEnergyGain();
    }

    private void SetUpTowerPlacement()
    {
        ToggleDialoguePanel(false);
        //put in  a visual indicator of wereh to place tower
        towerUIArrow.SetActive(true);
        towerWorldArrow.SetActive(true);
        towerPlaceMat.SetActive(true);
    }

    public void TowerPlacedResponse()
    {
        towerUIArrow.SetActive(false);
        towerWorldArrow.SetActive(false);
        towerPlaceMat.SetActive(false);
        StartCoroutine(TowerPlaceResponseEndFrame());
    }
    public IEnumerator TowerPlaceResponseEndFrame()
    {
        yield return new WaitForEndOfFrame();
        tutTowerScript = towerManager.GetFirstTowerInScene();
        enemySpawner.TutorialMove();
        StartCoroutine(WaitForEnemyDeath());
    }

    IEnumerator WaitForEnemyDeath()
    {
        yield return new WaitForSeconds(13);
        StartNewDialogue(upkeepTowerDialogue);
        tutTowerScript.PauseRubbleTimer();
    }

    private void SetUpUpkeepPress()
    {
        upkeepAllowed = true;
        ToggleDialoguePanel(false);
        //visual indicators
        upkeepUIArrow.SetActive(true);
        upkeepWorldArro.SetActive(true);
    }

    public void UpkeepPressedResponse()
    {
        upkeepUIArrow.SetActive(false);
        upkeepWorldArro.SetActive(false);
        StartCoroutine(ConcludeDialogueOnDelay());
    }

    IEnumerator ConcludeDialogueOnDelay()
    {
        yield return new WaitForSeconds(1.4f);
        StartNewDialogue(concludeTutorialDialogue);
    }

    private void StartGame()
    {
        tutorialActive = false;
        tutTowerScript.RemoveTowerNoRubble();
        enemySpawner.StartSpawning();
        coreScript.SetEnergy(700);
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
        DataStore.chap1EndHealth = coreScript.Health;
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
                ToggleDialoguePanel(false);
                yesOrNoPanel.SetActive(true);
            }
            else if (currentTransitionIndex == 1)
            {
                SetUpTowerPlacement();
            }
            else if (currentTransitionIndex == 2)
            {
                SetUpUpkeepPress();
            }
            else if (currentTransitionIndex == 3)
            {
                StartGame();
            }
            else if (currentTransitionIndex == 4)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(finalSceneIndex);
            }
            currentTransitionIndex++;
        }
    }
}
