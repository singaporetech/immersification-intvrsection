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
\file GiveWay.cs
\brief Class to describe vehicle's giving way logic using trigger volumes
*/

public class GiveWay : MonoBehaviour
{
    [HideInInspector] public bool carIsPresent = false;                         /**If a car is present inside this trigger volume*/
    [HideInInspector] public List<GameObject> cars = new List<GameObject>();    /**The list of car objects inside this trigger volume*/

    /*!
    \fn OnTriggerEnter(Collider other);
    \brief Function for adding new cars entering the trigger volume to the list
    \param other The car's collider
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Auto Car") ||
            other.CompareTag("Normal Car"))
        {
            cars.Add(other.gameObject);
        }
    }

    /*!
    \fn OnTriggerStay(Collider other);
    \brief Function for sensing that there are still cars within this trigger volume
    \param other The car's collider
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Auto Car") ||
            other.CompareTag("Normal Car"))
        {
            carIsPresent = true;
        }
    }

    /*!
    \fn OnTriggerExit(Collider other);
    \brief Function for removing cars exiting the trigger volume from the list
    \param other The car's collider
    */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Auto Car") ||
            other.CompareTag("Normal Car"))
        {
            carIsPresent = false;
            cars.Remove(other.gameObject);
        }
    }
}
