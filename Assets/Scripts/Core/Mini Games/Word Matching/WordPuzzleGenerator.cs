using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordPuzzleGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI definitionText;
    [SerializeField] private GameObject letterBlockPrefab;
    [SerializeField] private GameObject selectableLetterBlockPrefab;

    private const int MIN_MISSING_LETTERS = 2;
    private const int MIN_EXTRA_BLOCKS = 2;
    private const int MAX_EXTRA_BLOCKS = 4;

    public readonly struct PuzzleData
    {
        public List<SelectableLetterBlock> SelectableLetterBlocks { get; }
        public List<LetterBlock> DisplayedLetterBlocks { get; }
        public PuzzleData(List<SelectableLetterBlock> selectableLetterBlocks, List<LetterBlock> displayedLetterBlocks)
        {
            SelectableLetterBlocks = selectableLetterBlocks;
            DisplayedLetterBlocks = displayedLetterBlocks;
        }
    }

    private void Start()
    {
        GeneratePuzzle();
    }

    public void GeneratePuzzle()
    {
        var word = WordPool.GetRandomWord();
        definitionText.text = word.Definition;

        PuzzleData puzzleData = GetPuzzleData(word.Word);

        // TODO: Arrange the blocks in the scene.
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

        // Randomly remove letters from the word
        int maxMissingLetters = Mathf.Max(MIN_MISSING_LETTERS, word.Length - 2);
        int missingLettersCount = Random.Range(MIN_MISSING_LETTERS, maxMissingLetters + 1);
        char[] displayedLetters = word.ToCharArray();
        for (int i = 0; i < missingLettersCount; i++)
        {
            int pos = positions[i];
            displayedLetters[pos] = '_';
            missingLetters.Add(upperWord[pos]);
        }

        // Generate letter blocks for the displayed letters
        foreach (char letter in displayedLetters)
        {
            GenerateLetterBlockObject(letter, letterBlockPrefab, displayedLetterBlocks);
        }

        // Generate letter blocks for the missing letters
        foreach (char letter in missingLetters)
        {
            GenerateLetterBlockObject(letter, selectableLetterBlockPrefab, selectableLetterBlocks);
        }

        // Generate extra blocks
        int extraBlocksCount = Random.Range(MIN_EXTRA_BLOCKS, MAX_EXTRA_BLOCKS + 1);
        for (int i = 0; i < extraBlocksCount; i++)
        {
            char randomLetter = (char)Random.Range('A', 'Z' + 1);
            GenerateLetterBlockObject(randomLetter, selectableLetterBlockPrefab, selectableLetterBlocks);
        }
        return new(selectableLetterBlocks, displayedLetterBlocks);
    }

    private void GenerateLetterBlockObject<T>(char letter, GameObject letterBlockPrefab, List<T> letterBlocks) where T : LetterBlock
    {
        GameObject letterBlockGo = Instantiate(letterBlockPrefab);
        if (letterBlockGo.TryGetComponent(out T lb))
        {
            lb.Init(letter);
            letterBlocks.Add(lb);
        }
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}