using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 視差のあるレベル間で1つの背写真を移動する
 * move single background image across level with parallax
 * Author: 雪果樹ら
 * */

public class LD_Parallax : MonoBehaviour {
    
    private Vector3 levelSize;
    public Vector2 length;
	public Vector2 origin;

    //静的データを設定します
    void Start() {
<<<<<<< HEAD
        カム = GameObject.Find("Main Camera").GetComponent<Camera>();
        Transform 左 = transform.Find("左");
        Transform 右 = transform.Find("右");
        Transform 上 = transform.Find("上"); 
        Transform 下 = transform.Find("下"); 
=======
        Transform 左 = transform.Find("left");
        Transform 右 = transform.Find("right");
        Transform 上 = transform.Find("down"); 
        Transform 下 = transform.Find("up"); 
>>>>>>> origin/master

        length = new Vector2(右.position.x - 左.position.x,
            上.position.y - 下.position.y);
		levelSize =100* new Vector2(左.position.x + length.x / 2,
             下.position.y + length.y / 2);
		length *= 100;
		origin = new Vector2 (左.position.x + length.x / 2, 左.position.y - length.y / 2);
		print ("Level Size: " + levelSize);
    }

    //写真はプレーヤーに移動します
    void Update() {
    }
}

//ベクターのヘルパークラス
public static class Helper {
    public static Vector2 div(this Vector2 v1, Vector2 v2) {
        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }

    public static Vector2 mul(this Vector2 v1, Vector2 v2) {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }
}