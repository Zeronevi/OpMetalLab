using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentCamera : MonoBehaviour
{

    public static float MIN_NECESSARY_TO_START_BLACKOUT = 65;
    public static float MAX_LIFE = 200f;

    [SerializeField] private float angular_speed = 1f;
    [SerializeField] private float maxVariationAngle = 20f;
    [SerializeField] private float time_wait = 2f;
    [SerializeField] private GameObject vision = null;

    [SerializeField] Animator animator;
    [SerializeField] AudioClip estaticaAudio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioSource audioSource;

    private float current_angle;
    private float current_speed;
    private float time = 0;

    private float ENERGY_MAX = 20f;
    private float energy; 

    private float initial_angle;
    private float life;
    private bool working = true;
    // Start is called before the first frame update
    void Start()
    {
        current_angle = -maxVariationAngle;
        current_speed = angular_speed;
        initial_angle = transform.rotation.eulerAngles.z;
        animator = transform.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        life = MAX_LIFE;
        energy = ENERGY_MAX;
    }

    public void setInitialAngle(float angle)
    {
        this.initial_angle = angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!working) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;

            if (time > 0) return;
        }

        float deltaAngle = current_speed * Time.deltaTime;
        float newAngle = current_angle + deltaAngle;
        if (newAngle >= maxVariationAngle)
        {
            newAngle = maxVariationAngle;
            current_speed = -angular_speed;
            time = time_wait;
        }
        else if (newAngle <= -maxVariationAngle)
        {
            newAngle = -maxVariationAngle;
            current_speed = angular_speed;
            time = time_wait;
        }
        current_angle = newAngle;
        transform.rotation = Quaternion.Euler(0, 0, current_angle+initial_angle);
        animator.SetFloat("Life", life);
            
    }

    private void Update()
    {
        if (life > MAX_LIFE - MIN_NECESSARY_TO_START_BLACKOUT) return;

        float speed = 20f;
        if (energy > 0)
        {
            float deltaEnergy = (1f - life / MAX_LIFE) * speed * Time.deltaTime;
            energy -= deltaEnergy;
            if (energy < 0)
            {
                vision.SetActive(false);
            }
        }
        else
        {
            float deltaEnergy = life / MAX_LIFE * Time.deltaTime * speed / 12f * Random.Range(0.01f, 0.20f);
            energy += deltaEnergy;
            if (energy > 0)
            {
                energy = ENERGY_MAX;
                vision.SetActive(true);
            }
        }

        //print(energy + "  " + life);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Contains("Bullet"))
        {
            if (working) Dano(collision.gameObject.GetComponent<Bullet>().GetDamage());
            Destroy(collision.gameObject);

        }
    }

    public void Dano(float dano)
    {
        life -= dano;

        if (life <= 0)
        {
            working = false;
            vision.SetActive(false);
            animator.SetBool("Fail", true);

            audioSource.Stop();
            audioSource.clip = explosionAudio;
            audioSource.loop = false;
            audioSource.maxDistance = 100f;
            audioSource.Play();

            Destroy(this, 3f);

        } else if (life <= 150)
        {
            audioSource.Stop();
            audioSource.clip = estaticaAudio;
            audioSource.loop = true;
            audioSource.maxDistance = 10f;
            audioSource.Play();
        }
    }
}
