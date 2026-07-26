using UnityEngine;

public class MiniGameView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer baseRenderer;
    [SerializeField] private MoodTimerUI moodTimerUi;

    public void ShowView(MiniGameContext context)
    {
        gameObject.SetActive(true);
        baseRenderer.sprite = SeatSpriteStore.GetTableSprite(context.SeatColor);
        baseRenderer.enabled = true;

        moodTimerUi.Init(context.MoodTimerRef);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
        moodTimerUi.CleanUp();
    }
}
