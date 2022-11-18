using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] private AudioSource jukebox = null;
    [SerializeField] private GameObject current_camera = null;
    [SerializeField] private GameObject Enemy_example = null;
    [SerializeField] private int enemys = 3;
    [SerializeField] private float time_spawn = 10f;

    private float current_time = 0;
    // Start is called before the first frame update
    void Start()
    {
        instanceAnother();
    }


    [SerializeField] private float MIN_X = -15f;
    [SerializeField] private float MAX_X = 15f;

    [SerializeField] private float MIN_Y = -10f;
    [SerializeField] private float MAX_Y = 10f;

    private void instanceAnother()
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(MIN_X, MAX_X);

        float height = 2 * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        if (position.x < -width/2 || position.x > width / 2)
        {
            position.y = Random.Range(MIN_Y, MAX_Y);
        } else
        {
            float indicador = Random.Range(-1, 1);
            if(indicador < 0)
            {
                position.y = Random.Range(height/2, MAX_Y);
            } else
            {
                position.y = Random.Range(MIN_Y, -height / 2);
            }
        }

        float angle = Utils.GetAngleFormVectorFloat(-position);

        current_camera.transform.position = position;
        current_camera.GetComponent<MovimentCamera>().setInitialAngle(angle);


        for (int i = 0; i < 3; i++)
        {
            Vector3 positionEnemy = Vector3.zero;
            positionEnemy.x = Random.Range(-width / 2, width / 2);
            positionEnemy.y = Random.Range(-height / 2, height / 2);

            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

            Instantiate(Enemy_example, positionEnemy, rotation).SetActive(true);
        }

        current_time = time_spawn;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlaySoundSelect()
    {
        AudioSource juke_copy = Instantiate(jukebox, Vector3.zero, Quaternion.identity);
        juke_copy.Play();
        Destroy(juke_copy.gameObject, 0.5f);
    }
}
