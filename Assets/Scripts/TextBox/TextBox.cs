using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    // Start is called before the first frame update

    public static float TIME_TO_DESTROY = 5f;

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private TextMeshPro text;

    private GameObject referenceObj = null;

    Vector3 initial_position;

    void Start()
    {
        initial_position = transform.position;
    }

    public void SetReferenceObj(GameObject obj)
    {
        this.referenceObj = obj;
    } 

    public void SetMessage(string txt)
    {
        Adjust(txt);
    }

    private void Adjust(string txt)
    {
        this.text.text = txt;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        Destroy(gameObject, TIME_TO_DESTROY);
    }

    private void Update()
    {
        if (referenceObj != null) transform.position = referenceObj.transform.position + initial_position;
    }

}
