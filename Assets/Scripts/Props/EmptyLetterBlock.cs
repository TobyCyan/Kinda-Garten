using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class EmptyLetterBlock : LetterBlock, IDropHandler
{
    private char supposedLetter;
    public UnityEvent OnCorrectLetterDropped;

    public override void Init(char letter)
    {
        supposedLetter = letter;
        base.Init(EMPTY_LETTER);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<SelectableLetterBlock>(out var droppedLetterBlock))
        {
            TextMeshPro droppedLetterText = droppedLetterBlock.GetComponentInChildren<TextMeshPro>();
            char droppedLetter = droppedLetterText.text[0];
            if (IsCorrectLetter(droppedLetter))
            {
                Destroy(droppedLetterBlock.gameObject);
                letterText.text = supposedLetter.ToString().ToUpper();
            }
        }
    }

    private bool IsCorrectLetter(char letter)
    {
        return letter.ToString().ToUpper().Equals(supposedLetter.ToString().ToUpper());
    }
}