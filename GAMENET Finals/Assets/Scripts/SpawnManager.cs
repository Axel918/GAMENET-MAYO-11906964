using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Transform[] spawnPoints;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public GameObject[] powerUps;

    public float time;
    private float currentTime;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerManager.instance.timerActive == true)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                Spawner();
                currentTime = time;
            }
        }
    }

    public void Spawner()
    {
        int randomIndex = Random.Range(0, powerUps.Length);
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Instantiate(powerUps[randomIndex], new Vector2(randomX, randomY), Quaternion.identity);
    }
}
