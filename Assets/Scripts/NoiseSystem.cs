using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton　(X﹏X)。
//Todo: Deixar de ser Singleton, <<controlar as coisas por eventos>>

public class NoiseSystem : MonoBehaviour
{ 
    public static List<Noise> Noises;
    public GameObject Player;
    
    public void Start()
    {
        Noises = new List<Noise>();
    }

    public void Update()
    {
        DebugInfo();
    }

    public static void MakeNoise(Vector2 position, float strenght)
    {
        foreach (GameObject enemy in Enemy.enemyList)
        {
            if (Vector2.Distance(position, enemy.transform.position) < strenght)
            {
                enemy.GetComponent<Enemy>().SetDestiny(position);
                print("Sampla");
            }
        }
        Noises.Add(new Noise(position,strenght));
    }

    void DebugInfo()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Noises.Add(new Noise(SharedContent.MousePosition,2.0f));
        }
    }
    
    
    public void OnDrawGizmos()
    {
        //TODO: Desenhar os Gizmos
        if (Noises != null && Noises.Count != 0)
        {
            foreach (var noise in Noises)
            {
                Gizmos.DrawWireSphere(noise.Position, noise.Strength);
            }
        }
    }
}


public class Noise
{
    public Vector2 Position;
    public float Strength;

    public Noise(Vector2 pos, float strength)
    {
        Position = pos;
        Strength = strength;
    }
}
