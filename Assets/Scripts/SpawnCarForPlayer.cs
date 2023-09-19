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

/*
\file SpawnCarForPlayer.cs
\brief Class implementing logic to spawn a car for the player automatically when they are about to cross the road
*/

public class SpawnCarForPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerCamera;       /**The player camera*/
    [SerializeField] CarPool carPool;               /**The Car Pool object in the scene*/
    [SerializeField] int spawnpointIndex;           /**The index of the spawnpoint at which to spawn the car at*/

    [HideInInspector] public bool spawned = false;  /**If a car has already been spawned*/
    //bool carBraking = false;                        /**If the car has started braking*/
    GameObject car = null;                          /**Cache variable for the car spawned*/
    public Vector3 crossingPosition;                /**Position of the crossing in the scene*/

    /*!
    \fn OnTriggerStay(Collider other);
    \brief Function for triggering a vehicle to spawn
        a car is only spawned when the spawnpoint is out of the FOV of the player
    \param other The object's collider
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawned)
                return;

            Vector3 d = carPool.spawnPoints[spawnpointIndex].transform.position - playerCamera.transform.position;
            float angle = Vector3.Angle(d, playerCamera.transform.forward);
            if (angle > 60.0f)
            {
                car = carPool.SpawnCar(spawnpointIndex, true);
                spawned = true;
            }
        }
    }

    /*!
    \fn OnTriggerEnter(Collider other);
    \brief Function for triggering the traffic light reaction timer to stop
    \param other The object's collider
    */
    private void OnTriggerEnter(Collider other)
    {

    }

    /*!
    \fn OnDrawGizmos();
    \brief This function draws debugging gizmos in the editor
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(crossingPosition, 0.1f);
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    private void Update()
    {
        /* 31/01/23 Ben - Disabled for now, Very weird way to start tracking when player cross the street */
        /*
        if (!car || carBraking)
            return;

        if (car.GetComponent<CarEngine>().isBraking)
        {
            TimerWalkAcrossRoad.instance.StartTimer();
            carBraking = true;

            TimerCarDeceleration.instance.StartTimer(car);

            Vector3 d = crossingPosition - car.transform.position;
            Logger.instance.Log("Braking Distance", d.magnitude);
        }
        */
    }
}
