using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] private float YELLOW_LIFE = 0.30f;
    [SerializeField] private float RED_LIFE = 0.15f;

    private Image life_bar;
    private Image referemce_life_bar;

    // Start is called before the first frame update
    void Start()
    {
        referemce_life_bar = transform.GetChild(1).gameObject.GetComponent<Image>();
        life_bar = transform.GetChild(2).gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
    }

    private void UpdateLife()
    {
        float life = 0;
        life = PlayerStatus.getCurrentLife() / PlayerStatus.MAX_LIFE;

        float maxSize = referemce_life_bar.transform.localScale.x;

        Vector3 current_scale = life_bar.transform.localScale;
        current_scale.x = life * maxSize;
        life_bar.transform.localScale = current_scale;

        if (life <= RED_LIFE) life_bar.color = Color.red;
        else if (life <= YELLOW_LIFE) life_bar.color = Color.yellow;
        else life_bar.color = Color.green;
    }

}
