using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    protected TextMeshPro letterText;

    public const char EMPTY_LETTER = ' ';

    private void Awake()
    {
        letterText = GetComponentInChildren<TextMeshPro>();
    }

    public virtual void Init(char letter)
    {
        if (letterText != null)
        {
            letterText.text = letter.ToString().ToUpper();
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
