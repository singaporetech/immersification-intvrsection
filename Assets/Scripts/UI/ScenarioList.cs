using System.Collections;
using System.Collections.Generic;
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
\file ScenarioList.cs
\brief Class for the dropdown UI for scenario selection
*/

public class ScenarioList : MonoBehaviour
{
    /*!
    \fn Start();
    \brief Start is called before the first frame update
    */
    void Start()
    {
        Dropdown dropdown = gameObject.GetComponent<Dropdown>();

        //Get all the scenes from the sorted scenedictionary and convert to an array
        List<string> sceneList = new List<string>();
        foreach(KeyValuePair<string, string> scene in SceneController.instance.SceneDictionary)
        {
            sceneList.Add(scene.Value);
        }
        string[] scenarios = sceneList.ToArray();

        if (scenarios.Length <= 0)
            return;

        foreach (string scenario in scenarios) 
        { 
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = scenario;
            dropdown.options.Add(newData);
        }
        
        {
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = "Blank";
            dropdown.options.Add(newData);
        }
        
        dropdown.value = scenarios.Length;
    }
}
