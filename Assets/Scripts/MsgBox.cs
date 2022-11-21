using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MsgBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textArea = null;
    [SerializeField] private Animator anime = null;

    private void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1;
            anime.SetBool("destroy", true);
            Destroy(this.gameObject, 1f);
        }
    }

    public void SetText(string text)
    {
        textArea.text = text;
    }
}
