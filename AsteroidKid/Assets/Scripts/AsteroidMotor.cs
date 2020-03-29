using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMotor : MonoBehaviour
{
    [HideInInspector]
    public AsteroidSpawner spawner;
    public Transform asteroidTrans;

    public bool active = false;
    public float moveSpeed = 0.1f, rotateSpeed = 10f;

    private void OnEnable()
    {
        active = true;
        moveSpeed = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        transform.position += transform.up * Time.deltaTime * moveSpeed;
        asteroidTrans.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);

        //Remove asteroid if it hits the floor
        if (transform.position.y < -4.5f)
            RemoveAsteroid();
    }

    /// <summary>
    /// Resets asteroid and pushes back to the pool
    /// </summary>
    public void RemoveAsteroid()
    {
        transform.position = transform.parent.position;
        active = false;
        gameObject.SetActive(false);

        spawner.ReturnToPool(gameObject);
    }
}
