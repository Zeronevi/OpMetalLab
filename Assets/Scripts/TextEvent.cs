using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent : MonoBehaviour
{
    [SerializeField] string text = "SET THE TEXT HERE";
    [SerializeField] bool isSpeak = false;
    [SerializeField] bool isMsgBox = false;

    [SerializeField] bool onEnter = false;
    [SerializeField] bool onExit = false;
    [SerializeField] bool justOne = false;

    private TextSpeakSystem speakSystem = null;
    private void Start()
    {
        speakSystem = FindObjectOfType<TextSpeakSystem>();
        if(speakSystem == null)
        {
            Debug.Log("Nenhum speakSystem Configurado!");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("Player") && onEnter)
        {
            execute();
            if (justOne) Destroy(gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Player") && onExit)
        {
            execute();
            if (justOne) Destroy(gameObject);
        }

        
    }

    private void execute()
    {
        if (isSpeak)
        {
            speakSystem.ShowText(text);
        }
        else if (isMsgBox)
        {
            speakSystem.ShowMsgBox(text);
        }
    }
}
