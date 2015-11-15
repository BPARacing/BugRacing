using UnityEngine;
using System.Collections;

public class StartAI : MonoBehaviour {

    public float default_move_speed;
    public float default_turn_speed;

    public Vector2 exitpoint;
    public bool isPlayer;
    private float move_speed;
    private float turn_speed;

    private float axis_horiz = 0f;
    private float axis_vert = 0f;
    private GameObject gb;
    private Quaternion q;

    private Rigidbody2D rb;


    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        //set the Interanl variables
        move_speed = default_move_speed;
        turn_speed = default_turn_speed;

        //set the exit point
        rb = GetComponent<Rigidbody2D>();

        // get goal point
        gb = GameObject.FindGameObjectWithTag("Hazard");
        exitpoint = new Vector2(gb.transform.position.x, gb.transform.position.y);

        //TODO: Place Car
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update ()
    {
        // if player, control
        if (isPlayer)
        {
            axis_horiz = Input.GetAxis("Horizontal");
            axis_vert = Input.GetAxis("Vertical");
        }
    }


    /// <summary>
    /// Fixed Update
    /// </summary>
    void FixedUpdate () {
        if (!isPlayer)
        {
            angleTowardsGoal();
        }
        motion();
    }

    /// <summary>
    /// Angle car towards goal
    /// </summary>
    void angleTowardsGoal()
    {
        Vector3 vectorToTarget = gb.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    /// <summary>
    /// Handles the motion from input
    /// </summary>
    void motion()
    {
        // move forward
        if (axis_vert != 0) { rb.AddForce(transform.up * move_speed * axis_vert); }

        // rotate if moving
        if (rb.velocity.x != 0  || rb.velocity.y != 0)
        {
            if (isPlayer)
            {
                transform.Rotate(Vector3.forward, -axis_horiz * turn_speed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turn_speed);
            }
        }
    }

    /// <summary>
    /// Called on 2D collision
    /// </summary>
    /// <param name="col">The collider</param>
    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("Hazard"))
        {
            this.move_speed = this.move_speed * (float)-1.5;
        }
        else if (col.gameObject.CompareTag("Spill"))
        {
            
        }
    }

    public void return_default_speed()
    {
        move_speed = default_move_speed;
    }
}
