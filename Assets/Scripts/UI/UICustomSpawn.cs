using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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
\file UICustomSpawn.cs
\brief Class for the handling of the custom spawning of vehicles
*/

public class UICustomSpawn : MonoBehaviour
{
    public GameObject carpool;                  /**The Car Pool object in the scene*/
    int spawnpointIndex = 0;                    /**Index of the spawnpoint to spawn a vehicle at*/
    string carType;                             /**The type of vehicle to spawn*/

    public Dropdown spawnpointsListDropdown;    /**Dropdown UI component for the spawnpoints selection*/
    public Dropdown carTypesListDropdown;       /**Dropdown UI component for the car type selection*/

    /*!
    \fn Start();
    \brief Start is called before the first frame update
    */
    private void Start()
    {
        spawnpointsListDropdown.onValueChanged.AddListener(delegate{ SetSpawnpointIndex(); });
        carTypesListDropdown.onValueChanged.AddListener(delegate{ SetCarType(); });
        carType = carTypesListDropdown.GetComponent<CarTypesList>().carTypes[0];
    }

    /*!
    \fn OnDisable();
    \brief Called when this object is disabled
    */
    private void OnDisable()
    {
        //free the callback listeners 
        spawnpointsListDropdown.onValueChanged.RemoveAllListeners();
        carTypesListDropdown.onValueChanged.RemoveAllListeners();
    }

    /*!
    \fn Spawn();
    \brief Calls the TrySpawn function in the carpool script
    */
    public void Spawn()
    {
        bool isAuto = false;
        if (carType == "Auto Car")
            isAuto = true;

        carpool.GetComponent<CarPool>().TrySpawnCar(spawnpointIndex, isAuto);
    }

    /*!
    \fn SetSpawnpointIndex();
    \brief DropDown onValueChanged listener function. Called when the user interacts with the spawnpointsListDropdown UI
    */
    public void SetSpawnpointIndex()
    {
        spawnpointIndex = spawnpointsListDropdown.value;
    }

    /*!
    \fn SetCarType();
    \brief DropDown onValueChanged listener function. Called when the user interacts with the carTypesListDropdown UI
    */
    public void SetCarType()
    {
        carType = carTypesListDropdown.GetComponent<CarTypesList>().carTypes[carTypesListDropdown.value];
    }
}
