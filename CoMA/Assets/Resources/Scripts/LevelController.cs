﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LD_Parallax))]
public class LevelController : MonoBehaviour {
    
    /// <summary> how long the game goes until the win condition is checked </summary>
    public float LevelTime = 3*60;
    public float enemies = 1;
    [HideInInspector] public bool isMultiplayer = false;

    private GameObject player, cubicle;
    private GameObject nextLevel;
    FadeController fader;
    private List<GameObject> SpawnPoints;
    private List<Character> NPCs;
    private Sprite[] attributes;
	public bool[,] navMap;

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
    private Character reference;

	private LD_Parallax parallax;

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
    public enum LCState { StartGame, SwitchScene, Doso}

	// Use this for initialization
	void Start () {
		parallax = FindObjectOfType<LD_Parallax> ();
        fader = GameObject.FindObjectOfType<FadeController>();
        SpawnPoints = new List<GameObject>();
        NPCs = new List<Character>();
        attributes = Resources.LoadAll<Sprite>("Sprites/CoMA People");
        player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().canMove = false;

        cubicle = Resources.Load<GameObject>("Prefabs/Cubicle");
        SpawnCubicles();
		navMap = generateNavMap ();
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

                        // sudden death?
                    } else if (score > TIE_THRESHOLD) {
                        // enemy wins! majority is happy!
                    } else if (score < TIE_THRESHOLD) {
                        // player wins!!! majority is fucked!

                    }

                    // endGame
                    EndGame();
                }
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

    public void Notify() {
        switch (lcState) {
            case LCState.SwitchScene:
                // transition to next Scene
                break;
            case LCState.StartGame:
                Debug.Log("StartSplash");
                SplashController splash = FindObjectOfType<SplashController>();
                splash.GetComponent<Animator>().SetTrigger("StartSplash");
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

    public void SpawnCubicles() {
        int count = 0;
        for (int h = 0; h < 3; h++) {
            for (int w = -4; w < 5; w++) {
                if (w != 0) {
                    GameObject inst = Instantiate(cubicle, new Vector3(w, 2 - 2 * h), Quaternion.identity) as GameObject;
                    inst.transform.parent = GameObject.Find("Level").transform;
                    inst.name = "Cubicle" + count.ToString();

                    if (h == 0) {
                        inst.GetComponent<SpriteRenderer>().sortingOrder = -4110;
                    } else if (h == 1) {
                        inst.GetComponent<SpriteRenderer>().sortingOrder = -110;
                    } else {
                        inst.GetComponent<SpriteRenderer>().sortingOrder = 3830;
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
                npc.GetComponent<AIMovement>().canMove = false;

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





	public bool[,] generateNavMap () {
		float indexToPos = 1 / 10;
		int posToIndex = 10;

		Vector2 mapOrigin = Vector2.zero;
		Vector2 left = (Vector2)transform.GetChild (0).GetChild (0).position;
		Vector2 right = (Vector2)transform.GetChild (0).GetChild (1).position;
		Vector2 up = (Vector2)transform.GetChild (0).GetChild (2).position;
		Vector2 down = (Vector2)transform.GetChild (0).GetChild (3).position;
		Vector2 mapSize = new Vector2 ((right.x - left.x), (up.y - down.y));
		//Vector2 posToIndex = new Vector2 (1 / samplesPerUnitX, 1 / samplesPerUnitY);
		//Vector2 indexToPos = new Vector2 (samplesPerUnitX, samplesPerUnitY);

		navMap = new bool[Mathf.RoundToInt(mapSize.x * posToIndex) + 1, Mathf.RoundToInt(mapSize.y * posToIndex) + 1];
		print ("Map Size: " + mapSize);
		print ("NavMap Size: [" + navMap.GetLength (0) + " ," + navMap.GetLength (1) + "]");

		for (int i = 0; i < navMap.GetLength(0); i++) {
			for (int j = 0; j < navMap.GetLength(1); j++) {
				navMap [i, j] = false;
			}
		}

		BoxCollider2D[] currentCols = FindObjectsOfType<BoxCollider2D> ();

		foreach (BoxCollider2D col in currentCols) {
			if (col.gameObject.layer != LayerMask.NameToLayer ("Default"))
				continue;

			Vector2 colPos = col.transform.TransformPoint (col.offset);
			Vector2 colSize = col.size;
			Vector2 gridUL = new Vector2(colPos.x - colSize.x, colPos.x + colSize.y);
			ULs.Add (gridUL);
			Vector2 gridUR = new Vector2(colPos.x + colSizes.x, colPos.x + colSize.y);
			Vector2 gridLL = new Vector2(colPos.x - colSize.x, colPos.x - colSize.y);
			Vector2 gridLR = new Vector2(colPos.x + colSize.x, colPos.x - colSize.y);

			print ("UL: " + gridUL + "\tUR: " + gridUR + "\tLL: " + gridLL + "LR: " + gridLR);

			navMap[Mathf.RoundToInt(gridUL.x * posToIndex + mapSize.x / 2), Mathf.RoundToInt(gridUL.y * posToIndex + mapSize.y / 2)] = true;
			navMap[Mathf.RoundToInt(gridUR.x * posToIndex + mapSize.x / 2), Mathf.RoundToInt(gridUR.y * posToIndex + mapSize.y / 2)] = true;
			navMap[Mathf.RoundToInt(gridLL.x * posToIndex + mapSize.x / 2), Mathf.RoundToInt(gridLL.y * posToIndex + mapSize.y / 2)] = true;
			navMap[Mathf.RoundToInt(gridLR.x * posToIndex + mapSize.x / 2), Mathf.RoundToInt(gridLR.y * posToIndex + mapSize.y / 2)] = true;

		}

		return navMap;
	}

	List<Vector2> ULs;

	void OnDrawGizmos () {
		Gizmos.color = Color.red;

		for (int i = 0; i < max; i++) {
			
		}
	}

	/*
	public bool[,] generateNavMap() {
		float resolution = 10f;
		Vector2 originPoint = parallax.origin;
		Vector2 boundary = parallax.length;
		Vector2 colliderSize = new Vector2 (0, 0);
		Vector2 colliderPosition = new Vector2(0, 0);
		// Upper-right, Upper-left, Lower-right, Lower-left
		Vector2 colliderUR, colliderUL, colliderLR, colliderLL;
		float boundaryX = boundary.x / resolution;
		float boundaryY = boundary.y / resolution;

		bool[,] navMap = new bool[Mathf.RoundToInt(Mathf.Abs(boundaryX)), Mathf.RoundToInt(Mathf.Abs(boundaryY))];
		// Initially initialize the boolean map to all false (no collisions).
		for (int x = 0; x < navMap.GetLength (0); x++) {
			for (int y = 0; y < navMap.GetLength (1); y++) {
				navMap [x, y] = false;
			}
		}

		BoxCollider2D[] currentCols = FindObjectsOfType<BoxCollider2D> ();

		foreach (BoxCollider2D boxBounds in currentCols){
			colliderPosition = boxBounds.transform.position;
			colliderSize = boxBounds.size;

			// Multiplied by a factor of 10 to account for the divide-by-10 in the original part of the function.
			// The factor is halved for size (due to how colliders work).
//			colliderUR = new Vector2(((colliderPosition.x * resolution) + (colliderSize.x * 5)), ((colliderPosition.y * resolution) + (colliderSize.y * 5)));
//			colliderUL = new Vector2(((colliderPosition.x * resolution) - (colliderSize.x * 5)), ((colliderPosition.y * resolution) + (colliderSize.y * 5)));
//			colliderLL = new Vector2(((colliderPosition.x * resolution) - (colliderSize.x * 5)), ((colliderPosition.y * resolution) - (colliderSize.y * 5)));
//			colliderLR = new Vector2(((colliderPosition.x * resolution) + (colliderSize.x * 5)), ((colliderPosition.y * resolution) - (colliderSize.y * 5)));
			colliderUR 


			//Mathf.RoundToInt (colliderUR.x); Mathf.RoundToInt (colliderUR.y);
			//Mathf.RoundToInt (colliderUL.x); Mathf.RoundToInt (colliderUL.y);
			//Mathf.RoundToInt (colliderLL.x); Mathf.RoundToInt (colliderLL.y);
			for (int x = 0; x < (Mathf.RoundToInt (colliderLR.x) - Mathf.RoundToInt (colliderLL.x)); x++) {
				for (int y = 0; y < (Mathf.RoundToInt (colliderUR.y) - Mathf.RoundToInt (colliderLR.y)); y++) {
					Debug.Log ("Collision at: " + x + " " + y);
					//Debug.Log (Mathf.RoundToInt (colliderLL.x) + x);
					navMap [(Mathf.RoundToInt (colliderLL.x) + x), (Mathf.RoundToInt (colliderLR.y) + y)] = true;
				}
			}

		}

		return navMap;
	}*/
}
