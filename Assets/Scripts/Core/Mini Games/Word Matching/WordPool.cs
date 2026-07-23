using UnityEngine;

public class WordPool
{
    public readonly struct WordData
    {
        public string Word { get; }
        public string Definition { get; }
        public WordData(string word, string description)
        {
            Word = word;
            Definition = description;
        }
    }

    private static readonly WordData[] words = new WordData[]
    {
        new("apple", "A red round-shaped fruit."),
        new("banana", "A monkey's all-time favorite."),
        new("cherry", "Red round-shaped fruits usually in pairs."),
        new("book", "A type of reading material."),
        new("flower", "Smells nice, looks pretty, attracts butterflies."),
        new("star", "What's on top of a Christmas tree?"),
        new("grape", "A red round-shaped fruit."),
        new("table", "A furniture to place things on top of."),
        new("pencil", "I use this to write notes."),
        new("lemon", "Extremely sour!"),
        new("calendar", "How to tell the date?")
    };

    public static WordData GetRandomWord()
    {
        int randomIndex = Random.Range(0, words.Length);
        return words[randomIndex];
    }
}
