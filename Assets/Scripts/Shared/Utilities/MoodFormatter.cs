using UnityEngine;

public class MoodFormatter
{
    private static readonly Color32 MOOD_COLOR_HAPPY = new(123, 224, 111, 255); // Green
    private static readonly Color32 MOOD_COLOR_NEUTRAL = new(248, 152, 103, 255); // Orange
    private static readonly Color32 MOOD_COLOR_UNHAPPY = new(248, 105, 102, 255); // Red

    public static Color32 FormatMoodColor(float moodValue, float baseMoodValue)
    {
        float moodRatio = moodValue / baseMoodValue;
        if (moodRatio >= 0.65f)
        {
            return MOOD_COLOR_HAPPY;
        }
        
        if (moodRatio >= 0.4f)
        {
            return MOOD_COLOR_NEUTRAL;
        }

        return MOOD_COLOR_UNHAPPY;
    }
}
