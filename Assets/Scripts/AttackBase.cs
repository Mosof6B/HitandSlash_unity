using System;
using UnityEngine;

// 攻撃の種別: ヒット専用 or スラッシュ専用
public enum AttackCategory { Hit, Slash }

// 攻撃のひな形クラス。個別攻撃はこれを継承して実装する
public abstract class AttackBase : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] AttackCategory category;
    [SerializeField] string attackName = "(attack)";
    [SerializeField] float basePower = 1f;

    // 共通エフェクト/SE。Inspectorで直接アセット参照する
    [SerializeField] protected GameObject hitEffectPrefab;
    [SerializeField] protected AudioClip hitSe;

    public int Id => id;
    public AttackCategory Category => category;
    public string AttackName => attackName;
    public float BasePower => basePower;

    // 共通イベント。攻撃固有のイベントは派生クラス側で追加定義する
    public event Action<AttackBase> OnAttackStart;
    public event Action<AttackBase, float> OnDamageDealt;
    public event Action<AttackBase> OnAttackEnd;

    // 攻撃実行のエントリポイント
    public void Execute(CharData attacker) {
        OnAttackStart?.Invoke(this);
        float damage = CalculateDamage(attacker);
        OnExecute(attacker, damage);
        OnDamageDealt?.Invoke(this, damage);
        OnAttackEnd?.Invoke(this);
    }

    // ダメージ計算。Hitは筋力依存、Slashは感性依存。必要なら派生でoverride
    protected virtual float CalculateDamage(CharData attacker) {
        float stat = category == AttackCategory.Hit ? attacker.strength : attacker.sense;
        return basePower * (1f + stat);
    }

    // 派生クラスで個別攻撃の挙動を実装
    protected abstract void OnExecute(CharData attacker, float damage);
}
