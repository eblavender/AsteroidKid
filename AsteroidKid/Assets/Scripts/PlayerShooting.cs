using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [HideInInspector]
    public GameManager manager;

    [Header("Variables")]
    public GameObject bulletPrefab;
    public GameObject laserEyeLeft, laserEyeRight;
    public Transform spawnPos;
    public AudioClip[] shootingClips;

    private List<GameObject> bulletPool;
    private AudioSource audioSource;

    private PlayerMotor motor;

    public const float MAX_Y_HEIGHT = 5f;

    private GameObject tempObject;

    [Header("DEBUG")]
    public bool debugWithMouse = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        motor = GetComponent<PlayerMotor>();
        audioSource = GetComponent<AudioSource>();

        bulletPool = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Check for screen touches
        if (Input.touches.Length > 0 && motor.alive)
        {
            foreach (Touch touch in Input.touches)
            {
                //Check if touch is within top 2 thirds of screen
                if (touch.position.y < Screen.height / 3f)
                    continue;
                else if (touch.phase == TouchPhase.Began)
                    SpawnBullet();
            }
        }

        //Debug overrides for mouse input
        if (debugWithMouse && motor.alive)
            if (Input.GetMouseButtonDown(0))
                SpawnBullet();
    }

    /// <summary>
    /// Creates an instance of a bullet
    /// </summary>
    private void SpawnBullet()
    {
        if (bulletPool.Count == 0)
        {
            //Create new bullet if pool is empty
            tempObject = Instantiate(bulletPrefab, spawnPos.position, transform.rotation);
            tempObject.GetComponent<Bullet>().spawner = this;
        }
        else
        {
            //If pool is not empty grab next available bullet
            tempObject = GetNextFromPool();
            tempObject.transform.position = spawnPos.position;
        }

        //Play random shooting sound
        audioSource.clip = shootingClips[Random.Range(0, shootingClips.Length)];
        audioSource.Play();

        //Activate bullet and effects
        tempObject.SetActive(true);
        laserEyeLeft.SetActive(true);
        laserEyeRight.SetActive(true);
    }

    /// <summary>
    /// Returns next available bullet from pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetNextFromPool()
    {
        tempObject = bulletPool[0];
        bulletPool.Remove(tempObject);
        return tempObject;
    }
    /// <summary>
    /// Pushes a bullet back into the available pool
    /// </summary>
    /// <param name="asteroid"></param>
    public void ReturnToPool(GameObject asteroid)
    {
        bulletPool.Add(asteroid);
    }
}
