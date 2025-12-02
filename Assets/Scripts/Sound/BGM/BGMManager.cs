using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public List<AudioClip> bgmList;
    public AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(PlayRandomBGM());
    }

    private IEnumerator PlayRandomBGM()
    {
        while (true)
        {
            AudioClip clip = bgmList[Random.Range(0, bgmList.Count)];
            source.clip = clip;
            source.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }
}
