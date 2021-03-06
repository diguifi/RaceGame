﻿using UnityEngine;
using Diguifi.wheelsManager;

namespace Diguifi.CarElement
{
    public class Car : MonoBehaviour
    {
        [SerializeField]
        WheelCollider[] guideWheels;
        WheelsManager[] wheels;

        float direction = 0f;
        float acceleration = 0f;
        float speedKmh;
        [SerializeField]
        float force;
        [SerializeField]
        float breakForce;
        [SerializeField]
        float breakInstability;
        [SerializeField]
        float maxTorque;

        Rigidbody car;

        [SerializeField]
        int turningStrength;

        [SerializeField]
        AudioClip carClip;
        [SerializeField]
        AudioClip skidRoad;
        [SerializeField]
        AudioClip skidGrass;

        [SerializeField]
        AudioSource carAudioSource;
        [SerializeField]
        AudioSource screechAudioSource;

        float rpm;
        [SerializeField]
        float[] gears;
        int actualGear = 0;

        [SerializeField]
        float maxRpm;
        float minRpm;

        Vector3 finalForce;
        [SerializeField]
        Transform centerOfMass;

        [SerializeField]
        float soundPitch;

        void Start()
        {
            car = GetComponent<Rigidbody>();
            car.centerOfMass = centerOfMass.position;
            carAudioSource.clip = carClip;
            screechAudioSource.volume = 0;

            wheels = new WheelsManager[guideWheels.Length];
            for (int i = 0; i < guideWheels.Length; i++)
            {
                wheels[i] = guideWheels[i].GetComponent<WheelsManager>();
            }
        }

        void Update()
        {
            direction = Input.GetAxis("Horizontal");
            acceleration = Input.GetAxis("Vertical");
        }

        void FixedUpdate()
        {
            // Turn right/left
            for (int i = 0; i < guideWheels.Length; i++)
            {
                if(turningStrength > 350 && speedKmh < 5)
                    guideWheels[i].steerAngle = direction * 100 / speedKmh;
                else
                    guideWheels[i].steerAngle = direction * turningStrength/speedKmh;
                guideWheels[i].motorTorque = 1f;

                if (wheels[i].currentWheel != 0)
                {
                    car.AddTorque((transform.up * (breakInstability / 1.5f) * speedKmh / 45f) * direction);

                    if (screechAudioSource.clip != skidGrass)
                    {
                        screechAudioSource.clip = skidGrass;
                        screechAudioSource.Play();
                    }
                }
                else
                {
                    if (screechAudioSource.clip != skidRoad)
                    {
                        screechAudioSource.clip = skidRoad;
                        screechAudioSource.Play();
                    }
                }
            }

            // Speed and RPM
            speedKmh = car.velocity.magnitude * 3.6f;
            rpm = speedKmh * gears[actualGear] * 15f;

            // Gear change
            if (rpm > maxRpm)
            {
                if(acceleration > 0)
                    actualGear++;

                if (actualGear == gears.Length)
                {
                    actualGear--;
                }
            }
            if (rpm < minRpm)
            {
                actualGear--;
                if (actualGear < 0)
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
        }
    }
}
