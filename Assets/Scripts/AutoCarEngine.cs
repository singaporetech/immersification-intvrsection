using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Copyright 2023 Singapore Institute of Technology 
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*!
\file AutoCarEngine.cs
\brief Class to describe the operations of autonomous vehicles
*/

public class AutoCarEngine : CarEngine
{
    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        currentNode = 0;
        //Car speed measurements
        maxSteerAngle = 60f;
        initialSpeed = -200f;

        //27000 is normal brake 2700 is ebrake
        slowSpeed = maxSpeed * 0.4f;
        brakeSpeed = 900f;
        currentMaxSpeed = maxSpeed;
        maxBrakeSpeed = 6800f;
        trafficDist = 1.5f;

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;

        SetPathNodes();

        wheelFL = FLWheel.GetComponent<WheelCollider>();
        wheelFR = FRWheel.GetComponent<WheelCollider>();
        wheelRL = RLWheel.GetComponent<WheelCollider>();
        wheelRR = RRWheel.GetComponent<WheelCollider>();

        horn = gameObject.GetComponent<AudioSource>();
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    private void Update()
    {
        if (transform.position.y > 2) 
        {
            gameObject.SetActive(false);
        }

        if (path != null)
        {
            ApplySteer();
            CheckWaypointDistance();
        }
        CarBrakeLogic();
        CarHorningLogic();
        WheelUpdate();
    }
}
