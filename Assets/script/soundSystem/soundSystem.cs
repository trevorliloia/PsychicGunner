using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundSystem : MonoBehaviour {
    public static soundSystem sharedInstance;

    public List<audioClipMeta> audioClipMetas;
    private Dictionary<audioClipKey, audioClipMeta> audioClipMetaMap;

    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }

        audioClipMetaMap = new Dictionary<audioClipKey, audioClipMeta>();

        foreach (audioClipMeta meta in audioClipMetas) {
            audioClipMetaMap[meta.key] = meta;
        }
        // TODO: load and cache audio clips dynamically
    }

    public void Play(audioClipKey key) {
        GameObject audioSourceObj = objectPool.sharedInstance.GetPooledObject(pooledObjectKey.AUDIO_SOURCE);
        if (audioSourceObj != null) {
            audioSourceObj.SetActive(true);
            AudioSource audioSource = audioSourceObj.GetComponent<AudioSource>();
            audioSource.clip = audioClipMetaMap[key].clip;
            audioSource.Play();
            StartCoroutine(ClipTimer(audioSourceObj));
        }
    }

    private IEnumerator ClipTimer(GameObject audioSourceObj) {
        yield return new WaitWhile(() => audioSourceObj.GetComponent<AudioSource>().isPlaying);

        audioSourceObj.SetActive(false);
    }
}
