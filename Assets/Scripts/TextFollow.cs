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
\file TextFollow.cs
\brief Class for making Text smoothly follow within the player's FOV
*/

public class TextFollow : MonoBehaviour
{
    public float yPos;          /**The height of the text*/
    public Camera mCam;         /**The main camera object*/

    /*!
    \fn Update();
    \brief Update is called once per frame
    */
    void Update()
    {
        Vector3 newPosition = mCam.transform.position + mCam.transform.forward * 3.0f;
        newPosition.y = yPos;
        transform.position = newPosition;
        Vector3.Lerp(transform.position, newPosition, 0.1f);
        transform.rotation = Quaternion.LookRotation(mCam.transform.forward);
    }
}
