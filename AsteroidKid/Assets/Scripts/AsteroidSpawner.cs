using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public const float START_Y = 5f;
    public const float MIN_X = -5f;
    public const float MAX_X = 5f;

    public List<GameObject> asteroidPool;
    public GameObject asteroidPrefab;

    private GameObject tempObject;
    private AudioSource audioSource;
    private Vector3 spawnPos;

    public float respawnTime = 1f;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        timer = respawnTime;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = respawnTime;
        SpawnAsteroid();
    }

    /// <summary>
    /// Create an instance of an asteroid
    /// </summary>
    private void SpawnAsteroid()
    {
        spawnPos = new Vector3(Random.Range(MIN_X, MAX_X), START_Y, 0);

        if (asteroidPool.Count == 0)
        {
            //Create new asteroid if pool is empty
            tempObject = Instantiate(asteroidPrefab, spawnPos, transform.rotation, transform);
            tempObject.GetComponent<AsteroidMotor>().spawner = this;
        }
        else
        {
            //If pool is not empty grab next available asteroid
            tempObject = GetNextFromPool();
            tempObject.transform.position = spawnPos;
            tempObject.SetActive(true);
        }

        //Randomise travel direction
        tempObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(120f, 240f));
    }

    /// <summary>
    /// Returns next available asteroid from pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetNextFromPool()
    {
        tempObject = asteroidPool[0];
        asteroidPool.Remove(tempObject);
        return tempObject;
    }
    /// <summary>
    /// Pushes an asteroid back into the available pool
    /// </summary>
    /// <param name="asteroid"></param>
    public void ReturnToPool(GameObject asteroid)
    {
        audioSource.Play();
        asteroidPool.Add(asteroid);
    }
}
