using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    #region  Serialized Fields 

    [Header("Reels")]
    [SerializeField] private ReelSimpleLoop[] reels;
    [SerializeField] private SlotSoundManager soundManager;

    [Header("UI")]
    [SerializeField] private GameObject backImage;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text pointsText;

    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private float blinkDuration = 2f;

    [Header("Handle")]
    [SerializeField] private Image handleImage;
    [SerializeField] private Sprite handleDefault;
    [SerializeField] private Sprite handleTouched;
    [SerializeField] private float handleTouchDuration = 1f;

    #endregion

    #region  Private Fields 

    private Coroutine handleRoutine;
    private Coroutine blinkRoutine;

    private int stoppedCount;
    private int totalPoints;

    private SlotState currentState = SlotState.Idle;

    #endregion

    #region  Enums 

    private enum SlotState
    {
        Idle,
        Spinning,
        Stopping
    }

    #endregion

    #region  Unity Lifecycle 

    private void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        // Prevent memory leaks
        foreach (var reel in reels)
        {
            if (reel != null)
                reel.OnReelStopped -= OnSingleReelStopped;
        }
    }

    #endregion

    #region  Initialization 

    private void Initialize()
    {
        foreach (var reel in reels)
        {
            if (reel != null)
                reel.OnReelStopped += OnSingleReelStopped;
        }

        if (pointsText != null)
            pointsText.text = "Points : 00";

        if (rewardText != null)
            rewardText.text = "";

        if (backImage != null)
            backImage.SetActive(false);

        if (handleImage != null && handleDefault != null)
            handleImage.sprite = handleDefault;
    }

    #endregion

    #region  Callable Functions 

    /// <summary>
    /// Starts spinning all reels
    /// </summary>
    public void StartAll()
    {
        if (currentState != SlotState.Idle)
            return;

        currentState = SlotState.Spinning;
        stoppedCount = 0;

        PlayHandleEffect();
        StopBlinkEffect();
        ResetUI();

        foreach (var reel in reels)
        {
            reel?.StartSpin();
        }
        soundManager?.PlayHandle();
        soundManager?.StartReelLoop();
    }

    /// <summary>
    /// Stops reels one by one
    /// </summary>
    public void StopAllSequential()
    {
        if (currentState != SlotState.Spinning)
            return;
        soundManager.PlayStopSound();

        currentState = SlotState.Stopping;
        StartCoroutine(StopRoutine());
    }

    #endregion

    #region  Core Logic 

    private IEnumerator StopRoutine()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i]?.StopSpin();
          
            yield return new WaitForSeconds(0.5f);
            
        }
    }

    private void OnSingleReelStopped()
    {
        stoppedCount++;
        soundManager?.PlayReelStop();
        

        if (stoppedCount < reels.Length)
            return;

        stoppedCount = 0;
        soundManager?.StopReelLoop();

        EvaluateResult();

        currentState = SlotState.Idle;
    }

    private void EvaluateResult()
    {
        SlotSymbol a = reels[0].GetCurrentSymbol();
        SlotSymbol b = reels[1].GetCurrentSymbol();
        SlotSymbol c = reels[2].GetCurrentSymbol();

        int points;
        string message;

        if (a == SlotSymbol.Seven && b == SlotSymbol.Seven && c == SlotSymbol.Seven)
        {
            points = 1000;
            message = "JACKPOT!";
            soundManager?.PlayJackpot();
        }
        else if (a == b && b == c)
        {
            points = 100;
            message = "Triple Match!";
        }
        else if (a == b || b == c || a == c)
        {
            points = 50;
            message = "Two Match!";
        }
        else
        {
            points = 0;
            message = "No Match";
        }

        ApplyResult(points, message);
    }

    private void ApplyResult(int points, string message)
    {
        totalPoints += points;

        if (rewardText != null)
            rewardText.text = message;

        if (pointsText != null)
            pointsText.text = $"Points : {totalPoints:00}";

        PlayBlinkEffect();
    }

    #endregion

    #region  Effects 

    private void PlayBlinkEffect()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkEffect());
    }

    private void StopBlinkEffect()
    {
        if (blinkRoutine == null) return;

        StopCoroutine(blinkRoutine);
        blinkRoutine = null;

        if (backImage != null)
            backImage.SetActive(false);
    }

    private IEnumerator BlinkEffect()
    {
        float timer = 0f;

        while (timer < blinkDuration)
        {
            if (backImage != null)
                backImage.SetActive(!backImage.activeSelf);

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (backImage != null)
            backImage.SetActive(true);
    }

    private void PlayHandleEffect()
    {
        if (handleRoutine != null)
            StopCoroutine(handleRoutine);

        handleRoutine = StartCoroutine(HandleTouchEffect());
    }

    private IEnumerator HandleTouchEffect()
    {
        if (handleImage != null && handleTouched != null)
            handleImage.sprite = handleTouched;

        yield return new WaitForSeconds(handleTouchDuration);

        if (handleImage != null && handleDefault != null)
            handleImage.sprite = handleDefault;

        handleRoutine = null;
    }

    #endregion

    #region  UI Helpers 

    private void ResetUI()
    {
        if (rewardText != null)
            rewardText.text = "";

        if (backImage != null)
            backImage.SetActive(false);
    }

    #endregion
}