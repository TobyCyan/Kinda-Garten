using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [SerializeField] private List<SfxData> soundData;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private int poolSize = 8;

    private Dictionary<SfxId, SfxData> sfxDataById;
    private readonly Queue<AudioSource> sourcePool = new();

    private void Awake()
    {
        Instance = this;

        sfxDataById = soundData.ToDictionary(s => s.id, s => s);

        for (int i = 0; i < poolSize; i++)
        {
            var src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            sourcePool.Enqueue(src);
        }
    }

    public void Play(SfxId id)
    {
        if (!sfxDataById.TryGetValue(id, out var sfx))
            return;

        var source = GetSource();
        source.clip = sfx.clip;
        source.Play();

        StartCoroutine(ReturnWhenDone(source));
    }

    private AudioSource GetSource()
    {
        if (sourcePool.Count > 0)
            return sourcePool.Dequeue();

        return Instantiate(audioSourcePrefab, transform);
    }

    private IEnumerator ReturnWhenDone(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length / Mathf.Abs(source.pitch));
        sourcePool.Enqueue(source);
    }
}
