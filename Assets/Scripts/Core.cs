using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Core : MonoBehaviour
{
    //Check for loss
    public int Health;
    [SerializeField] private int energy = 600;
    [SerializeField] private int energyGainIncrement = 50;
    [SerializeField] private float energyGainCooldown = 5f;
    private float energyGainTimer = 0f;
    [SerializeField] private FirstLevelManager sceneManager;
    [SerializeField] private SecondLevelManager sceneManager2;
    [SerializeField] private ThirdLevelManager sceneManager3;
    /*[SerializeField] private SecondLevelManager sceneManager3;*/
    private AudioManager audioManagerScript;
    [SerializeField] private BottomBarController bottomBarScript;
    [SerializeField] private TextMeshProUGUI gameText;
    [SerializeField] private TextMeshProUGUI[] hpText;
    [SerializeField] private TextMeshProUGUI[] energyText;
    //energy change visualization
    [SerializeField] private GameObject energyChangeVisualPrefab;
    [SerializeField] private Color gainColor;
    [SerializeField] private Color lossColor;

    public int CurrentEnergy
    {
        get
        {
            return energy;
        }
    }

    //Check for win
    bool gameLost;
    int enemiesOnMap;
    bool doneSpawning;

    private void Awake()
    {
        move.DealDamage += RecieveDamage;
    }

    private void Start()
    {
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        UpdateAllHPText();
        UpdateAllEnergyText();
    }

    private void OnDestroy()
    {
        move.DealDamage -= RecieveDamage;
    }

    public void StartPassiveEnergyGain()
    {
        StartCoroutine(PassiveEnergyGain());
    }
    private IEnumerator PassiveEnergyGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(energyGainCooldown);
            GainEnergy(energyGainIncrement);
            /*audioManagerScript.PlaySound("Core Energy Gain");*/
            /*energyGainTimer += Time.deltaTime;
            if (energyGainTimer >= energyGainCooldown)
            {
                energyGainTimer = 0;
                GainEnergy(energyGainIncrement);
            }
            yield return null;*/
        }
    }

    public void RecieveDamage(int Dam)
    {
        audioManagerScript.PlaySound("Core Hit");
        if (Health > 0)
        {
            Health -= Dam;
            UpdateAllHPText();
        }
        if (Health <= 0)
        {
            /*Debug.Log("game over");
            gameText.text = "You Lose.";*/
            audioManagerScript.StopSound("Dreams");
            audioManagerScript.PlaySound("Round Loss");
            gameLost = true;
            /*Time.timeScale = 0;*/
            if (sceneManager != null) //changed recently
            {
                sceneManager.GameLost();
            }
            else if (sceneManager2 != null)
            {
                sceneManager2.GameLost();
            }
            else if (sceneManager3 != null)
            {
                sceneManager3.GameLost();
            }
            Health = -1;
        }
    }

    //called by move script
    public void EnemyEnteredMap()
    {
        enemiesOnMap++;
    }

    //called by Tower Manager
    public void EnemyLeftMap()
    {
        enemiesOnMap--;
        if (!gameLost && doneSpawning && enemiesOnMap == 0)
        {
            /*print("game over");
            gameText.text = "You Win!";*/
            audioManagerScript.StopSound("Dreams");
            if (sceneManager != null) //changed recently
            {
                sceneManager.GameWon();
            }
            else if (sceneManager2 != null)
            {
                sceneManager2.GameWon();
            }
            else if (sceneManager3 != null)
            {
                sceneManager3.GameWon();
            }
        }
    }

    //called by EnemySpawner
    public void EnemiesDoneSpawning()
    {
        doneSpawning = true;
        if (!gameLost && enemiesOnMap == 0)
        {
            /*print("game over");
            gameText.text = "You Win!";*/
            if (sceneManager != null) //changed recently
            {
                sceneManager.GameWon();
            }
            else if (sceneManager2 != null)
            {
                sceneManager2.GameWon();
            }
            else if (sceneManager3 != null)
            {
                sceneManager3.GameWon();
            }
        }
    }

    //Two money methods which affect UI; both called by towermanager and bottombar
    public bool CanAfford(int money)
    {
        return energy >= money;
    }

    public void SpendEnergy(int money)
    {
        energy -= money;
        PlayEnergyChangeAnim(lossColor, "-" + money);
        UpdateAllEnergyText();
    }

    public void GainEnergy(int money)
    {
        energy += money;
        PlayEnergyChangeAnim(gainColor, "+" + money);
        UpdateAllEnergyText();
    }

    public void SetEnergy(int value)
    {
        energy = value;
        UpdateAllEnergyText();
    }

    private void PlayEnergyChangeAnim(Color visualColor, string changeText)
    {
        GameObject animationInstance = Instantiate(energyChangeVisualPrefab, bottomBarScript.transform);
        TextMeshProUGUI energyChangeText = animationInstance.GetComponent<TextMeshProUGUI>();
        energyChangeText.color = visualColor;
        energyChangeText.text = changeText;
        StartCoroutine(DestroyEnergyAnimOnDelay(animationInstance));
    }

    private IEnumerator DestroyEnergyAnimOnDelay(GameObject animObject)
    {
        yield return new WaitForSeconds(1); //1 is the length of the animation
        Destroy(animObject);
    }

    private void UpdateAllHPText()
    {
        foreach (TextMeshProUGUI hp in hpText)
        {
            hp.text = Health.ToString();
        }
    }

    private void UpdateAllEnergyText()
    {
        foreach (TextMeshProUGUI energy in energyText)
        {
            energy.text = this.energy.ToString();
        }
        bottomBarScript.UpdateMoneyButtons();
    }

    //made because i dont know how to code
    public void CallBottomBarRoutine(GameObject animObject)
    {
        StartCoroutine(bottomBarScript.DestroyEnergyAnimOnDelay(animObject));
    }
}
