using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float TIME_TO_ANOTHER_WARNING = 10f;

    [SerializeField] int keys_need = 1;
    [SerializeField] TextSpeakSystem speakSystem = null;

    [SerializeField] private GameFinishControl gameFinish = null;

    private bool isEnter = false;
    private float time = 0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnter && !collision.name.Equals("Player")) return;

        int my_keys = PlayerStatus.GetInstance().getKeys();
        if (my_keys >= keys_need)
        {
            Instantiate(gameFinish, Vector2.zero, Quaternion.identity).Winner();
        } else
        {
            int keys_reaming = (keys_need - my_keys);
            if (speakSystem != null && time <= 0)
            {
                if(keys_reaming == 1) speakSystem.ShowText(keys_reaming + " key reamining!");
                else speakSystem.ShowText(keys_reaming + " keys reamining!");
                time = TIME_TO_ANOTHER_WARNING;
            }
        }
        isEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isEnter = false;
    }

    private void FixedUpdate()
    {
        if(time > 0) time -= Time.fixedDeltaTime;
        
        if(time <= 0)
        {
            time = 0;     
        }
    }
}
