using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Stops a rotating UI hand inside a radial target slice.
/// One successful stop completes the mini-game; misses retry with a new target.
/// </summary>
public class CalmingMusicGameMaster : MonoBehaviour, IMiniGameMaster
{
    [Header("Required UI")]
    [Tooltip("Root containing the mini-game UI. Keep this controller outside that root.")]
    [SerializeField] private GameObject miniGameRoot;

    [FormerlySerializedAs("spinningHand")]
    [Tooltip("The replaceable hand graphic. Its artwork must point up from the bottom centre.")]
    [SerializeField] private Image handImage;

    [Tooltip("The complete circle shown behind the target. Replace this Source Image to change the wheel art.")]
    [SerializeField] private Image wheelImage;

    [FormerlySerializedAs("greenZone")]
    [Tooltip("The green radial overlay. It automatically reuses the Wheel Image sprite.")]
    [SerializeField] private Image targetZoneImage;

    [Header("Optional")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private AudioSource calmingMusicSource;

    [Header("Timing")]
    [SerializeField, Range(10f, 180f)] private float minTargetZoneDegrees = 30f;
    [SerializeField, Range(10f, 180f)] private float maxTargetZoneDegrees = 60f;
    [SerializeField, Min(1f)] private float handSpeedDegreesPerSecond = 150f;
    [SerializeField, Min(0f)] private float feedbackDuration = 0.7f;
    [SerializeField] private bool rotateClockwise = true;

    public event Action OnMiniGameCompleted;

    private const float InputGracePeriod = 0.15f;

    private bool isActive;
    private bool isSpinning;
    private float handAngle;
    private float targetCentreAngle;
    private float inputEnabledAt;
    
    // Store the randomly chosen size for the current attempt
    private float currentTargetZoneDegrees;

    private void Awake()
    {
        ConfigureImages();
        SetMiniGameVisible(false);
    }

    private void Update()
    {
        if (!isActive || !isSpinning)
        {
            return;
        }

        RotateHand();

        if (Time.unscaledTime >= inputEnabledAt && WasStopPressed())
        {
            StopHand();
        }
    }

    public void GenerateMiniGame()
    {
        if (!HasRequiredReferences())
        {
            Debug.LogError(
                "Calming Music requires Mini Game Root, Hand Image, Wheel Image, and Target Zone Image.",
                this);
            return;
        }

        StopAllCoroutines();
        ConfigureImages();

        isActive = true;
        SetMiniGameVisible(true);
        PlayMusic();
        MoveTargetZone();
        BeginAttempt();
    }

    public void CleanUpMiniGame()
    {
        StopAllCoroutines();
        isActive = false;
        isSpinning = false;
        StopMusic();
        SetMiniGameVisible(false);
    }

    public void StopHand()
    {
        if (!isActive || !isSpinning)
        {
            return;
        }

        isSpinning = false;

        if (IsInsideTarget(handAngle))
        {
            SetStatus("Perfect timing!");
            StartCoroutine(CompleteAfterFeedback());
        }
        else
        {
            SetStatus("Missed! Try again.");
            StartCoroutine(RetryAfterFeedback());
        }
    }

    private void RotateHand()
    {
        float direction = rotateClockwise ? 1f : -1f;
        handAngle = Mathf.Repeat(
            handAngle + direction * handSpeedDegreesPerSecond * Time.unscaledDeltaTime,
            360f);
        ApplyHandAngle();
    }

    private IEnumerator RetryAfterFeedback()
    {
        yield return new WaitForSecondsRealtime(feedbackDuration);
        MoveTargetZone();
        BeginAttempt();
    }

    private IEnumerator CompleteAfterFeedback()
    {
        yield return new WaitForSecondsRealtime(feedbackDuration);

        isActive = false;
        isSpinning = false;
        StopMusic();
        SetMiniGameVisible(false);
        OnMiniGameCompleted?.Invoke();
    }

    private void BeginAttempt()
    {
        handAngle = GetStartingAngleOutsideTarget();
        ApplyHandAngle();
        SetStatus("Stop the hand inside the green slice");

        inputEnabledAt = Time.unscaledTime + InputGracePeriod;
        isSpinning = true;
    }

    private void MoveTargetZone()
    {
        // Pick a random size for the target zone between your specified minimum and maximum
        currentTargetZoneDegrees = UnityEngine.Random.Range(minTargetZoneDegrees, maxTargetZoneDegrees);
        targetCentreAngle = UnityEngine.Random.Range(0f, 360f);

        float startAngle = targetCentreAngle - currentTargetZoneDegrees * 0.5f;
        targetZoneImage.fillAmount = currentTargetZoneDegrees / 360f;
        targetZoneImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, -startAngle);
    }

    private float GetStartingAngleOutsideTarget()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);

        for (int i = 0; i < 12 && IsInsideTarget(angle); i++)
        {
            angle = UnityEngine.Random.Range(0f, 360f);
        }

        return angle;
    }

    private bool IsInsideTarget(float angle)
    {
        float distance = Mathf.Abs(Mathf.DeltaAngle(angle, targetCentreAngle));
        return distance <= currentTargetZoneDegrees * 0.5f;
    }

    private void ApplyHandAngle()
    {
        handImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, -handAngle);
    }

    private void ConfigureImages()
    {
        ConfigureHandImage();
        ConfigureWheelImages();
    }

    private void ConfigureHandImage()
    {
        if (handImage == null)
        {
            return;
        }

        RectTransform hand = handImage.rectTransform;
        hand.anchorMin = new Vector2(0.5f, 0.5f);
        hand.anchorMax = new Vector2(0.5f, 0.5f);
        hand.pivot = new Vector2(0.5f, 0f);
        hand.anchoredPosition = Vector2.zero;
        hand.sizeDelta = new Vector2(
            Mathf.Max(1f, Mathf.Abs(hand.sizeDelta.x)),
            Mathf.Max(1f, Mathf.Abs(hand.sizeDelta.y)));

        handImage.type = Image.Type.Simple;
        handImage.preserveAspect = true;
        handImage.raycastTarget = false;
    }

    private void ConfigureWheelImages()
    {
        if (wheelImage != null)
        {
            wheelImage.type = Image.Type.Simple;
            wheelImage.preserveAspect = true;
            wheelImage.raycastTarget = false;
        }

        if (targetZoneImage == null)
        {
            return;
        }

        if (wheelImage != null)
        {
            targetZoneImage.sprite = wheelImage.sprite;
        }

        targetZoneImage.type = Image.Type.Filled;
        targetZoneImage.fillMethod = Image.FillMethod.Radial360;
        targetZoneImage.fillOrigin = (int)Image.Origin360.Top;
        targetZoneImage.fillClockwise = true;
        targetZoneImage.raycastTarget = false;
    }

    private bool HasRequiredReferences()
    {
        return miniGameRoot != null &&
               handImage != null &&
               handImage.sprite != null &&
               wheelImage != null &&
               wheelImage.sprite != null &&
               targetZoneImage != null &&
               targetZoneImage.sprite != null;
    }

    private void SetMiniGameVisible(bool isVisible)
    {
        if (miniGameRoot != null && miniGameRoot != gameObject)
        {
            miniGameRoot.SetActive(isVisible);
        }
    }

    private void PlayMusic()
    {
        if (calmingMusicSource == null || calmingMusicSource.clip == null)
        {
            return;
        }

        calmingMusicSource.loop = true;
        calmingMusicSource.Play();
    }

    private void StopMusic()
    {
        if (calmingMusicSource != null)
        {
            calmingMusicSource.Stop();
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private static bool WasStopPressed()
    {
        bool mouse = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        return mouse;
    }

    private void OnValidate()
    {
        minTargetZoneDegrees = Mathf.Clamp(minTargetZoneDegrees, 10f, 180f);
        maxTargetZoneDegrees = Mathf.Clamp(maxTargetZoneDegrees, minTargetZoneDegrees, 180f);
        handSpeedDegreesPerSecond = Mathf.Max(1f, handSpeedDegreesPerSecond);
        ConfigureImages();

        if (miniGameRoot == gameObject)
        {
            Debug.LogWarning(
                "Keep CalmingMusicMiniGame outside Mini Game Root so it can hide the UI.",
                this);
        }
    }
}