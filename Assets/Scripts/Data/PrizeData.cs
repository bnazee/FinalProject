using UnityEngine;
using System.Collections.Generic;

public enum PrizeID
{
    SpinCostBack,
    PlusOne,
    ReduceScore,
    FreeSpin,
    IncreaseScore,
    Curse,
    AutoClick,
    RandomPoints,
    ROFL,
    LOSE
}

[CreateAssetMenu(fileName = "PrizeData", menuName = "ScriptableObjects/PrizeData", order = 1)]
public class PrizeData : ScriptableObject
{
    [System.Serializable]
    public struct Prize
    {
        public PrizeID ID;
        public int index;
        public string text;
    }

    public List<Prize> prizes;

    public int GetIndex(PrizeID prizeID)
    {
        Prize prize = prizes.Find(x => x.ID == prizeID);

        return prize.index; 
    }
    public PrizeID GetID(int index)
    {
        Prize prize = prizes.Find(x => x.index == index);

        return prize.ID;
    }

    public string GetText(PrizeID prizeID)
    {
        Prize prize = prizes.Find(x => x.ID == prizeID);

        return prize.text;
    }
}
