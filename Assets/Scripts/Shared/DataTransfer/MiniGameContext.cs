using static Seat;
public record MiniGameContext
{
    public SeatColor SeatColor { get; }
    public MoodTimer MoodTimerRef { get; }

    public MiniGameContext(SeatColor seatColor, MoodTimer moodTimerRef)
    {
        SeatColor = seatColor;
        MoodTimerRef = moodTimerRef;
    }
}
