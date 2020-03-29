using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEye : MonoBehaviour
{
    public float respawnTime = 0.1f;
    private float timer = 0;

    private void OnEnable()
    {
        timer = respawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        gameObject.SetActive(false);
    }
}
