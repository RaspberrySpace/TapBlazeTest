using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrizePool))]
public class PrizePoolEditor : Editor
{
    private PrizePool pool;
    private int testDraws = 1000;
    private TestDrawResults testResults = null;

    void OnEnable()
    {
        pool = (PrizePool)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawPrizePoolTest();
    }

    private void DrawPrizePoolTest()
    {
        testDraws = EditorGUILayout.IntField(new GUIContent("Test Draw Count", "The number of times to draw from the pool"), testDraws);
        if (GUILayout.Button("Run Test Draws"))
        {
            RunTest();
            LogTest();
        }
    }

    private void RunTest()
    {
        testResults = new TestDrawResults
        {
            prizeDrawCounts = new Dictionary<Prize, int>(),
            totalDrawCount = testDraws
        };
        for(int i = 0; i < testDraws; i++)
        {
            var draw = pool.Draw();
            if (!testResults.prizeDrawCounts.ContainsKey(draw.prize))
            {
                testResults.prizeDrawCounts.Add(draw.prize, 0);
            }
            testResults.prizeDrawCounts[draw.prize]++;
        }
    }

    private void LogTest()
    {
        float totalProbability = pool.GetTotalProbability();
        Debug.Log($"Prize Pool {pool.name} Draw Test with {testResults.totalDrawCount} draws");
        foreach (var result in testResults.prizeDrawCounts)
        {
            Debug.Log($"{result.Key.id}_{result.Key.count} was drawn {result.Value} times");
        }
    }

    private class TestDrawResults
    {
        public Dictionary<Prize, int> prizeDrawCounts;
        public int totalDrawCount;
    }
}
