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
\file SensorBoundingBox.cs
\brief Class describing the sensor bounds reations for vehicles
*/

public class SensorBoundingBox : MonoBehaviour
{
    [HideInInspector] public bool isPlayer; /**If the player is present in the bounding box*/
    [HideInInspector] public bool isZC = false; /**If the bounding box has hit a Zebra Crossing*/

    /*!
    \fn OnTriggerEnter(Collider other);
    \brief Function for triggering if the player steps into this bounding box
    \param other The object's collider
    */
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayer = true;

        if (other.CompareTag("ZebraCrossing"))
        {
            isZC = true;
        }
    }

    /*!
    \fn OnTriggerExit(Collider other);
    \brief Function for triggering if the player steps out of this bounding box
    \param other The object's collider
    */
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayer = false;

        if (other.CompareTag("ZebraCrossing"))
        {
            isZC = false;
        }
    }
}
