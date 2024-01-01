using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterSpeaker")]
public class CharacterSpeakers : ScriptableObject
{
    public string characterName;
    public Sprite characterNameTag;
    private Dictionary<string, Sprite> emotionSprites = new Dictionary<string, Sprite>();
    public enum Attitudes { Pushy, Excited, Resigned, Shy };
    //The order of these two arrays must correspond
    private string[] attitudes = new string[] { "Pushy", "Excited", "Resigned", "Shy" };
    private Sprite[] sprites; //order set in SetSprites method

    //Sprites are set in editor
    [SerializeField] private Sprite pushySprite;
    [SerializeField] private Sprite excitedSprite;
    [SerializeField] private Sprite resignedSprite;
    [SerializeField] private Sprite shySprite;

    //Called by Scene Manager at start of scene
    public void SetSprites()
    {
        sprites = new Sprite[]{ pushySprite, excitedSprite, resignedSprite, shySprite };
        for (int i = 0; i < sprites.Length; i++)
        {
            emotionSprites.Add(attitudes[i], sprites[i]);
        }
    }

    //called by Dialogue manager to set scene
    public Sprite GetSprite(string attitude)
    {
        return emotionSprites[attitude];
    }
}
