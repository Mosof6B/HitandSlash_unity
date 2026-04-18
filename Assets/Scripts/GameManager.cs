using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("ヒット")]
    [SerializeField] CharData data_hit;
    [Header("スラッシュ")]
    [SerializeField] CharData data_slash;
    [Header("ノーツマネージャー")]
    [SerializeField] NotesManager notesManager;
    [Header("UIマネージャー")]
    [SerializeField] UIManager uiManager;

    public CharacterManager manager_Hit { get; private set; }
    public CharacterManager manager_Slash { get; private set; }

    // ステート駆動用変数
    enum GameState {
        Title,   // タイトル画面
        Menu,    // メニュー選択
        Pause,   // ポーズ
        Playing, // プレイ中
        Result  // 結果表示
    }

    bool isFirstFrame = true; // 最初のフレームかどうかを判定するフラグ
    GameState currentState = GameState.Menu; // 現在のゲーム状態

    float startTime;
    float endTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameManager.Start() called");
        // キャラクターデータの初期化
        if (data_hit == null) Debug.LogError("data_hit is null!");
        if (data_slash == null) Debug.LogError("data_slash is null!");
        
        manager_Hit = new CharacterManager(data_hit);
        manager_Slash = new CharacterManager(data_slash);
        Debug.Log("GameManager.Start() completed");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState) {
            case GameState.Title:
                if(isFirstFrame) {
                    Debug.Log("タイトル画面");
                    isFirstFrame = false;
                }
                break;

            case GameState.Menu:
                if(isFirstFrame) {
                    Debug.Log("メニュー選択");
                    isFirstFrame = false;
                }

                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) {
                    if(manager_Hit.slot.Count == 0 || manager_Slash.slot.Count == 0) {
                        Debug.Log("ヒットまたはスラッシュのノーツが設定されていません。");
                        break;
                    }

                    uiManager.NotActicve_SlotUI();  // スロットUIを非表示にする
                    
                    UpdateState(GameState.Playing);
                }
                break;

            case GameState.Pause:
                if(isFirstFrame) {
                    Debug.Log("ポーズ");
                    isFirstFrame = false;
                }
                break;

            case GameState.Playing:
                if(isFirstFrame) {
                    Debug.Log("プレイ中");
                    isFirstFrame = false;

                    notesManager.StartNotes(manager_Hit.slot, manager_Slash.slot, Mathf.Max(manager_Hit.currentMaxSlot, manager_Slash.currentMaxSlot));
                }

                if (Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame) {
                    //Debug.Log("Zキーが押されました");
                    notesManager.EvaluateInput(NoteType.Hit);
                }

                if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame) {
                    //Debug.Log("Xキーが押されました");
                    notesManager.EvaluateInput(NoteType.Slash);
                }
                break;

            case GameState.Result:
                if(isFirstFrame) {
                    Debug.Log("結果表示");
                    isFirstFrame = false;
                }
                break;
        }
    }

    // 状態遷移処理: 新しいゲーム状態に移行し、次のフレームを初回フレーム扱いにする
    void UpdateState(GameState newState) {
        currentState = newState;
        isFirstFrame = true;
    }
}
