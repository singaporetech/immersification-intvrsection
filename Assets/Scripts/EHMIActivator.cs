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
\file EHMIActivator.cs
\brief Class to control set the EHMI of the AV to intention or instruction
*/

public class EHMIActivator : MonoBehaviour
{
    public GameObject EHMIIntention1;
    public GameObject EHMIIntention2;
    public GameObject EHMIInstruction;


    /*!
    \fn OnEnable();
    \brief Called when Object is enabled
    */
    void OnEnable()
    {
        /*Manually set ehmi if editing scene without going through start screen*/
        string ehmiName = SceneController.instance.getCurrentEHMIName();
        //string ehmiName = "Intention";
        //string ehmiName = "Instruction";
        //string ehmiName = "No_EHMI";
        if (ehmiName == "Intention")
        {
            EHMIIntention1.SetActive(true);
            EHMIIntention2.SetActive(true);
            EHMIInstruction.SetActive(false);
        }
        else if (ehmiName == "Instruction")
        {
            EHMIIntention1.SetActive(false);
            EHMIIntention2.SetActive(false);
            EHMIInstruction.SetActive(true);
        }
        else if (ehmiName == "No_EHMI") 
        {
            EHMIIntention1.SetActive(false);
            EHMIIntention2.SetActive(false);
            EHMIInstruction.SetActive(false);
        }
    }

    private void OnDisable()
    {
        EHMIIntention1.SetActive(false);
        EHMIIntention2.SetActive(false);
        EHMIInstruction.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
