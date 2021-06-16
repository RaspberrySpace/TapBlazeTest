using UnityEngine;

[CreateAssetMenu(fileName = "PrizePool", menuName = "PrizeWheel/PrizePool")]
public class PrizePool : ScriptableObject
{
    public PrizePoolObject[] prizes;

    public int Count => prizes.Length;

    public Prize[] GetPrizes()
    {
        Prize[] p = new Prize[prizes.Length];
        for(int i = 0; i < prizes.Length; i++)
        {
            p[i] = prizes[i].prize;
        }
        return p;
    }

    public PrizePoolDraw Draw()
    {
        int prizeIndex = 0;

        float totalProbability = GetTotalProbability();
        float randomDraw = Random.Range(0.0f, totalProbability);
        while(randomDraw > prizes[prizeIndex].probability && prizeIndex < prizes.Length - 1)
        {
            randomDraw -= prizes[prizeIndex].probability;
            prizeIndex++;
        }

        return new PrizePoolDraw
        {
            index = prizeIndex,
            prize = prizes[prizeIndex].prize
        };
    }

    public float GetTotalProbability()
    {
        float total = 0.0f;
        for(int i = 0; i < prizes.Length; i++)
        {
            total += prizes[i].probability;
        }
        return total;
    }
    
}

[System.Serializable]
public class PrizePoolObject
{
    public Prize prize;
    public float probability;
}

public struct PrizePoolDraw
{
    public int index;
    public Prize prize;
}
