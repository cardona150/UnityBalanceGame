using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float windForceMaximumMagnitude;
    public float secondsBetweenWindChange = 1;

    private float secondsSinceLastWindChange;
    private float windForce;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //transform.position = new Vector3(0, 0, transform.position.z);
    }

    private void FixedUpdate()
    {
        // Accumulate the amount of seconds since last FixedUpdate
        secondsSinceLastWindChange += Time.fixedDeltaTime;

        //If it's been long enough, generate a new random wind force
        if (secondsSinceLastWindChange > secondsBetweenWindChange)
        {
            windForce = UnityEngine.Random.Range(-windForceMaximumMagnitude, windForceMaximumMagnitude);
            //windForce = 0;

            // Reset the accumulator
            secondsSinceLastWindChange = 0;
        }

        // This will always be between -1 and 1
        var horizontalAxisForce = Input.GetAxis("Horizontal");

        // The more tilted the player is the stronger their rebalance should be
        // Use this help factor to achieve it
        float boost = 0;

        // In Unity euler angles are
        //     0/360
        //       ↑
        //  90 ←---→ 270
        //       ↓ 
        //      180
        if (0 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 90)
        {
            // Divide the angle by something because a boost of more than 45 is too strong
            //boost = (transform.rotation.eulerAngles.z) / 5;
            boost = (transform.rotation.eulerAngles.z) / 5;
        }
        else if (270 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 360)
        {
            // Divide the angle by something because a boost of more than 45 is too strong
            //boost = (360 - transform.rotation.eulerAngles.z) / 5;
            boost = (360 - transform.rotation.eulerAngles.z) / 5;
        }

        // Calculate the final vorce vector
        var finalForceVector3 = new Vector3((horizontalAxisForce * boost) + windForce, 0.0f, 0.0f);

        // Actually add the force
        rb.AddRelativeForce(finalForceVector3);
    }
}