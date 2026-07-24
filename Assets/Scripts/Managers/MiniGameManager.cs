using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    private List<IMiniGameGenerator> generators = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        generators = new List<IMiniGameGenerator>(FindObjectsByType<MonoBehaviour>().OfType<IMiniGameGenerator>());
    }

    private void Start()
    {
        GenerateMiniGame();
    }

    public void GenerateMiniGame()
    {
        var generator = generators[Random.Range(0, generators.Count)];
        if (generator == null)
        {
            return;
        }
        generator.GenerateMiniGame();
    }
}