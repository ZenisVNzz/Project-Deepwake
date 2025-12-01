using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public List<AudioClip> bgmList;
    public AudioSource source;

    private void Start()
    {
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
