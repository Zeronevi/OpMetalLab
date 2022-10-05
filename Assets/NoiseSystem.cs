using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton　(X﹏X)。
//Todo: Deixar de ser Singleton, <<controlar as coisas por eventos>>

public class NoiseSystem : MonoBehaviour
{
    public List<Noise> SoundSources;
    public void OnDestroy()
    {
        //TODO: Desenhar os Gizmos
    }
}

public class Noise
{
    private Vector2 position;
    private float intensity;
}
