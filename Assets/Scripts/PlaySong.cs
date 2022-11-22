using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    [SerializeField] AudioSource jukebox = null;

    private bool destroy = false;

    float time = 0.5f;
    private void Update()
    {
        if(destroy)
        {
            time -= Time.unscaledDeltaTime;
            if (time < 0) Destroy(gameObject);
        }
    }

    public void PlaySoundSelect()
    {
        AudioSource juke_copy = Instantiate(jukebox, Vector3.zero, Quaternion.identity);
        juke_copy.Play();
        juke_copy.GetComponent<PlaySong>().destroy = true;
    }
}
