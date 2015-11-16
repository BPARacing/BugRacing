using UnityEngine;
using System.Collections;

public class StartAI : MonoBehaviour {

    // default behaviors
    public float default_accel_rate;   // default accel rate
    public float default_turn_speed;    // default turn speed
    public float default_max_speed;     // default max speed
    public float default_wheel_grip;    // min 0 - max 1
    public bool isPlayer;               // whether or not the car is a player

    private Vector2 exitpoint;          // exitpoint for the AI
    private float move_speed;           // REAL accel rate
    private float turn_speed;           // REAL turn speed
    private float max_speed;            // REAL max speed

    private float axis_horiz = 0f;      // horizontal input
    private float axis_vert = 0f;       // vertical iput
    private GameObject exitObj;         // the gameobject of this car
    private Quaternion quat;            // the quaternion for turning
    private Rigidbody2D rb;             // this object's rigid body

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        // set the Interanl variables
        move_speed = default_accel_rate;
        turn_speed = default_turn_speed;
        max_speed = default_max_speed;

        // set the exit point
        rb = GetComponent<Rigidbody2D>();

        // get goal point
        exitObj = GameObject.FindGameObjectWithTag("Exit");
        exitpoint = new Vector2(exitObj.transform.position.x, exitObj.transform.position.y);
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
        Vector3 vectorToTarget = exitObj.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Debug.Log(transform.gameObject.name +"\nVector is: " + vectorToTarget.ToString() + "\nAngle is:" + angle.ToString());
        quat = Quaternion.AngleAxis(angle - 90, Vector3.forward);
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
                transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * turn_speed);
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
            Invoke("return_default_speed",2);
        }
        else if (col.gameObject.CompareTag("Spill"))
        {
            
        }
    }

    /// <summary>
    /// Returns the default speed
    /// </summary>
    public void return_default_speed()
    {
        move_speed = default_accel_rate;
    }
}
