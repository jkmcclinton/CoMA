using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LD_Unit : MonoBehaviour {
    public Color color = Color.blue;
   public void OnDrawGizmosSelected() {
        Transform parent = transform.parent;
        Transform 左 = parent.Find("左");
        Transform 右 = parent.Find("右");
        Transform 上 = parent.Find("上");
        Transform 下 = parent.Find("下");
        Vector2 start = gameObject.transform.position;
        Vector2 end = gameObject.transform.position;

        switch (gameObject.name) {
            case "右":
                start = new Vector2(右.position.x, 下.position.y);
                end = new Vector2(右.position.x, 上.position.y);
                break;
            case "左":
                start = new Vector2(左.position.x, 下.position.y);
                end = new Vector2(左.position.x, 上.position.y);
                break;
            case "上":
                start = new Vector2(左.position.x, 上.position.y);
                end = new Vector2(右.position.x, 上.position.y);
                break;
            case "下":
                start = new Vector2(左.position.x, 下.position.y);
                end = new Vector2(右.position.x, 下.position.y);
                break;
        }

        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }
}
