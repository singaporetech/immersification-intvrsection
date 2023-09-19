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
\file CarSpawnPoint.cs
\brief Class to describe the car spawn point logic
*/

public class CarSpawnPoint : MonoBehaviour
{
    public GameObject path;                                                 /**The path cars the spawn at this spawn point should follow*/
    public Color colour;                                                    /**Colour for the Gizmo*/

    public GameObject trafficLight;                                         /**Traffic light along this spawn point's path*/
    [HideInInspector] public bool blocked = false;                          /**Bool for if this spawnpoint is blocked by another collision body*/

    public GameObject zebraCrossing;

    public GameObject[] giveWayCheck;                                       /**Array of bounding volumes to check to give way to, assigned in the inspector*/
    [HideInInspector] public List<GiveWay> giveWay = new List<GiveWay>();   /**List of bounding volumes to check to give way to, to be passed on to any car spawning at this spawnpoint*/

    public bool turn = false;                                               /**If there is an intersection turn along this path*/
    public bool automaticSpawning = true;                                   /**If the spawnpoint automatically spawns cars using the spawning algorithm*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        if (giveWayCheck == null)
            return;

        foreach (GameObject go in giveWayCheck)
            giveWay.Add(go.GetComponent<GiveWay>());
    }

    /*!
    \fn OnDrawGizmos();
    \brief Draws Gizmo to show the spawn point facing direction
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = colour;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward);
    }

    /*!
    \fn OnTriggerStay(Collider other);
    \brief Function for triggering if the spawnpoint is blocked.
        There is another collider intersection the trigger bounds.
    \param other The object's collider
    */
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Auto Car") ||
            collider.CompareTag("Normal Car"))
        {
            blocked = true;
        }
    }

    /*!
    \fn OnTriggerExit(Collider other);
    \brief Function for triggering if the spawnpoint is not blocked.
        Another collider has left the trigger bounds.
    \param other The object's collider
    */
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Auto Car") ||
            collider.CompareTag("Normal Car"))
        {
            blocked = false;
        }
    }
}
