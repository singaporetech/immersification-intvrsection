using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
\file SpawnpointList.cs
\brief Class for the dropdown UI for spawnpoint selection
*/

public class SpawnpointList : MonoBehaviour
{
    /*!
    \fn Start();
    \brief Start is called before the first frame update
    */
    void Start()
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        GameObject carpoolObject = transform.parent.GetComponent<UICustomSpawn>().carpool;
        CarPool carpool = carpoolObject.GetComponent<CarPool>();

        if (carpool.spawnPoints.Length <= 0)
            return;

        for (int i = 0; i < carpool.spawnPoints.Length; ++i)
        {
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = "Spawnpoint " + (i + 1);
            dropdown.options.Add(newData);
        }

        Text label = transform.GetChild(0).GetComponent<Text>();
        label.text = "Spawnpoint 1";
    }
}
