using UnityEngine;

public class ReelController : MonoBehaviour
{
    [SerializeField] private ReelSimpleLoop[] reels;

    private int stoppedCount = 0;

    private void Start()
    {
        // 🔥 THIS WAS MISSING
        foreach (var reel in reels)
        {
            reel.OnReelStopped += OnSingleReelStopped;
        }
    }

    public void StartAll()
    {
        stoppedCount = 0;

        foreach (var reel in reels)
        {
            reel.StartSpin();
        }
    }

    public void StopAllSequential()
    {
        StartCoroutine(StopRoutine());
    }

    private System.Collections.IEnumerator StopRoutine()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StopSpin();
            yield return new WaitForSeconds(0.5f);
        }
    }

    // 🔥 Called when each reel stops
    private void OnSingleReelStopped()
    {
        stoppedCount++;

        if (stoppedCount >= reels.Length)
        {
            stoppedCount = 0;
            CheckResult(); // ✅ NOW it runs automatically
        }
    }

    public void CheckResult()
{
    SlotSymbol a = reels[0].GetCurrentSymbol();
    SlotSymbol b = reels[1].GetCurrentSymbol();
    SlotSymbol c = reels[2].GetCurrentSymbol();

    Debug.Log($"Reel1: {a}, Reel2: {b}, Reel3: {c}");

    int points = 0;

    // 🎯 JACKPOT (7,7,7)
    if (a == SlotSymbol.Seven && b == SlotSymbol.Seven && c == SlotSymbol.Seven)
    {
        points = 1000;
        Debug.Log("🔥 JACKPOT! +1000");
    }
    // 🎯 All 3 same (but not 7)
    else if (a == b && b == c)
    {
        points = 100;
        Debug.Log("✨ Triple Match! +100");
    }
    // 🎯 Any 2 match
    else if (a == b || b == c || a == c)
    {
        points = 50;
        Debug.Log("👍 Two Match! +50");
    }
    // ❌ No match
    else
    {
        points = 0;
        Debug.Log("❌ No Match +0");
    }

    Debug.Log("Total Points: " + points);
}
}