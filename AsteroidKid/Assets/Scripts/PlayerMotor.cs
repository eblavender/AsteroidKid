using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private enum MoveState { Idling, Running, Dead};

    private GameManager manager;

    //Camera clamps
    public const float MIN_RUN_X = -7.5f;
    public const float MAX_RUN_X = 7.5f;

    public float moveSpeed = 1f;
    public bool alive = true;
    public bool debugWithMouse = false;

    private Camera mainCam;
    private Animator anim;
    private MoveState moveState;

    private Vector2 touchPos;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        mainCam = Camera.main;
        moveState = MoveState.Idling;

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for screen touches
        if (Input.touches.Length > 0 && alive)
        {
            foreach (Touch touch in Input.touches)
            {
                //Check if touch is within bottom third of screen
                if (touch.position.y > Screen.height / 3f)
                    continue;

                //Update player positions and states based on touch positions
                if (touch.phase == TouchPhase.Began)
                    SetPosition(touch.position);
                else if (touch.phase == TouchPhase.Stationary)
                    MovePlayer();
                else if (touch.phase == TouchPhase.Moved)
                {
                    SetPosition(touch.position);
                    MovePlayer();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    moveState = MoveState.Idling;
                    anim.SetTrigger("Idle");
                }
            }
        }

        //Debug overrides for mouse input
        if (debugWithMouse && alive)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                SetPosition(mainCam.WorldToScreenPoint(transform.position + Vector3.right));
                MovePlayer();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                SetPosition(mainCam.WorldToScreenPoint(transform.position + Vector3.left));
                MovePlayer();
            }
            else if (moveState != MoveState.Idling)
            {
                moveState = MoveState.Idling;
                anim.SetTrigger("Idle");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Checks if collided with an asteroid
        if (collider.tag == "Asteroid" && alive)
        {
            alive = false;
            moveState = MoveState.Dead;

            anim.SetTrigger("Dead");
            StartCoroutine(DeathDelay());
        }
    }

    /// <summary>
    /// Sets and stores first touch position (in pixels) into world space
    /// </summary>
    /// <param name="newPosition"></param>
    private void SetPosition(Vector2 newPosition)
    {
        touchPos = mainCam.ScreenToWorldPoint(newPosition);

        //Checks facing direction of player
        if (transform.position.x <= touchPos.x)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180f, 0);
    }
    /// <summary>
    /// Updates the players position based on the most recently store touch position
    /// </summary>
    private void MovePlayer()
    {
        if (moveState != MoveState.Running)
        {
            anim.SetTrigger("Run");
            moveState = MoveState.Running;
        }

        if (transform.position.x > touchPos.x)
            transform.localPosition += Vector3.left * Time.deltaTime * moveSpeed;
        else if (transform.position.x < touchPos.x)
            transform.localPosition += Vector3.right * Time.deltaTime * moveSpeed;
        else if(moveState != MoveState.Idling)
        { 
            moveState = MoveState.Idling;
            anim.SetTrigger("Idle");
        }

        //Clamp player X direction
        if (transform.position.x < MIN_RUN_X)
            transform.position = new Vector3(MIN_RUN_X, transform.position.y, transform.position.z);
        else if (transform.position.x > MAX_RUN_X)
            transform.position = new Vector3(MAX_RUN_X, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Adds delay for death animation before taking a hit
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);

        manager.PlayerHit();
    }
}
