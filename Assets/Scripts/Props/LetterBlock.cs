using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    private TextMeshPro letterText;

    private void Awake()
    {
        letterText = GetComponentInChildren<TextMeshPro>();
    }

    public void Init(char letter)
    {
        if (letterText != null)
        {
            letterText.text = letter.ToString().ToUpper();
        }
    }
}
