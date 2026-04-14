using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ヒット")]
    [SerializeField] CharData data_hit;
    [Header("スラッシュ")]
    [SerializeField] CharData data_slash;

    public CharacterManager manager_Hit {  get; private set; }
    public CharacterManager manager_Slash {  get; private set; }

    bool isHitFlg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager_Hit = new CharacterManager(data_hit);
        manager_Slash = new CharacterManager(data_slash);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
