using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using 

[RequireComponent(typeof(Character))]
public class AIMovement : BaseMovement {

	public float time = 0.0f;
	public float moveSpeed = 0.03f;
	private Vector3 targetPos;
	public enum AIState { Idle, Talk, Seek, Enforce };
	public AIState state;

    NavGrid navmesh;
    Transform seeker, target;

     void Awake() {
        navmesh= FindObjectOfType<NavGrid>();
    }

    [HideInInspector]
	private Character character;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
		character = GetComponent<Character> ();
		switch (character.type) {
		case Character.CharacterClass.normie:
			state = AIState.Idle;
			break;
		case Character.CharacterClass.bibleThumper:
		case Character.CharacterClass.meanie:
			moveSpeed = 0.005f;		// Enforcers move slower than civilians due to the fact that they're constantly moving.
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
            FindPath(transform.position, targetPos);
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.02f);
        }
	}

	void updateAI(){
		switch (state) { 
		case AIState.Idle:
			// Continually move towards target location while not at target.
			// Idle for 1.5-3 seconds, then seek out another target.
			if (state == AIState.Idle && transform.position == targetPos) {
				time = Random.Range (1.5f, 3.0f);
				state = AIState.Seek;
			}
			break;
		case AIState.Seek:
			// By default: search for nearby person. If low mood: search for nearby happy person.
			// Else: go randomly.

			// Case 1: Mood is not low. Search for a nearby person.

			// Case 2: Mood is low. Search for nearby happy person.

			// Case 3: Nobody is nearby. Move within random location.
			targetPos.x = (transform.position.x + Random.Range (-1.5f, 1.5f));
			targetPos.y = (transform.position.z + Random.Range (-1.5f, 1.5f));
			state = AIState.Idle;
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
					break;
				}
			}
			break;
		default:
			break;
		}

	}
    
    private void FindPath(Vector2 start, Vector2 target) {
        NavGrid.Node sNode = navmesh.NodeFromWorldPoint(start);
        NavGrid.Node tNode = navmesh.NodeFromWorldPoint(target);

        List<NavGrid.Node> open = new List<NavGrid.Node>();
        HashSet<NavGrid.Node> closed = new HashSet<NavGrid.Node>();
        open.Add(sNode);

        while (open.Count > 0) {
            for (int i = 1; i < open.Count; i++) {
                NavGrid.Node currentNode = open[0];
                if (open[i].f < currentNode.f ||
                    open[i].f == currentNode.f & open[i].h < currentNode.h) {
                    currentNode = open[i];
                }

                open.Remove(currentNode);
                closed.Add(currentNode);

                if (currentNode == tNode) {
                    RetracePath(sNode, tNode);
                    return;

                }

                foreach (NavGrid.Node n in navmesh.GetNeighbors(currentNode)) {
                    if (!n.walkable || closed.Contains(n)) continue;

                    int newC = currentNode.g + getDistance(currentNode, n);
                    if (newC < n.g || open.Contains(n)) {
                        n.g = newC;
                        n.h = getDistance(n, tNode);
                        n.parent = currentNode;

                        if (!open.Contains(n))
                            open.Add(n);
                    }
                }
            }
        }
    }

    

    void RetracePath(NavGrid.Node start, NavGrid.Node end) {
        List <NavGrid.Node > path = new List<NavGrid.Node>();
        NavGrid.Node current = end;
        while (current != start) {
            path.Add(current);
            current = current.parent;
        } path.Reverse();

        navmesh.path = path;
    }

    public int getDistance(NavGrid.Node a, NavGrid.Node b) {
        Vector2 d = new Vector2(Mathf.Abs(a.grid.x - b.grid.x),
            Mathf.Abs(a.grid.y - b.grid.y));

        return (int)((d.x > d.y) ? (14 * d.y + 10 * (d.x - d.y)) : 
            (14 * d.x + 10 * (d.y - d.x)));
    }
}
