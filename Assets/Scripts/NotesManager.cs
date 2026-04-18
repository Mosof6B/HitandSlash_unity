using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum NoteType {
    Hit,
    Slash
}

public class NotesManager : MonoBehaviour
{
    [Header("ノーツオブジェクト(プレハブ)")]
    [SerializeField] GameObject notes_objecct = null;
    [Header("全ノーツが流れるのにかかる時間")]
    public float maxTime_allNotes { get; private set; } = 10f;
    [Header("打鍵時の許容誤差")]
    public float toleranceTime { get; private set; } = 1f;
    public float approachTime { get; private set; } = 2f;    // ノーツがスポーンしてから判定位置に到達するまでの時間

    Vector2 spawnPos_Hit = new Vector2(-2, 5);
    Vector2 spawnPos_Slash = new Vector2(2, 5);
    Vector2 perfectPos_Hit = new Vector2(-2, -2);
    Vector2 perfectPos_Slash = new Vector2(2, -2);

    // ノーツスケジュール: 時刻とノーツの種類を管理する
    List<float> notesSchedule_Hit = new List<float>();
    List<float> notesSchedule_Slash = new List<float>();

    int currentNoteIndex_Hit = 0;
    int currentNoteIndex_Slash = 0;

    bool isPlaying = false; // ノーツが流れているかどうかを管理するフラグ

    // 初期化処理: ノーツプールを作成する
    void Start(){
        SetNotes();
    }

    void Update() {
        if(!isPlaying) return;  // ノーツが流れていない場合は何もしない

        float currentTime = Time.time;

        // リストが空でないかチェック
        if(notesSchedule_Hit.Count > 0 && notesSchedule_Slash.Count > 0) {
            if(currentTime >= notesSchedule_Hit[notesSchedule_Hit.Count - 1] && currentTime >= notesSchedule_Slash[notesSchedule_Slash.Count - 1]) {
                Debug.Log("全ノーツが流れ終わりました");
                isPlaying = false;
            }
        }

        // Hitノーツのスケジュールを確認し、必要に応じてノーツを発射する
        if(notesSchedule_Hit.Count > 0 && currentNoteIndex_Hit < notesSchedule_Hit.Count) {
            if(currentTime >= notesSchedule_Hit[currentNoteIndex_Hit] - approachTime) {
                NotesMain note = SelectNotes();
                if(note != null) {
                    note.Initialize(spawnPos_Hit, perfectPos_Hit, approachTime, notesSchedule_Hit[currentNoteIndex_Hit]);
                    // Debug.Log($"Hitノーツ発射: scheduled={notesSchedule_Hit[currentNoteIndex_Hit]}, current={currentTime}");
                }
                currentNoteIndex_Hit++;
            }
        }

        // Slashノーツのスケジュールを確認し、必要に応じてノーツを発射する
        if(notesSchedule_Slash.Count > 0 && currentNoteIndex_Slash < notesSchedule_Slash.Count) {
            if(currentTime >= notesSchedule_Slash[currentNoteIndex_Slash] - approachTime) {
                NotesMain note = SelectNotes();
                if(note != null) {
                    note.Initialize(spawnPos_Slash, perfectPos_Slash, approachTime, notesSchedule_Slash[currentNoteIndex_Slash]);
                    // Debug.Log($"Slashノーツ発射: scheduled={notesSchedule_Slash[currentNoteIndex_Slash]}, current={currentTime}");
                }
                currentNoteIndex_Slash++;
            }
        }
    }

    // ノーツ発射
    public void StartNotes(SortedDictionary<int, int> slot_hit, SortedDictionary<int, int> slot_slash, int maxSlot) {
        currentNoteIndex_Hit = 0;
        currentNoteIndex_Slash = 0;
        SetNotesPerfectSchedule(slot_hit, slot_slash, Time.time, maxSlot);
        isPlaying = true;
    }

    // ノーツスケジュール算出
    void SetNotesPerfectSchedule(SortedDictionary<int, int> slot_hit, SortedDictionary<int, int> slot_slash, float startTime, int maxSlot) {
        notesSchedule_Hit.Clear();
        notesSchedule_Slash.Clear();

        foreach(var kvp in slot_hit) {
            if(kvp.Value == 1) {  // 値が1の場合のみ
                // ノーツの判定時刻を算出する。
                // 最初のノーツは startTime + approachTime
                // 最後のノーツは startTime + maxTime_allNotes
                float judgeTime = startTime + approachTime +
                                  (maxTime_allNotes - approachTime) * kvp.Key / Mathf.Max(maxSlot - 1, 1);
                notesSchedule_Hit.Add(judgeTime);
            }
        }

        foreach(var kvp in slot_slash) {
            if(kvp.Value == 1) {  // 値が1の場合のみ
                float judgeTime = startTime + approachTime +
                                  (maxTime_allNotes - approachTime) * kvp.Key / Mathf.Max(maxSlot - 1, 1);
                notesSchedule_Slash.Add(judgeTime);
            }
        }
    }

    // プール内から未使用のノーツを探し、アクティブ化して返す
    NotesMain SelectNotes() {
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (!child.gameObject.activeSelf) {
                child.gameObject.SetActive(true);
                NotesMain note = child.GetComponent<NotesMain>();
                if (note == null) {
                    Debug.LogError($"Child {i} does not have NotesMain component!");
                    return null;
                }
                // Debug.Log($"Reused note from pool at index {i}");
                return note;
            }
        }

        // そもそもPrehubが設定されていない場合、nullを返す
        if (notes_objecct == null) {
            Debug.LogError("notes_objecct prefab is null!");
            return null;
        }

        GameObject newNote = Instantiate(notes_objecct, Vector2.zero, Quaternion.identity, transform);
        NotesMain notesMain = newNote.GetComponent<NotesMain>();
        if (notesMain == null) {
            Debug.LogError("生成したノーツにNotesMainコンポーネントがアタッチされてない");
            return null;
        }
        return notesMain;
    }

    // オブジェクトプールを作成してノーツをあらかじめ配置する
    void SetNotes() {
        for(int i = 0; i < 20; i++) {
            Instantiate(notes_objecct,
                        Vector2.zero,
                        Quaternion.identity,
                        transform);
        }
    }

    // 打鍵の評価
    public void EvaluateInput(NoteType noteType) {
        float currentTime = Time.time;

        List<float> schedule = (noteType == NoteType.Hit) ? notesSchedule_Hit : notesSchedule_Slash;
        int currentIndex = (noteType == NoteType.Hit) ? currentNoteIndex_Hit : currentNoteIndex_Slash;

        // 現在のノーツスケジュールから、最も近いノーツを探す
        float closestTime = float.MaxValue;
        int closestIndex = -1;

        // Debug.Log("スケジュールの確認: " + string.Join(", ", schedule) + ", currentTime: " + currentTime);

        for(int i = 0; i < currentIndex; i++) {  // 発射済みのノーツのみチェック
            float timeDiff = Mathf.Abs(schedule[i] - currentTime);
            // Debug.Log("ノーツスケジュール: " + schedule[i] + ", 現在時刻: " + currentTime + ", 差分: " + timeDiff);
            if(timeDiff < closestTime) {
                closestTime = timeDiff;
                closestIndex = i;
            } else {
                // スケジュールは時間順に並んでいるので、これ以上先はさらに遠くなる
                break;
            }
        }

        if(closestIndex != -1 && closestTime <= toleranceTime) {
            Debug.Log($"Perfect! NoteType: {noteType}, TimeDiff: {closestTime}");
            // ノーツをヒットさせる処理をここに追加する（例: スコア加算、エフェクト表示など）

            // ヒットしたノーツをスケジュールから削除する
            schedule.RemoveAt(closestIndex);
            // インデックスを調整（削除したノーツより後のインデックスを1つ減らす）
            if (noteType == NoteType.Hit) {
                currentNoteIndex_Hit--;
            } else {
                currentNoteIndex_Slash--;
            }
        } else {
            Debug.Log($"Miss! NoteType: {noteType}, ClosestTimeDiff: {closestTime}");
            // ミスの処理をここに追加する（例: ライフ減少、ミスエフェクト表示など）
        }
    }
}
