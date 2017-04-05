using UnityEngine;
using System.Collections;

public class AudioTick : MonoBehaviour {
    [SerializeField]
    private Transform transform;
    [SerializeField]
    private AudioSource audioSource;

    private bool hasPlayed;

    public Vector3 Position {
        set {
            transform.position = new Vector3(value.x, transform.position.y, value.y);
        }
    }

    public void Play(float pitch = 1) {
        audioSource.pitch = pitch;
        audioSource.Play();
        hasPlayed = true;
    }

    private void Update() {
        if (hasPlayed && !audioSource.isPlaying) {
            Destroy(this.gameObject);
        }
    }
}