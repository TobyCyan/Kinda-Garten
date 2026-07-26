using UnityEngine.EventSystems;
using TMPro;
using System;

public class EmptyLetterBlock : LetterBlock, IDropHandler
{
    public event Action OnCorrectLetterDropped;
    private char supposedLetter;

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
                SfxManager.Instance.Play(SfxId.WoodenBlockPlaced);
                Destroy(droppedLetterBlock.gameObject);
                base.Init(droppedLetter);
                OnCorrectLetterDropped?.Invoke();
            }
            else
            {
                SfxManager.Instance.Play(SfxId.WrongMove);
            }
        }
    }

    private bool IsCorrectLetter(char letter)
    {
        return letter.ToString().ToUpper().Equals(supposedLetter.ToString().ToUpper());
    }
}