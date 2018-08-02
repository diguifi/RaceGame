using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public WheelCollider[] guideWheels;

    float direction = 0f;
    float acceleration = 0f;
    float speedKmh;
    public float force;
    public float breakForce;
    public float breakInstability;
    public float maxTorque;

    Rigidbody car;

    public AnimationCurve wheelCurve;

    public AudioClip carClip;
    public AudioSource carAudioSource;
    public AudioSource screechAudioSource;

    float rpm;
    public float[] gears;
    int actualGear = 0;

    public float maxRpm;
    public float minRpm;

    Vector3 finalForce;
    public Transform centerOfMass;

    public float soundPitch;

    void Start()
    {
        car = GetComponent<Rigidbody>();
        car.centerOfMass = centerOfMass.position;
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
            guideWheels[i].steerAngle = direction * wheelCurve.Evaluate(speedKmh);
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
        if (acceleration < -0.5f)
        {
            car.AddForce(-transform.forward * breakForce);
            car.AddTorque((transform.up * breakInstability * speedKmh / 45f) * direction);

            acceleration = 0;
        }
        finalForce = transform.forward * ((maxTorque / (actualGear + 1)) + maxTorque / 1.85f) * acceleration;
        car.AddForce(finalForce);

        // Sound
        carAudioSource.pitch = rpm / soundPitch;

        if (speedKmh >= 30f)
        {
            float angle = Vector3.Angle(transform.forward, car.velocity);
            float finalValue = (angle / 10f) - 0.3f;

            screechAudioSource.volume = Mathf.Clamp(finalValue, 0f, 1f);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 120, 32), rpm + " RPM");
        GUI.Label(new Rect(20, 40, 120, 32), actualGear + 1 + "nd Gear");
        GUI.Label(new Rect(20, 60, 120, 32), speedKmh + " KM/h");
        //GUI.Label(new Rect(20, 80, 120, 32), finalForce.magnitude.ToString());
    }
}
