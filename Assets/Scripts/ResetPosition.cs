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
\file ResetPosition.cs
\brief Class to reset the player's position at the start of the scene.
*/

public class ResetPosition : MonoBehaviour
{
    [SerializeField] Transform resetTransform;
    [SerializeField] GameObject player;
    [SerializeField] Camera playerHead;

    /*!
    \fn OnEnable()
    \brief Function to reset player position and orientation.
    */
    public void PositionReset() 
    {
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
        player.transform.Rotate(0, rotationAngleY, 0);

        var distanceDiff = resetTransform.position - playerHead.transform.position;

        player.transform.position += new Vector3(distanceDiff.x, 0, distanceDiff.z);
    }

    /*!
    \fn Update()
    \brief Called every frame
    */
    private void Update()
    {
        //Position Reset is mapped to 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            PositionReset();
        }
    }
}
