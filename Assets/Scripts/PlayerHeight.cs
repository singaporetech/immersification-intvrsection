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
\brief Class for controlling the height of the player based on the ground collider
*/

public class PlayerHeight : MonoBehaviour
{

    /*!
    \fn Update();
    \brief Update is called once per frame
    */
    void Update()
    {
        Vector3 position = transform.position;
        position.y += 1.0f;

        RaycastHit hit;
        bool h = Physics.Raycast(position, Vector3.down, out hit, 5.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        if (h)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y;

            transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
        }
    }
}
