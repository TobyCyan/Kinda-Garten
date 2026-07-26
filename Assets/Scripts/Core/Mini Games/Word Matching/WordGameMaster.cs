using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.Random;

public class WordGameMaster : MonoBehaviour, IMiniGameMaster
{
    // UI
    [SerializeField] private TextMeshProUGUI definitionText;
    [SerializeField] private GameObject definitionObject;
    [SerializeField] private SpriteRenderer woodenCase;

    // Prefabs for the letter blocks
    [SerializeField] private GameObject letterBlockPrefab;
    [SerializeField] private GameObject selectableLetterBlockPrefab;
    [SerializeField] private GameObject emptyLetterBlockPrefab;

    private const int MIN_MISSING_LETTERS = 2;
    private const int MAX_MISSING_LETTERS = 3;
    private const int MIN_EXTRA_BLOCKS = 2;
    private const int MAX_EXTRA_BLOCKS = 3;

    [SerializeField] private float rowWidth = 8.0f;   // total width available for the row
    [SerializeField] private int maxVisibleBlocks = 8;
    [SerializeField] private Transform displayedRowCenter;
    [SerializeField] private Transform selectableRowCenter;

    private PuzzleData currentPuzzleData;

    public class PuzzleData
    {
        public string Word { get; }
        public List<SelectableLetterBlock> SelectableLetterBlocks { get; }
        public List<LetterBlock> DisplayedLetterBlocks { get; }
        public PuzzleData(string word, List<SelectableLetterBlock> selectableLetterBlocks, List<LetterBlock> displayedLetterBlocks)
        {
            Word = word;
            SelectableLetterBlocks = selectableLetterBlocks;
            DisplayedLetterBlocks = displayedLetterBlocks;
        }
    }

    public readonly struct DisplayedLetter
    {
        public char Letter { get; }
        public bool IsMissing { get; }
        public DisplayedLetter(char letter, bool isMissing)
        {
            Letter = letter;
            IsMissing = isMissing;
        }
    }

    public event Action OnMiniGameCompleted;

    public void GenerateMiniGame()
    {
        var word = WordPool.GetRandomWord();
        definitionText.text = word.Definition;
        definitionObject.SetActive(true);
        woodenCase.enabled = true;

        PuzzleData puzzleData = GetPuzzleData(word.Word);
        currentPuzzleData = puzzleData;
        ArrangeRow(puzzleData.DisplayedLetterBlocks, displayedRowCenter);
        ArrangeRow(puzzleData.SelectableLetterBlocks, selectableRowCenter);
    }

    public void CleanUpMiniGame()
    {
        definitionObject.SetActive(false);
        woodenCase.enabled = false;
        if (currentPuzzleData == null)
            return;

        foreach (var lb in currentPuzzleData.DisplayedLetterBlocks)
        {
            if (lb != null)
                Destroy(lb.gameObject);
        }

        foreach(var lb in currentPuzzleData.SelectableLetterBlocks)
        {
            if (lb != null)
                Destroy(lb.gameObject);
        }
    }

    private PuzzleData GetPuzzleData(string word)
    {
        List<SelectableLetterBlock> selectableLetterBlocks = new();
        List<LetterBlock> displayedLetterBlocks = new();

        string upperWord = word.ToUpper();

        // Choose unique positions to hide
        List<int> positions = Enumerable.Range(0, word.Length).ToList();
        positions.Shuffle();

        List<char> missingLetters = new();
        HashSet<int> missingPositions = new();

        // Randomly remove letters from the word
        int missingLettersCount = Range(MIN_MISSING_LETTERS, MAX_MISSING_LETTERS + 1);
        char[] charArray = upperWord.ToCharArray();
        for (int i = 0; i < missingLettersCount; i++)
        {
            int pos = positions[i];
            missingPositions.Add(pos);
            missingLetters.Add(upperWord[pos]);
        }

        DisplayedLetter[] displayedLetters = FromCharArray(charArray, missingPositions);

        // Generate letter blocks for the displayed letters
        foreach (DisplayedLetter dl in displayedLetters)
        {
            GameObject prefab = dl.IsMissing ? emptyLetterBlockPrefab : letterBlockPrefab;
            GenerateLetterBlockObject(dl.Letter, prefab, displayedLetterBlocks);
        }

        // Generate letter blocks for the missing letters
        foreach (char letter in missingLetters)
        {
            GenerateLetterBlockObject(letter, selectableLetterBlockPrefab, selectableLetterBlocks);
        }

        // Generate extra blocks
        int extraBlocksCount = Range(MIN_EXTRA_BLOCKS, MAX_EXTRA_BLOCKS + 1);
        for (int i = 0; i < extraBlocksCount; i++)
        {
            char randomLetter = (char)Range('A', 'Z' + 1);
            GenerateLetterBlockObject(randomLetter, selectableLetterBlockPrefab, selectableLetterBlocks);
        }
        selectableLetterBlocks.Shuffle();
        return new(word, selectableLetterBlocks, displayedLetterBlocks);
    }

    private DisplayedLetter[] FromCharArray(char[] charArray, HashSet<int> missingPositions)
    {
        DisplayedLetter[] displayedLetters = new DisplayedLetter[charArray.Length];
        for (int i = 0; i < charArray.Length; i++)
        {
            displayedLetters[i] = new DisplayedLetter(charArray[i], missingPositions.Contains(i));
        }
        return displayedLetters;
    }

    private void GenerateLetterBlockObject<T>(char letter, GameObject letterBlockPrefab, List<T> letterBlocks) where T : LetterBlock
    {
        GameObject letterBlockGo = Instantiate(letterBlockPrefab);
        if (letterBlockGo.TryGetComponent(out T lb))
        {
            lb.Init(letter);
            letterBlocks.Add(lb);
            if (letterBlockGo.TryGetComponent<EmptyLetterBlock>(out var elb))
            {
                elb.OnCorrectLetterDropped += CheckWord;
            }
        }
    }

    private void CheckWord()
    {
        string currentWord = currentPuzzleData.Word.ToLower();
        string displayedWord = currentPuzzleData.DisplayedLetterBlocks
                .Select(lb => lb.GetDisplayedLetter()).Aggregate("", (acc, c) => acc + c)
                .ToLower();
        if (currentWord.Equals(displayedWord))
        {
            OnMiniGameCompleted?.Invoke();
        }
    }

    private void ArrangeRow<T>(IReadOnlyList<T> blocks, Transform rowCenter) where T : LetterBlock
    {
        if (blocks == null || blocks.Count == 0 || rowCenter == null)
            return;

        int count = Mathf.Min(blocks.Count, maxVisibleBlocks);

        // If there is only one block, center it exactly.
        if (count == 1)
        {
            SetBlockPosition(blocks[0].transform, rowCenter, Vector2.zero);
            return;
        }

        // Fit the row inside a fixed width and center it.
        float spacing = rowWidth / (count - 1);
        float startX = -rowWidth * 0.5f;

        for (int i = 0; i < count; i++)
        {
            float x = startX + (i * spacing);
            SetBlockPosition(blocks[i].transform, rowCenter, new Vector2(x, 0f));
        }
    }

    private void SetBlockPosition(Transform block, Transform parent, Vector2 localPos)
    {
        if (block == null || parent == null)
            return;

        block.SetParent(parent, false);
        block.localPosition = localPos;
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}