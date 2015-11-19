using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartAI : MonoBehaviour {

    // default behaviors
    public float default_accel_rate;    // default accel rate
    public float default_turn_speed;    // default turn speed
    public float default_max_speed;     // default max speed
    public float wheel_grip             // wheel grip -min 0  -max 1
    {
        get { return this.real_grip; }
        set { this.real_grip = Mathf.Clamp01(value); }
    }
    public float max_wheel_turn;        // maximum wheel angle
    public float wheel_turn_speed;      // wheel turn speed
    public bool isPlayer;               // whether or not the car is a player

    private Vector2 exitpoint;          // exitpoint for the AI
    private float accel_rate;           // REAL accel rate
    private float turn_speed;           // REAL turn speed
    private float max_speed;            // REAL max speed
    private float real_grip;            // REAL grip

    private float axis_horiz = 0f;      // horizontal input
    private float axis_vert = 0f;       // vertical iput
    private GameObject exitObj;         // the gameobject of this car
    private Quaternion quat;            // the quaternion for turning
    private Rigidbody2D rb;             // this object's rigid body
    private float EPSILON = 0.4f;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        // set the Interanl variables
        accel_rate = default_accel_rate;
        turn_speed = default_turn_speed;
        max_speed = default_max_speed;
        real_grip = wheel_grip;

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
        // MOVE CAR
        if (axis_vert != 0) { rb.AddForce(transform.up * accel_rate * axis_vert * real_grip); }

        // ENFORCE SPEED LIMIT
        if (rb.velocity.magnitude > max_speed)
        {
            rb.velocity = rb.velocity.normalized * max_speed;
        }

        // ROTATE WHEEL
        Transform wheel = transform.FindChild("Wheel");
        float testangle = wheel.localEulerAngles.z + (Time.deltaTime * -axis_horiz * wheel_turn_speed);
        Debug.Log("test: " + testangle.ToString());

        wheel.Rotate(0,0,Time.deltaTime * -axis_horiz * wheel_turn_speed);

        float anglez = wheel.localEulerAngles.z;

        if (anglez >= 0 && anglez < 180)
        {
            anglez = Mathf.Clamp(anglez,0,max_wheel_turn);
        }
        else
        {
            anglez = Mathf.Clamp(anglez,360-max_wheel_turn-1,360);
        }

        wheel.localEulerAngles = new Vector3(0,0,anglez);

       // CAR ROTATION

        //transform.Rotate(Vector3.forward, -axis_horiz * turn_speed * Time.deltaTime);

        // rotate car if moving
        if (rb.velocity.x != 0  || rb.velocity.y != 0)
        {
            if (isPlayer)
            {
                transform.Rotate(Vector3.forward, (wheel.localEulerAngles.z * Time.deltaTime));
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
            this.accel_rate = this.accel_rate * (float)-1.5;
            Invoke("return_default_speed",2);
        }
        else if (col.gameObject.CompareTag("Spill"))
        {
            wheel_grip = 0.3f;
        }
    }

    /// <summary>
    /// Returns the default speed
    /// </summary>
    public void return_default_speed()
    {
        accel_rate = default_accel_rate;
    }
}
