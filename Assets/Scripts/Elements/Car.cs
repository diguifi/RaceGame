using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public WheelCollider[] guideWheels;

    float direction = 0f;
    float acceleration = 0f;
    float speedKmh;
    public float force;

    Rigidbody car;

    public AudioClip carClip;
    public AudioSource carAudioSource;

    float rpm;
    public float[] gears;
    int actualGear = 0;

    public float maxRpm;
    public float minRpm;

    public float soundPitch;

    void Start()
    {
        car = GetComponent<Rigidbody>();
        carAudioSource.clip = carClip;
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        acceleration = Input.GetAxis("Vertical");
    }

    void FixedUpdate ()
    {
        // Turn right/left
		for (int i=0 ; i<guideWheels.Length ; i++)
        {
            guideWheels[i].steerAngle = direction * 15f;
            guideWheels[i].motorTorque = 1f;
        }

        // Speed and RPM
        speedKmh = car.velocity.magnitude * 3.6f;
        rpm = speedKmh * gears[actualGear] * 15f;

        // Gear change
        if (rpm > maxRpm)
        {
            actualGear++;
            if (actualGear == gears.Length)
            {
                actualGear--;
            }
        }
        if (rpm < minRpm)
        {
            actualGear--;
            if(actualGear < 0)
            {
                actualGear = 0;
            }
        }

        // Forces
        car.AddForce(transform.forward * force * acceleration);

        // Sound
        carAudioSource.pitch = rpm / soundPitch;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 120, 32), rpm + " RPM");
        GUI.Label(new Rect(20, 40, 120, 32), actualGear + 1 + "nd Gear");
        GUI.Label(new Rect(20, 60, 120, 32), speedKmh + " KM/h");
    }
}
