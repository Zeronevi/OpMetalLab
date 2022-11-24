using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpeakSystem : MonoBehaviour
{

    [SerializeField] private MainCharacter player = null;
    [SerializeField] private TextBox textBox = null;
    [SerializeField] private MsgBox msgBox = null;
    private TextBox current_textBox = null;
    
    [SerializeField] private GameObject actionBox;

    float timeToWait = 0;


    public void ShowText(string text)
    {
        current_textBox = Instantiate(textBox, Vector3.zero, Quaternion.identity);
        current_textBox.SetReferenceObj(player.gameObject);
        current_textBox.SetMessage(text);
        current_textBox.Enable();

        timeToWait = TextBox.TIME_TO_DESTROY;

    }

    public void ShowMsgBox(string text)
    {
        MsgBox copy = Instantiate(msgBox, Vector3.zero, Quaternion.identity);
        copy.SetText(text);
    }

    public void ShowActionBox()
    {
        if(actionBox.activeSelf == false && actionBox != null) actionBox.SetActive(true);
    }

    public void HideActionBox()
    {
        if (actionBox.activeSelf == true && actionBox != null) actionBox.SetActive(false);
    }

    private void FixedUpdate()
    {
        timeToWait -= Time.deltaTime;
        if(timeToWait <= 0)
        {
            timeToWait = 0;
            current_textBox = null;
        }
    }

    private void Update()
    {
        actionBox.transform.position = player.transform.position + (new Vector3(0, -1, 0));
    }
}
