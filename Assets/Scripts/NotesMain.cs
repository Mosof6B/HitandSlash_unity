using UnityEngine;

public class NotesMain : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;
    float approachTime;
    float startTime;
    bool isActive;

    float expectedJudgeTime;

    // ノーツの移動開始時に初期化する
    public void Initialize(Vector2 start, Vector2 end, float approach, float judgeTime) {
        startPos = start;
        endPos = end;
        approachTime = approach;
        expectedJudgeTime = judgeTime;
        startTime = Time.time;
        isActive = true;

        transform.position = startPos;
    }

    // ノーツを強制的に非アクティブ化し、プールに戻す
    public void ForceDeactivate() {
        isActive = false;
        gameObject.SetActive(false);
    }

    // 毎フレーム移動処理を行い、到達後に非アクティブ化する
    void Update(){
        if(!isActive) return;

        float t = (Time.time - startTime) / approachTime;
        transform.position = Vector2.Lerp(startPos, endPos, t);

        if(t >= 1f) {
            float arrivalTime = Time.time;
            // Debug.Log($"ノーツが到達しました: 到達した時間={arrivalTime}, 予定された時間={expectedJudgeTime}, 差分={arrivalTime - expectedJudgeTime}");
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}
