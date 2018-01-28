using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    
    /// <summary> how long the game goes until the win condition is checked </summary>
    public float LevelTime = 3*60;
    public float enemies = 1;
    [HideInInspector] public bool isMultiplayer = false;

    NavGrid A_star;
    private GameObject player, cubicle;
    private string nextLevel;
    private Animator splashAnim;
    FadeController fader;
    private List<GameObject> SpawnPoints;
    private List<Character> NPCs;
    private Sprite[] attributes;

    public float time { get { return gameTimer; } }
    public void runMe() { gameRunning = true; }

    /// <summary> players maximum possible mood required to cause "Death" condition </summary>
    private const int MAX_LOSE = Character.MOOD_RANGE;
    /// <summary> absolute maximum possible average mood required to cause "Tie" condition </summary>
    private const float TIE_THRESHOLD = Character.MOOD_RANGE / 3f;
    /// <summary> running timer of game </summary>
    private float gameTimer;
    private bool gameRunning = true;

    /// <summary> refernce to NPC prefab  </summary>
    private Character reference, bible;
    
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
    
    public LevelController(LCState state) {
        this.lcState = state;
    }

    [HideInInspector]
    public LCState lcState = LCState.StartGame;
    public enum LCState { StartGame, SwitchScene, ShowGOMenu, CloseApplication, RestartGame}

	// Use this for initialization
	void Start () {
        fader = GameObject.FindObjectOfType<FadeController>();
        SplashController splash = FindObjectOfType<SplashController>();
        splashAnim = splash.GetComponent<Animator>();
        SpawnPoints = new List<GameObject>();
        NPCs = new List<Character>();
        attributes = Resources.LoadAll<Sprite>("Sprites/CoMA People");
        player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().canMove = false;
        A_star = GetComponent<NavGrid>();

        cubicle = Resources.Load<GameObject>("Prefabs/Cubicle");
        SpawnCubicles();

        if (A_star != null)
            A_star.GenerateMap();

        bible = Resources.Load<GameObject>("Prefabs/Bible").GetComponent<Character>();
        reference = Resources.Load<GameObject>("Prefabs/NPC").GetComponent<Character>();
        if (reference!=null) SpawnPeople();
            gameTimer = LevelTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameTimer >= 0 && gameRunning) {
            if (gameRunning) { 
            // Game-ending checks

            gameTimer -= Time.deltaTime;
                // Timeover conditions
                if (gameTimer <= 0) {
                    if (Mathf.Abs(score) <= TIE_THRESHOLD) {
                        // game is tied! no one wins!
                        splashAnim.SetInteger("WinState", 0);
                        // sudden death?
                    } else if (score > TIE_THRESHOLD) {
                        // enemy wins! majority is happy!
                        splashAnim.SetInteger("WinState", -1);
                    } else if (score < TIE_THRESHOLD) {
                        // player wins!!! majority is fucked!
                        splashAnim.SetInteger("WinState", 1);

                    }

                    // endGame
                    lcState = LCState.ShowGOMenu;
                    splashAnim.SetTrigger("TimeSplash");
                }
            }

            if (player.GetComponent<Character>().mood >= MAX_LOSE) {
                // game over! player has become happy
                lcState = LCState.ShowGOMenu;
                splashAnim.SetTrigger("DeathSplash");
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

    public void RestartGame() {
        lcState = LCState.RestartGame;
        fader.FadeOut(2);
    }

    public void QuitGame() {
        if (MsgBox.Confirm("Are you sure about that?")) {
            lcState = LCState.CloseApplication;
            fader.FadeOut(3);
        }
    }

    public void ReturnToMenu() {
        lcState = LCState.SwitchScene;
        nextLevel = "MainMenu";
        fader.FadeOut(2);
    }

    public void Notify() {
        switch (lcState) {
            case LCState.SwitchScene:
                // transition to next Scene
                SceneManager.LoadScene(nextLevel);
                break;
            case LCState.StartGame:
                splashAnim.SetTrigger("StartSplash");
                break;
            case LCState.CloseApplication:
                Application.Quit();
                break;
            case LCState.RestartGame:
                // reset all things

                foreach (Transform npc in GameObject.Find("NPCs").transform) {
                    GameObject.Destroy(npc.gameObject);
                }

                Character player = GameObject.Find("Player").GetComponent<Character>();
                player.mood = player.defaultMood = -Character.MOOD_RANGE;
                //reset cool downs

                A_star.ResetMap();

                gameTimer = LevelTime;
                lcState = LCState.StartGame;
                fader.FadeIn();
                break;
        }
    }

    /// <summary>
    /// Formats a float in seconds to a string in mm:ss
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string formatTime(float t) {
        return (t < 60) ? "00" : f(2, t / 60) + ":" + f(2, t % 60);
    }

    public static string f(int min, float t) {
        string s =((int) t).ToString();
        if (s.Length < 2) s = "0" + s;
        return s;
    }
    #region Spawn
    public void SpawnCubicles() {
        int count = 0;
        for (int h = 0; h < 3; h++) {
            for (int w = -4; w < 5; w++) {
                if (w != 0) {
                    GameObject inst = Instantiate(cubicle, new Vector3(w, 2 - 2 * h), Quaternion.identity) as GameObject;
                    inst.transform.parent = GameObject.Find("Level").transform;
                    inst.name = "Cubicle" + count.ToString();

                    if (h == 0) {
						inst.GetComponent<SpriteRenderer>().sortingOrder = -4510;
                    } else if (h == 1) {
                        inst.GetComponent<SpriteRenderer>().sortingOrder = -360;
                    } else {
                        inst.GetComponent<SpriteRenderer>().sortingOrder = 3520;
                    }

                    for (int i = 1; i <= inst.transform.childCount; i++) {
                        if (inst.transform.GetChild(i - 1).GetComponent<SpriteRenderer>()) {
                            inst.transform.GetChild(i - 1).GetComponent<SpriteRenderer>().sortingOrder = inst.GetComponent<SpriteRenderer>().sortingOrder + i * 2;

                            for (int j = 1; j <= inst.transform.GetChild(i - 1).childCount; j++) {
                                inst.transform.GetChild(i - 1).GetChild(i - 1).GetComponent<SpriteRenderer>().sortingOrder = inst.GetComponent<SpriteRenderer>().sortingOrder + i * 2 + j;
                            }
                        }
                    }

                    count++;
                }
            }
        }
    }

    // Spawn NPCs in random cubicles, randomize attributes
    public void SpawnPeople() {
        SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject point in SpawnPoints) {
            Transform root = GameObject.Find("NPCs").transform;
            int chance = UnityEngine.Random.Range(0, 100);

			int typeChance = UnityEngine.Random.Range (0, 100);

			Character chosenType;
			if (typeChance <= 90) {
				chosenType = reference;
			} else {
				chosenType = bible;
			}

            if (chance < 50) {
                GameObject npc = GameObject.Instantiate(reference.gameObject,
                    point.transform.position, Quaternion.identity);
                Character c = npc.GetComponent<Character>();
                NPCs.Add(c);
                int range = Character.MOOD_RANGE / 3;
                c.mood = UnityEngine.Random.Range(-range, range);                                               // random mood


                npc.GetComponent<AIMovement>().canMove = false;

                int gender = UnityEngine.Random.Range(0, 2);                                                  // random gender
                c.Skin.sprite = attributes[UnityEngine.Random.Range(0, 4) + 20];   // random skin
                c.Hair.GetComponent<SpriteRenderer>().sprite = attributes[gender + 1];                // gender hair
                c.Clothes.GetComponent<SpriteRenderer>().sprite = attributes[gender + 6];             // gender clothes
                npc.SetActive(true);
                npc.transform.SetParent(root);


                toRemove.Add(point);

				if (chosenType != reference) {
					c.BecomeEnforcer (false);
				}
            }
        }
        
        // spawn player in random leftover spawn point
        SpawnPoints = SpawnPoints.Except(toRemove).ToList();
        if (player != null) {
            int num = UnityEngine.Random.Range(0, SpawnPoints.Count - 1);
            player.transform.position = SpawnPoints[num].transform.position;
            SpawnPoints.RemoveAt(num);
        }

        // spawn eneme in random leftover spawn point
        //for (int i = 0; i < enemies; i++) {
        //    int num = UnityEngine.Random.Range(0, SpawnPoints.Count - 1);
            //GameObject enemy = GameObject.Instantiate(reference.gameObject, SpawnPoints[num].transform.position,
            //    Quaternion.identity);
            //Character eChar = enemy.GetComponent<Character>();
            //eChar.type = Character.CharacterClass.bibleThumper;
            //enemy.transform.SetParent(GameObject.Find("NPCs").transform);
            //enemy.SetActive(true);
            //SpawnPoints.RemoveAt(num);
        //}
    }
    
    public void TallyAgonyConversion() {
        this.AgonyConversionCount++;
    }

    public void TallyJoyConversion() {
        this.JoyConversionCount++;
    }
    #endregion

}
