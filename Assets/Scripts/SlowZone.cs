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
\file SlowZone.cs
\brief Class to define a zone for cars to regulate a slower speed 
*/

public class SlowZone : MonoBehaviour
{
    [SerializeField] private float modifier = 1.0f;

    /*!
    \fn OnTriggerStay(Collider other);
    \brief Function for triggering vehicles to start slowing down and maintaining a slow speed
    \param other The car's collider
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Auto Car" ||
            other.gameObject.tag == "Normal Car")
        {
            CarEngine e = other.GetComponent<CarEngine>();
            float speed = other.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            if (speed > e.slowSpeed * modifier)
            {
                if (!e.willTurn)
                    return;

                e.currentMaxSpeed = e.slowSpeed * modifier;
                e.CarBraking();
            }
        }
    }

    /*!
    \fn OnTriggerExit(Collider other);
    \brief Function for triggering vehicles to start speeding back up to their original speed
    \param other The car's collider
    */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Auto Car" ||
            other.gameObject.tag == "Normal Car")
        {
            CarEngine e = other.GetComponent<CarEngine>();

            if (!e.willTurn)
                return;

            e.currentMaxSpeed = e.maxSpeed;
        }
    }
}
