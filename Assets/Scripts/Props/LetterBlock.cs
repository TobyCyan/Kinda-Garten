using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    protected TextMeshPro letterText;

    public const char EMPTY_LETTER = ' ';
    private static readonly List<Color32> LETTER_COLORS = new()
    {
        new(238, 83, 100, 255), // Light Red
        new(173, 215, 137, 255), // Light Green
        new(64, 184, 231, 255), // Light Blue
        new(249, 202, 54, 255), // Light Yellow
    };
    private static readonly Dictionary<char, Color32> LETTER_COLOR_MAP = new();

    private void Awake()
    {
        letterText = GetComponentInChildren<TextMeshPro>();
    }

    public virtual void Init(char letter)
    {
        if (letterText == null) return;

        letterText.text = letter.ToString().ToUpper();
        if (LETTER_COLOR_MAP.ContainsKey(letter))
        {
            letterText.color = LETTER_COLOR_MAP[letter];
        }
        else
        {
            Color32 randomColor = LETTER_COLORS[Random.Range(0, LETTER_COLORS.Count)];
            letterText.color = randomColor;
            LETTER_COLOR_MAP[letter] = randomColor;
        }
    }

    public char GetDisplayedLetter()
    {
        if (letterText != null)
        {
            return letterText.text[0];
        }
        return EMPTY_LETTER;
    }
}
