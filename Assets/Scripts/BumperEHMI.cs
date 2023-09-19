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
\file BumperEHMI.cs
\brief Class to control the light-up bumper for Intention EHMI
*/

namespace bumperLightEHMI
{
    class BumperEHMI : MonoBehaviour
    {
        public CarEngine carEngine;
        [SerializeField] SensorBoundingBox sensorNear;
        [SerializeField] SensorBoundingBox sensorFar;

        Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
        }

        /*!
        \fn Update();
        \brief Called once per frame
        */
        void Update()
        {
            //If car is moving, set the lights according to it's current speed against max speed.
            if (carEngine.actualSpeed > 0.1)
            {
                if (carEngine.actualSpeed >= carEngine.maxSpeed)
                {
                    rend.material.mainTextureOffset = new Vector2(0, 0);
                }
                else 
                {
                    float offset = -0.55f - ((carEngine.actualSpeed / carEngine.maxSpeed) * -0.55f);
                    rend.material.mainTextureOffset = new Vector2(offset, 0);
                }
            }
            //If car is not moving, set the bumper to be completely lighted up.
            else 
            {
                rend.material.mainTextureOffset = new Vector2(-0.55f, 0);
            }
        }

    }
}