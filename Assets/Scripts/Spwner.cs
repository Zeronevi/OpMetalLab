using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwner : MonoBehaviour
{
    [SerializeField] float TIME_TO_SPAWN = 5f;
    [SerializeField] GameObject enemyModel = null;

    private float time = 0;
    private bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        time -= Time.deltaTime;
        if(time <= 0)
        {
            Spawnar();
            time = TIME_TO_SPAWN + Random.Range(0, 2);
        }

    }

    public void Active()
    {
        isActive = true;
    }

    public void Spawnar()
    {
        Enemy enemy = Instantiate(enemyModel, gameObject.transform.position, Quaternion.identity).GetComponentInChildren<Enemy>();
        enemy.EnterChase();
    }
}
