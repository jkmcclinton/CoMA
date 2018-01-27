using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Tooltip("Delete me! Should spawn from prefab, not reference")]
    public Character reference;
    /// <summary> how long the game goes until the win condition is checked </summary>
    public float LevelTime = 3*60;
    public float enemies = 1;
    [HideInInspector] public bool isMultiplayer = false;

    private GameObject player;
    private GameObject nextLevel;
    FadeController fader;
    private List<GameObject> SpawnPoints;
    private List<Character> NPCs;
    private Sprite[] attributes;

    /// <summary> players maximum possible mood required to cause "Death" condition </summary>
    private const int MAX_LOSE = Character.MOOD_RANGE;
    /// <summary> absolute maximum possible average mood required to cause "Tie" condition </summary>
    private const float TIE_THRESHOLD = Character.MOOD_RANGE / 3f;
    /// <summary> running timer of game </summary>
    private float gameTimer;
    private bool gameRunning = true;

    // list of runtime statistics
    /// <summary> number of times player has converted people </summary>
    private int AgonyConversionCount = 0;
    /// <summary> number of times enemy has converted people </summary>
    private int JoyConversionCount = 0;
    /// <summary> how fast player tended to depress people </summary>
    private float PlayerAggressiveness = 0;

    /// <summary> total score of the game </summary>
    private float score = 0;
    public float LevelScore {
        get { return score; }
    }

	// Use this for initialization
	void Start () {
        fader = GameObject.FindObjectOfType<FadeController>();
        SpawnPoints = new List<GameObject>();
        NPCs = new List<Character>();
        attributes = Resources.LoadAll<Sprite>("Sprites/CoMA People");
        player = GameObject.Find("Player");
        if (reference!=null) SpawnPeople();

        gameTimer = LevelTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameTimer >= 0 && gameRunning) {

            // Game-ending checks

            gameTimer -= Time.deltaTime;
            // Timeover conditions
            if (gameTimer <= 0) {
                if (Mathf.Abs(score) <= TIE_THRESHOLD) {
                    // game is tied! no one wins!

                    // sudden death?
                } else if (score > TIE_THRESHOLD) {
                    // enemy wins! majority is happy!
                } else if (score < TIE_THRESHOLD) {
                    // player wins!!! majority is fucked!
                }

                // endGame
                EndGame();
            }

            if (player.GetComponent<Character>().mood >= MAX_LOSE) {
                // game over! player has become happy


                EndGame();
            }

            // control level


            // tally score
            int sum = 0;
            foreach (Character c in NPCs)
                sum += c.mood;
            score = sum / (float)NPCs.Count;
        }

        // game should be in transition mode otherwise
        // stats should be showing, player goven the opportunity
        // to play again, go to main menu, quit
	}

    public void EndGame() {
        // show stats window
        gameRunning = false;
    }

    public void ReturnToMenu() {
        fader.FadeOut(2);

    }

    public void notify() {
        // transition to next Scene
    }

    // Spawn NPCs in random cubicles, randomize attributes
    public void SpawnPeople() {
        SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject point in SpawnPoints) {
            Transform root = GameObject.Find("NPCs").transform;
            int chance = Random.Range(0, 100);
            if (chance < 70) {
                GameObject npc = GameObject.Instantiate(reference.gameObject,
                    point.transform.position, Quaternion.identity);
                Character c = npc.GetComponent<Character>();
                NPCs.Add(c);
                int range = Character.MOOD_RANGE / 3;
                c.mood = Random.Range(-range, range);                                               // random mood

                GameObject Skin = npc.transform.Find("Sprites/Skin").gameObject;
                GameObject Hair = npc.transform.Find("Sprites/Hair").gameObject;
                GameObject Clothes = npc.transform.Find("Sprites/Clothes").gameObject;

                int gender = Random.Range(0, 1);                                                    // random gender
                Skin.GetComponent<SpriteRenderer>().sprite = attributes[Random.Range(0, 4) + 20];   // random skin
                Hair.GetComponent<SpriteRenderer>().sprite = attributes[gender + 1];                // gender hair
                Clothes.GetComponent<SpriteRenderer>().sprite = attributes[gender + 6];             // gender clothes
                npc.SetActive(true);
                npc.transform.SetParent(root);

                toRemove.Add(point);
            }
        }

        // spawn player in random leftover spawn point
        SpawnPoints = SpawnPoints.Except(toRemove).ToList();
        if (player != null) {
            int num = Random.Range(0, SpawnPoints.Count - 1);
            player.transform.position = SpawnPoints[num].transform.position;
            SpawnPoints.RemoveAt(num);
        }

        // spawn eneme in random leftover spawn point
        for (int i = 0; i < enemies && SpawnPoints.Count>0; i++) {
            int num = Random.Range(0, SpawnPoints.Count - 1);
            GameObject enemy = GameObject.Instantiate(reference.gameObject, SpawnPoints[num].transform.position,
                Quaternion.identity);
            Character eChar = enemy.GetComponent<Character>();
            eChar.type = Character.CharacterClass.bibleThumper;
            enemy.transform.SetParent(GameObject.Find("NPCs").transform);
            enemy.SetActive(true);
            SpawnPoints.RemoveAt(num);
        }
    }


    public void TallyAgonyConversion() {
        this.AgonyConversionCount++;
    }

    public void TallyJoyConversion() {
        this.JoyConversionCount++;
    }
}
