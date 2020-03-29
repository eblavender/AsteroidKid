using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public PlayerShooting spawner;

    public bool active = false;
    public float bulletSpeed = 1f;

    private void OnEnable()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        transform.position += transform.up * Time.deltaTime * bulletSpeed;

        if (transform.position.y > 5f)
            RemoveBullet();
    }

    /// <summary>
    /// Check if collided with asteroid
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            collision.GetComponent<AsteroidMotor>().RemoveAsteroid();
            RemoveBullet();
            spawner.manager.BumpScore(true);
        }
    }

    /// <summary>
    /// Reset bullet and push back to the pool
    /// </summary>
    public void RemoveBullet()
    {
        transform.position = new Vector3(100,100,0);
        active = false;
        gameObject.SetActive(false);

        spawner.ReturnToPool(gameObject);
    }
}
