using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour {

    Node[,] navMap;
    public LayerMask unwalkableMask;
    public float nodeDiameter = 1;
    private float nodeRad { get { return nodeDiameter / 2; } }
    Vector2 mapSize;
   
    
    public Vector2 worldSize;

    // Use this for initialization
    void Start() {
        unwalkableMask = LayerMask.NameToLayer("Obstacle");
        //path = new List<Node>();
    }


    public List<Node> GetNeighbors(Node node) {
        List<Node> n = new List<Node>();
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) continue;

                Vector2 c = new Vector2(node.grid.x + x, node.grid.y + y);

                if(c.x>=0 &&c.x < mapSize.x && c.y >= 0 && c.y < mapSize.y) {
                    n.Add(navMap[(int)c.x, (int)c.y]);
                }
            }
        }

        return n;
    }

    public void ResetMap() {
        navMap = null;
    }

    public Node NodeFromWorldPoint(Vector2 pos) {
        Vector2 percent = new Vector2(Mathf.Clamp01((pos.x+worldSize.x/2f)/worldSize.x),
            Mathf.Clamp01((pos.y + worldSize.y / 2f) / worldSize.y));

        int x = Mathf.RoundToInt((mapSize.x - 1f) * percent.x);
        int y = Mathf.RoundToInt((mapSize.y - 1f) * percent.y);
        return navMap[x, y];
    }


    private static LayerMask invert(LayerMask mask) {
        return 1 << LayerMask.NameToLayer(mask.ToString());
    }

    public void GenerateMap() {

        mapSize = new Vector2(Mathf.RoundToInt(worldSize.x / nodeDiameter),
                            Mathf.RoundToInt(worldSize.y / nodeDiameter));
        navMap = new Node[(int)mapSize.x, (int)mapSize.y];
        Vector2 wbm = transform.position - Vector3.right * worldSize.x / 2f - Vector3.up * worldSize.y / 2f;
        
        for (int x = 0; x < navMap.GetLength(0); x++) {
            for (int y = 0; y < navMap.GetLength(1); y++) {
                Vector2 worldPoint = wbm + Vector2.right * ( x * nodeDiameter+nodeRad) +
                    Vector2.up * ( y * nodeDiameter +nodeRad );
                //Physics2D.
                bool w = Physics2D.BoxCast(worldPoint, Vector2.one*nodeDiameter, 0, 
                    Vector2.zero, 0, unwalkableMask).collider!=null;
                navMap[x, y] = new Node(!w, worldPoint, new Vector2(x, y));
            }
        }
        
    }

    public List<Node> path = new List<Node>();
    

    private void OnDrawGizmos() {
		if (path != null)
			foreach (Node n in path)
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y,1)); 
        if (navMap != null) {
            Node p = NodeFromWorldPoint(GameObject.Find("Player").transform.position);
            Node C = NodeFromWorldPoint(GameObject.Find("NPC(Clone)").transform.position);
            foreach (Node n in navMap) {
                
                Gizmos.color = n.walkable ? Color.green : Color.red;
				if (p == n ) Gizmos.color = Color.blue;
				if (n == C ) Gizmos.color = Color.yellow;

                Gizmos.DrawCube(n.loc,  Vector2.one*nodeRad*1.5f);
            }
		}
		if (path != null) {
			foreach(Node t in path){
				Gizmos.color = Color.black;
				Gizmos.DrawCube(t.loc, Vector2.one*nodeRad*1.5f);
			}
		}
    }


    public class Node {
        public bool walkable = true;
        public Vector2 loc, grid;
        public int g, h;
        public Node parent;
        public int f { get{return g+h; } }

        public Node(bool w, Vector2 l, Vector2 grid) {
            this.grid = grid;
            walkable = w;
            loc = l;
        }
    }
}
