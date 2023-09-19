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
\file GuidingRespawnArrow.cs
\brief Class for defining the logic for the guidance arrows in the death transition mode
*/

public class GuidingRespawnArrow : MonoBehaviour
{
    [SerializeField] Camera mCam;           /**The main camera object*/
    [SerializeField] Transform target;      /**The current target transform to point to*/
    [SerializeField] GameObject leftArrow;  /**The arrow on the left side of the text*/
    [SerializeField] GameObject rightArrow; /**The arrow on the right side of the text*/

    /*!
    \fn Update();
    \brief Called once per frame
    */
    void Update()
    {
        float angle = Vector3.Angle(mCam.transform.forward, target.position - transform.position);
        if (angle <= 60.0f)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
            return;
        }

        Vector3 d = target.transform.position - mCam.transform.position;
        d = Vector3.Normalize(d);
        float dir = Vector3.Dot(d, mCam.transform.right);
        if (dir >= 0.0f)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
    }
}
