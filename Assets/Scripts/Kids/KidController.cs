using System;
using System.Collections.Generic;
using UnityEngine;

public class KidController : MonoBehaviour
{
    [SerializeField] private MoodTimer moodTimer;

    private Animator animator;
    private const String IS_SAD = "IsSad";
    private const String IS_CRASHINGOUT = "IsCrashingOut";
    private const String IS_HAPPY = "IsHappy";

    private float _baseMoodDuration;
    private float _baseCooldownDuration;

    private float _currentMoodTimer;
    private float _currentCooldownTimer;

    public event Action<KidController> OnCooldownTimerFinished;

    private bool _isInCooldown = false;
    private bool _isFirstInit = false;

    private bool isSad = false;
    private bool isCrashingOut = false;
    private bool isHappy = false;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameStates.IsGameFinish || GameStates.IsPaused) return;

        animator.SetBool(IS_SAD, isSad);
        animator.SetBool(IS_CRASHINGOUT, isCrashingOut);
        animator.SetBool(IS_HAPPY, isHappy);

        if (_isInCooldown)
        {
            _currentCooldownTimer += Time.deltaTime;
            if (_currentCooldownTimer <= _baseCooldownDuration) return;

            _currentCooldownTimer = 0;
            _isInCooldown = false;
            moodTimer.SetVisiblity(true);
            OnCooldownTimerFinished?.Invoke(this);
        }
        else
        {
            _currentMoodTimer -= Time.deltaTime * AlarmManager.MoodTimerSpeedMultiplier;
            moodTimer.UpdateTimer(_currentMoodTimer);

            int displayedTime = Mathf.Max(0, Mathf.CeilToInt(_currentMoodTimer));
            ChangeMoodState(displayedTime, moodTimer.BaseMoodTimer);

            if (_currentMoodTimer > 0) return;

            _currentMoodTimer = _baseMoodDuration;
            TriggerCooldown();
            moodTimer.InvokeOnTimerFinished();
        }

    }

    public void SetupData(float moodDuration, float cooldownDuration)
    {
        _isFirstInit = true;
        _baseMoodDuration = moodDuration;
        _currentMoodTimer = _baseMoodDuration;

        _baseCooldownDuration = cooldownDuration;

        moodTimer.Init(moodDuration);
        moodTimer.OnTimerFinished += CrashoutAndRemove;

        if(!_isFirstInit)
        {
            _isFirstInit = true;
            TriggerCooldown();
        }
    }

    public void TriggerCooldown()
    {
        _isInCooldown = true;
        moodTimer.SetVisiblity(false);
        isHappy = true;
        isSad = false;
    }

    public MoodTimer GetMoodTimer()
    {
        return moodTimer;
    }

    public bool IsInCooldown()
    {
        return _isInCooldown;
    }

    public void CrashoutAndRemove()
    {
        Debug.Log("Crashout");
        SfxManager.Instance.Play(SfxId.KidCrashOut);
        moodTimer.CleanUp();

        Destroy(gameObject, 2.0f);
    }

    public void ChangeMoodState(float moodValue, float baseMoodValue)
    {
        float moodRatio = moodValue / baseMoodValue;

        if (moodRatio >= 0.35f)
        {
            isSad = false;
            isCrashingOut = false;
            isHappy = true;
        }
        else if (moodRatio > 0 && moodRatio < 0.35f)
        {
            isSad = true;
            isCrashingOut = false;
            isHappy = false;
        } 
        else if (moodRatio <= 0)
        {
            isSad = false;
            isCrashingOut = true;
            isHappy = false;
        }
        
    }
}
