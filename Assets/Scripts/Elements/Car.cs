using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public WheelCollider[] guideWheels;

    float direction = 0f;
    float acceleration = 0f;
    float speedKm;
    public float force;

    Rigidbody car;

    public AudioClip carClip;
    public AudioSource carAudioSource;

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
		for (int i=0 ; i<guideWheels.Length ; i++)
        {
            guideWheels[i].steerAngle = direction * 15f;
            guideWheels[i].motorTorque = 1f;
        }

        speedKm = car.velocity.magnitude * 3.6f;

        car.AddForce(transform.forward * force * acceleration);

        carAudioSource.pitch = 0.6f + speedKm / 60f;
    }
}
