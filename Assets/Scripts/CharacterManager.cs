using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// キャラクターマネージャー
public class CharacterManager {
    public CharData charData = new CharData();
    // スロット
    public SortedDictionary<int, int> slot { get; private set; } = new SortedDictionary<int, int>();
    // スロット上限
    public int currentMaxSlot { get; private set; } = 0;

    public CharacterManager(CharData data) { 
        charData = data;
        currentMaxSlot = charData.initialMaxSlot;
    }

    public void setSlot(int slotNum) {
        if (slotNum > currentMaxSlot) return;   // スロット上限を超えたスロットにセットしようとしたらreturn

        slot[slotNum] = 1;  // 将来的にはここに入れる番号で流れてくるノーツに変化が出るようにしたい(1なら1個のノーツ2なら連続ノーツ3なら長押しノーツみたいな)
    }

    void plusSlot(int plusNum) {
        currentMaxSlot += plusNum;
    }
}
