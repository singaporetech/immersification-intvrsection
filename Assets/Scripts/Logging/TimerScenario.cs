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
\file TimerScenarioCompletion.cs
\brief Class implementing a timer to measure the time taken to complete the scenario successfully
*/

public class TimerScenario : MonoBehaviour
{
    public static TimerScenario instance;               /**The singleton instance of this class*/
    float time;                                         /**The current time elapsed in seconds*/
    bool isTiming = true;                               /**If the timer is currently running*/

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

        time = 0.0f;
        StartCoroutine(Timer());
    }

    /*!
    \fn StopTimer();
    \brief Function to stop the timer
    */
    public void StopTimer()
    {
        if (isTiming)
            StopCoroutine(Timer());
        isTiming = false;
    }

    /*!
    \fn TimeStamp();
    \brief Function to log the timestamp
    */
    void TimeStamp()
    {
        if (!Logger.instance)
            return;

        Logger.instance.Log("Time", time);
    }

    /*!
    \fn Timer();
    \brief Coroutine function to log every second
    */
    IEnumerator Timer()
    {
        TimeStamp();
        yield return new WaitForSeconds(1.0f);
        time += 1.0f;
        StartCoroutine(Timer());
    }
}
