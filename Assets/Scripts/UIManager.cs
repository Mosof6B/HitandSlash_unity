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

    void SetUpSlot_UI() {
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            Button b = transform.GetChild(0).GetChild(i).GetComponent<Button>();
            if (b == null) continue;

            int index = i;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => OnButtonClick_SetSlot(index));
        }

        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnButtonClick_CharChange());
    }

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

    void OnButtonClick_CharChange() {
        isHitFlg = !isHitFlg;

        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (isHitFlg) ? "ヒット" : "スラッシュ";
    }
}
