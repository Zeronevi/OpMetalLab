using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] private float YELLOW_LIFE = 0.6f;
    [SerializeField] private float RED_LIFE = 0.3f;

    [SerializeField] private Image life_bar;
    [SerializeField] private Image referemce_life_bar;
    [SerializeField] private Image energy_bar;


    // Update is called once per frame
    void Update()
    {
        UpdateLife();
        UpdateEnergy();
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

    private void UpdateEnergy()
    {
        float energy = 0;
        energy = PlayerStatus.getCurrentEnergy() / PlayerStatus.MAX_ENERGY;

        float maxSize = referemce_life_bar.transform.localScale.x;

        Vector3 current_scale = energy_bar.transform.localScale;
        current_scale.x = energy * maxSize;
        energy_bar.transform.localScale = current_scale;

        energy_bar.color = energy * Color.blue + (1 - energy) * Color.red;
    }

}
