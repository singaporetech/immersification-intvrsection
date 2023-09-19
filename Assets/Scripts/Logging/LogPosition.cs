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
\file LogPosition.cs
\brief Class to Log position data to file
*/

public class LogPosition : MonoBehaviour
{
    bool isLogging = true;                               /**If the timer is currently running*/

    /*!
    \fn Start();
    \brief Called once in the first frame 
    */
    private void Start()
    {
        StartCoroutine(Timer());
    }

    /*!
    \fn StopTimer();
    \brief Function to stop the timer
    */
    public void StopTimer()
    {
        if (isLogging)
            StopCoroutine(Timer());
        isLogging = false;
    }

    /*!
    \fn LogData();
    \brief Function to log the position data
    */
    void LogData()
    {
        if (!Logger.instance)
            return;

        Logger.instance.Log("(" + gameObject.name + ") Position", transform.position);
    }

    /*!
    \fn Timer();
    \brief Coroutine function to log every second
    */
    IEnumerator Timer()
    {
        LogData();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Timer());
    }
}
