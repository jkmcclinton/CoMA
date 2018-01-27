using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * 視差のあるレベル間で1つの背写真を移動する
 * move single background image across level with parallax
 * Author: 雪果樹ら
 * */

public class LD_Parallax : MonoBehaviour {
    
    private Vector3 レベル;
    private Vector2 レン;
    private Camera カム;
    private SpriteRenderer 写真;

    //静的データを設定します
    void Start() {
        カム = GameObject.Find("Main Camera").GetComponent<Camera>();
        Transform 左 = transform.FindChild("左");
        Transform 右 = transform.FindChild("右");
        Transform 上 = transform.FindChild("上"); 
        Transform 下 = transform.FindChild("下"); 

        GameObject main = GameObject.Find("メイン");
        if (main == null) return;

        写真 = main.GetComponent<SpriteRenderer>();
        レン = new Vector2(右.position.x - 左.position.x,
            上.position.y - 下.position.y);
        レベル =100* new Vector2(左.position.x + レン.x / 2,
             下.position.y + レン.y / 2);
        レン *= 100;
    }

    //写真はプレーヤーに移動します
    void Update() {
        Vector2 シヂ = カム.pixelRect.size;
        Vector2 シ = 100 * カム.transform.position - (レベル - (Vector3)(レン / 2));
        Vector2 ビヂ = 写真.sprite.rect.size;
        Vector2 オッフ = -((シ - レン / 2).div(レン)).mul(ビヂ - シヂ);
        写真.transform.position = ((Vector2)(100 * カム.transform.position) + オッフ) /100;
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