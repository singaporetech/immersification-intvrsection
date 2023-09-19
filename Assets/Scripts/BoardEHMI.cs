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
\file BoardEHMI.cs
\brief Class to control the signboard for Instruction EHMI
*/

public class BoardEHMI : MonoBehaviour
{
    public CarEngine carEngine;
    public GameObject StopBoard;
    public GameObject GreenManBoard;

    /*!
    \fn OnEnable();
    \brief Called when Object is enabled
    */
    void OnEnable()
    {
        StopBoard.SetActive(true);
        GreenManBoard.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    void Update()
    {
        //If car is moving set board to Stop, if not set board to GreenMan
        if (carEngine.actualSpeed > 1)
        {
            StopBoard.SetActive(true);
            GreenManBoard.SetActive(false);
        }
        else
        {
            
            StopBoard.SetActive(false);
            GreenManBoard.SetActive(true);
        }
    }
}
