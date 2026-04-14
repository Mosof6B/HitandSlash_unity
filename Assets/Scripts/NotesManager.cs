using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [Header("ノーツオブジェクト(プレハブ)")]
    [SerializeField] GameObject notes_objecct = null;
    [Header("全ノーツが流れるのにかかる時間")]
    [SerializeField] float maxTime_allNotes = 10f;
    [Header("打鍵時の許容誤差")]
    [SerializeField] float toleranceTime = 0.5f;

    List<float> hitTimeList = new List<float>();
    float approachTime = 2f;

    Vector2 spawnPos = Vector2.zero;
    Vector2 perfectPos = Vector2.down * 2;

    private void Update() {
        
    }

    /*
     * 作るべきもの
     * 
     * ノーツマネージャー
     * ・オブジェクトプールでノーツ保持
     * ・スロット取得(ヒットとスラッシュそれぞれのクラスから)
     * ・キュー作成
     * 
     */

    void moveNotes() {
        foreach(Transform child in transform) {
            // プールの中から使用可能なノーツを検索
            if (!child.gameObject.activeSelf) {
                // 移動処理

                return;
            }
        }
    }

    void SetNotes() {
        for(int i = 0; i < 20; i++) {
            Instantiate(notes_objecct,
                        Vector2.zero,
                        Quaternion.identity);
        }
    }
}
