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
\file TimerWalkAcrossRoad.cs
\brief Class implementing a timer to measure the time taken from the car braking to the player crossing the road successfully
*/

public class TimerWalkAcrossRoad : MonoBehaviour
{
    public static TimerWalkAcrossRoad instance;     /**The singleton instance of this class*/

    float timer = 0.0f;                             /**The time elapsed when timing*/
    bool isTiming = false;                          /**If the timer is currently running*/

    /*!
    \fn Awake();
    \brief Called when the script instance is being loaded
    */
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;
        }
    }

    /*!
    \fn StartTimer();
    \brief Function to start the timer
    */
    public void StartTimer()
    {
        timer = 0.0f;
        isTiming = true;
    }

    /*!
    \fn StopTimer();
    \brief Function to stop the timer
    */
    public void StopTimer()
    {
        isTiming = false;

        if (!Logger.instance)
            return;

        Logger.instance.Log("Time To Cross Road", timer);
    }
}
