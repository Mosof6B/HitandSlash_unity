using UnityEngine;

// 攻撃のひな形クラス。個別の攻撃はこれを継承して実装する
public abstract class AttackBase : MonoBehaviour
{
    [SerializeField] protected string attackName = "(attack)";
    [SerializeField] protected float power = 1f;
    [SerializeField] protected float cooldown = 0f;

    float lastExecutedTime = -Mathf.Infinity;

    public string AttackName => attackName;
    public float Power => power;
    public float Cooldown => cooldown;

    // 実行可能か（クールダウン中でないか）
    public bool IsReady => Time.time - lastExecutedTime >= cooldown;

    // 外部からはこれを呼ぶ。クールダウン判定を共通化し、本体は派生クラスに委ねる
    public bool TryExecute() {
        if (!IsReady) return false;
        lastExecutedTime = Time.time;
        OnExecute();
        return true;
    }

    // 派生クラスで個別攻撃の挙動を実装する
    protected abstract void OnExecute();
}
