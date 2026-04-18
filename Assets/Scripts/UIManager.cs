using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("ゲームマネージャ")]
    [SerializeField] GameManager gameManager;

    bool isHitFlg = true;

    private void Start() {
        SetUpSlot_UI();
    }

    // スロットUIのセットアップ
    void SetUpSlot_UI() {
        // スロットUIのボタンにクリックイベントを設定
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            Button b = transform.GetChild(0).GetChild(i).GetComponent<Button>();
            if (b == null) continue;

            int index = i;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => OnButtonClick_SetSlot(index));
        }
    // キャラクター切り替えボタンにクリックイベントを設定
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnButtonClick_CharChange());
    }

    // スロットUIのボタンがクリックされたときの処理
    void OnButtonClick_SetSlot(int setNum) {
        // CharacterManagerのsetSlotを呼び出す
        switch (isHitFlg) {
            case true: // ヒット
                gameManager.manager_Hit.setSlot(setNum);
                break;
            case false: // スラッシュ
                gameManager.manager_Slash.setSlot(setNum);
                break;
        }

        Debug.Log($"ヒット：{string.Join(", ", gameManager.manager_Hit.slot)}");
        Debug.Log($"スラッシュ：{string.Join(", ", gameManager.manager_Slash.slot)}");
    }
    
    // キャラクター切り替えボタンがクリックされたときの処理
    void OnButtonClick_CharChange() {
        isHitFlg = !isHitFlg;

        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (isHitFlg) ? "ヒット" : "スラッシュ";
    }

    public void NotActicve_SlotUI() {
        gameObject.SetActive(false);
    }
}
