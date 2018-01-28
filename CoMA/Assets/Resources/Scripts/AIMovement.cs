using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using 

[RequireComponent(typeof(Character))]
public class AIMovement : BaseMovement {

	public float time = 0.0f;
	public float pathTime = 3.0f;
	public float moveSpeed = 0.03f;
	public Vector3 targetPos;
	public enum AIState { Idle, Talk, Seek, Enforce };
	public AIState state;

    NavGrid navmesh;
    public Transform seeker, target;

     void Awake() {
        navmesh= FindObjectOfType<NavGrid>();
    }

	public List<NavGrid.Node> path;

    [HideInInspector]
	private Character character;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
		character = GetComponent<Character> ();
		switch (character.type) {
		case Character.CharacterClass.normie:
			state = AIState.Seek;
			break;
		case Character.CharacterClass.bibleThumper:
		case Character.CharacterClass.meanie:
			moveSpeed = 0.015f;		// Enforcers move slower than civilians due to the fact that they're constantly moving.
			state = AIState.Enforce;
			break;
		default:
			break;
		}
	}

	void Update(){

		// Update AI pathing every time Time hits 0.
		if (time > 0) {
			time -= Time.deltaTime;
		} else {
			updateAI ();
		}

		if (canMove) {
			
			if (path != null && path.Count > 0 && pathTime > 0f) {
				// If it takes >5 seconds (stuck on wall) just find a new path entirely.
				pathTime -= Time.deltaTime;

				if (Vector3.Distance (transform.position, path [0].loc) <= 0.001f) {
					path.RemoveAt(0);					
				} else {
					transform.position = Vector2.MoveTowards (transform.position, path [0].loc, moveSpeed);
				}
			} else if (path == null || path.Count == 0 || pathTime <= 0f) {
				pathTime = 5.0f;
				state = AIState.Seek;
				FindPath (transform.position, targetPos);
			}
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.02f);
        }
	}

	void updateAI(){
		switch (state) { 
		case AIState.Idle:
			// Continually move towards target location while not at target.
			// Idle for 1.5-3 seconds, then seek out another target.
			if (state == AIState.Idle && transform.position == targetPos) {
				state = AIState.Seek;
			}
			break;
		case AIState.Seek:
			// By default: search for nearby person. If low mood: search for nearby happy person.
			// Else: go randomly.

			// Case 1: Mood is not low. Search for a nearby person.
			// Case 2: Mood is low. Search for nearby happy person.
			// Case 3: Nobody is nearby. Move within random location.

			RaycastHit2D[] nearbyPeople = Physics2D.CircleCastAll (transform.position, 3.0f, Vector2.zero, 0.0f, LayerMask.NameToLayer ("NPC"));
			if (nearbyPeople.Length != 0) {
				targetPos = nearbyPeople[0].point;
				Debug.Log ("Nearby person found!!");
			} else {
				targetPos.x = (transform.position.x + Random.Range (-1.5f, 1.5f));
				targetPos.y = (transform.position.z + Random.Range (-1.5f, 1.5f));
				Debug.Log ("Random position!");
			}
			state = AIState.Idle;
			time = Random.Range (2.5f, 3.25f);

			break;
		case AIState.Talk:
			// Placeholder for now.
			break;
		case AIState.Enforce:
			// Essentially an uber-seek. Stay ultra close to a target.
			Character[] charArray = FindObjectsOfType<Character> ();

			foreach (Character target in charArray) {
				if (target.type == Character.CharacterClass.normie) {
					targetPos.x = target.transform.position.x + Random.Range(-0.5f, 0.5f);
					targetPos.y = target.transform.position.y + Random.Range(-0.5f, 0.5f);
					time = Random.Range (0.5f, 1.5f);
				}
			}
			break;
		default:
			break;
		}

	}
    
    void FindPath(Vector3 startPos, Vector3 targetPos) {
		NavGrid.Node startNode = navmesh.NodeFromWorldPoint(startPos);
        NavGrid.Node targetNode = navmesh.NodeFromWorldPoint(targetPos);

		List<NavGrid.Node> openSet = new List<NavGrid.Node>();
		HashSet<NavGrid.Node> closedSet = new HashSet<NavGrid.Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
            NavGrid.Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].f < node.f || openSet[i].f == node.f) {
					if (openSet[i].h < node.h)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (NavGrid.Node neighbour in navmesh.GetNeighbors(node)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.g + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.g || !openSet.Contains(neighbour)) {
					neighbour.g = newCostToNeighbour;
					neighbour.h = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}


    void RetracePath(NavGrid.Node startNode, NavGrid.Node endNode) {
        List<NavGrid.Node> path = new List<NavGrid.Node>();
        NavGrid.Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        navmesh.path = path;
		this.path = path;
    }

    int GetDistance(NavGrid.Node nodeA, NavGrid.Node nodeB) {
        int dstX = (int)Mathf.Abs(nodeA.grid.x - nodeB.grid.x);
        int dstY = (int)Mathf.Abs(nodeA.grid.y - nodeB.grid.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
