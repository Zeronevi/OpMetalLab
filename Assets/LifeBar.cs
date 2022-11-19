using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    private float life = 1;

    public void SetLife(float life)
    {
        this.life = life;
        UpdateLife();
    }

    [SerializeField] private float INVISIBLE_LIFE = 0.75f;
    [SerializeField] private float YELLOW_LIFE = 0.30f;
    [SerializeField] private float RED_LIFE = 0.15f;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject lifeObject;
    [SerializeField] private SpriteRenderer life_bar;
    [SerializeField] private SpriteRenderer referemce_life_bar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeObject.transform.position = enemy.transform.position;
    }

    private void UpdateLife()
    {

        float maxSize = referemce_life_bar.transform.localScale.x;

        Vector3 current_scale = life_bar.transform.localScale;
        current_scale.x = life * maxSize;
        life_bar.transform.localScale = current_scale;

        if (life > INVISIBLE_LIFE)
        {
            Color color = Color.white;
            color.a = 0;
            life_bar.color = color;
        }
        else if (life <= RED_LIFE) life_bar.color = Color.red;
        else if (life <= YELLOW_LIFE) life_bar.color = Color.yellow;
        else life_bar.color = Color.green;
    }
}
