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
\file UICustomScenarioOrder.cs
\brief Class for the handling of the custom scenario order, saves the user's selections into the SceneManager instance
*/

public class UICustomScenarioOrder : MonoBehaviour
{
    [HideInInspector] public List<Dropdown> scenarioListDropdowns = new List<Dropdown>();   /**List of scenario dropdown components present on the UI*/
    [HideInInspector] public List<Dropdown> ehmiListDropdowns = new List<Dropdown>();       /**List of ehmi dropdown components present on the UI*/
    [SerializeField] GameObject scenariosListButtonPrefab;                                  /**Prefab for the scenarios dropdown UI object*/
    [SerializeField] GameObject ehmisListButtonPrefab;                                      /**Prefab for the ehmis dropdown UI object*/
    float yPos;                                                                             /**The y position on UI window to place next the dropdown button at*/
    float buttonHeight;                                                                     /**The height of the button*/

    /*!
    \fn Start();
    \brief Start is called before the first frame update
    */
    void Start()
    {
        RectTransform rt_prefab = scenariosListButtonPrefab.GetComponent<RectTransform>();
        buttonHeight = rt_prefab.rect.height;
        yPos = -buttonHeight * 0.5f - buttonHeight;
    }

    /*!
    \fn AddDropdowns();
    \brief Function for when the plus button is clicked, to add dropdown options
    */
    public void AddDropdowns()
    {
        Transform contentWindow = transform.GetChild(0).GetChild(0);

        int buttonWidth = (Screen.width / 3);

        {
            GameObject newScenariosButton = Instantiate(scenariosListButtonPrefab, contentWindow);
            RectTransform rt = newScenariosButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(buttonWidth / 2, yPos);
            rt.sizeDelta = new Vector2(buttonWidth, rt.sizeDelta.y);
            scenarioListDropdowns.Add(newScenariosButton.GetComponent<Dropdown>());
        }

        {
            GameObject newEhmisButton = Instantiate(ehmisListButtonPrefab, contentWindow);
            RectTransform rt = newEhmisButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(buttonWidth / 2 + buttonWidth, yPos);
            rt.sizeDelta = new Vector2(buttonWidth, rt.sizeDelta.y);
            ehmiListDropdowns.Add(newEhmisButton.GetComponent<Dropdown>());
        }

        float contentWindowHeight = buttonHeight * scenarioListDropdowns.Count + 1;
        RectTransform rt_content = contentWindow.GetComponent<RectTransform>();
        rt_content.sizeDelta = new Vector2(rt_content.sizeDelta.x, contentWindowHeight);

        yPos -= buttonHeight;
    }

    /*!
    \fn RemoveDropdowns();
    \brief Function for when the minus button is clicked, to remove dropdown options
    */
    public void RemoveDropdowns()
    {
        if (scenarioListDropdowns.Count <= 0)
            return;

        Transform contentWindow = transform.GetChild(0).GetChild(0);

        {
            Dropdown dropdown = scenarioListDropdowns[scenarioListDropdowns.Count - 1];
            scenarioListDropdowns.RemoveAt(scenarioListDropdowns.Count - 1);
            Destroy(dropdown.gameObject);
        }

        {
            Dropdown dropdown = ehmiListDropdowns[ehmiListDropdowns.Count - 1];
            ehmiListDropdowns.RemoveAt(ehmiListDropdowns.Count - 1);
            Destroy(dropdown.gameObject);
        }

        float contentWindowHeight = buttonHeight * scenarioListDropdowns.Count + 1;
        RectTransform rt_content = contentWindow.GetComponent<RectTransform>();
        rt_content.sizeDelta = new Vector2(rt_content.sizeDelta.x, contentWindowHeight);

        yPos += buttonHeight;
    }

    /*!
    \fn OnClickStart();
    \brief Button click callback function, an OnClick event listener. Starts the first scene in the selection
    */
    public void OnClickStart()
    {
        SceneController.instance.scenes.Clear();

        foreach (Dropdown dropdown in scenarioListDropdowns)
        {
            if (dropdown.value >= SceneController.instance.SceneDictionary.Count)
                continue;

            //Get the key from assignedIndex
            string key = SceneController.instance.assignedSceneIndex[dropdown.value];
            //SceneController.instance.scenes.Add(SceneController.instance.scenesList[dropdown.value]);
            //Use Key to get the correspending scene
            SceneController.instance.scenes.Add(SceneController.instance.SceneDictionary[key]);
        }

        SceneController.instance.ehmiSequence.Clear();

        foreach (Dropdown dropdown in ehmiListDropdowns)
        {
            if (dropdown.value >= SceneController.instance.EHMIDictionary.Count)
                continue;

            string key = SceneController.instance.assignedEHMIIndex[dropdown.value];
            SceneController.instance.ehmiSequence.Add(SceneController.instance.EHMIDictionary[key]);
        }
    }

    public void LoadDropdown()
    {
        int times = scenarioListDropdowns.Count;
        for(int i = 0; i < times; ++i)
        {
            RemoveDropdowns();
        }

        List<string> currentData = ReadConfig.instance.GetCurrentData();
        foreach(string data in currentData)
        {
            AddDropdowns();
            StartCoroutine(ChangeValue());
        }
    }

    IEnumerator ChangeValue()
    {
        yield return new WaitForEndOfFrame();
        List<string> currentData = ReadConfig.instance.GetCurrentData();
        for(int j = 0; j < currentData.Count; ++j)
        {
            var values = currentData[j].Split('-');
            for(int i = 0; i < SceneController.instance.assignedSceneIndex.Count; ++i)
            {
                //Debug.Log(SceneController.instance.assignedSceneIndex[i] + "," + values[0]);
                if (SceneController.instance.assignedSceneIndex[i] == values[0])
                {
                    scenarioListDropdowns[j].value = i;
                    break;
                }
            }

            for (int i = 0; i < SceneController.instance.assignedEHMIIndex.Count; ++i)
            {
                //Debug.Log(SceneController.instance.assignedEHMIIndex[i] + "," + values[1]);
                if (SceneController.instance.assignedEHMIIndex[i] == values[1])
                {
                    ehmiListDropdowns[j].value = i;
                    break;
                }
            }
        }
    }
}
