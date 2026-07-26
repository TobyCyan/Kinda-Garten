using System;
using System.Collections.Generic;
using UnityEngine;

public class KidController : MonoBehaviour
{
    [SerializeField] private MoodTimer moodTimer;
    [SerializeField] private List<Sprite> kidVariations;

    private float _baseMoodDuration;
    private float _baseCooldownDuration;

    private float _currentMoodTimer;
    private float _currentCooldownTimer;


    public event Action OnMoodTimerFinished;
    public event Action<KidController> OnCooldownTimerFinished;

    private bool _isInCooldown = false;
    private bool _isFirstInit = false;

    private void Start() 
    {
        SpriteRenderer kidSprite = GetComponentInChildren<SpriteRenderer>();
        if (kidVariations != null && kidVariations.Count > 0 && kidSprite != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, kidVariations.Count);
            kidSprite.sprite = kidVariations[randomIndex];
        }
    }

    private void Update()
    {
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
            moodTimer.RefreshUI(_currentMoodTimer);

            if (_currentMoodTimer > 0) return;

            moodTimer.SetVisiblity(false);
            _currentMoodTimer = _baseMoodDuration;
            TriggerCooldown();
            OnMoodTimerFinished?.Invoke();
        }

    }
    public void SetupData(float moodDuration, float cooldownDuration)
    {
        _isFirstInit = true;
        _baseMoodDuration = moodDuration;
        _currentMoodTimer = _baseMoodDuration;

        _baseCooldownDuration = cooldownDuration;

        moodTimer.Init(moodDuration);

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
    }

    public MoodTimer GetMoodTimer()
    {
        return moodTimer;
    }

    public bool IsInCooldown()
    {
        return _isInCooldown;
    }
}
