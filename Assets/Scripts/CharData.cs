using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "CharData", menuName = "Scriptable Objects/CharData")]
public class CharData : ScriptableObject
{
    public string charName = "(name)";
    public float strength = 0;
    public float sense = 0;
    public int initialMaxSlot = 10;
    // 持ってるパッシブスキルとかもここでセットするようにするべき？
}
