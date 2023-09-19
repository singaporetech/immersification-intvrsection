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
\file SensorController.cs
\brief Controller class for enabling the required sensors on normal cars based on the scenario
*/

public class SensorController : MonoBehaviour
{
    public GameObject sensorNear;           /**The near sensor*/
    public GameObject sensorMiddle;         /**The far sensor*/
    public GameObject sensorUnsigMiddle;    /**The largest sensor region infront of the car*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    void Start()
    {
        //Set the sensorUnsigMiddle to false when the scene contains traffic light
        bool containsTrafficLight = false;
        if (GameObject.FindGameObjectWithTag("Traffic Light"))
            containsTrafficLight = true;

        if (containsTrafficLight == true)
        {
            sensorNear.SetActive(true);
            sensorMiddle.SetActive(true);
            sensorUnsigMiddle.SetActive(false);
        }
        else
        {
            sensorNear.SetActive(true);
            sensorMiddle.SetActive(true);
            sensorUnsigMiddle.SetActive(true);
        }
    }

}
