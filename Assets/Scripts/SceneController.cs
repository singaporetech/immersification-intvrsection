using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

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
\file SceneController.cs
\brief Class to control the logic of switching between scenes
    The SceneController class is a singleton
*/

public class SceneController : MonoBehaviour
{
    public static SceneController instance;                 /**The instance of this class*/

    public List<string> scenes;         /**List for the order of the scenes*/
    [HideInInspector] public int sceneIndex = 0;            /**The current scene index*/
    public bool logJson = false; 
    public bool logData = false;                           /**If the logger is generating a Json file*/

    [HideInInspector] public List<string> ehmiSequence;     /**List for the order of the scenes*/
    public Dictionary<string, string> EHMIDictionary = new Dictionary<string, string>();
    public List<string> assignedEHMIIndex;

    public Dictionary<string, string> SceneDictionary = new Dictionary<string, string>(); /**Dictionary of scene file names with assigned no. as key*/
    public List<string> assignedSceneIndex;

    /*!
    \fn Awake();
    \brief Called when the script instance is being loaded
    */
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        loadSceneDictionary();
        loadEHMIDictionary();
    }

    private void Start()
    {
        if (logJson)
            Logger.instance.OpenJSONFile(0000);
    }

    /*!
    \fn OnApplicationQuit();
    \brief Called when the application exits
    */
    private void OnApplicationQuit()
    {
        if (logJson)
            Logger.instance.CloseJSONFile(0000);
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SwitchScenario();
        }
    }

    /*!
    \fn ReloadScenario();
    \brief Public function for reloading the scene
    */
    public void ReloadScenario()
    {
        SceneManager.LoadScene(scenes[sceneIndex]);
    }

    /*!
    \fn SwitchScenario();
    \brief Public function for switching scenes
    */
    public void SwitchScenario()
    {
        if (scenes.Count != 0)
        {
            ++sceneIndex;
            if (sceneIndex >= scenes.Count)
            {
                sceneIndex = 0;
                SceneManager.LoadScene("Start");
                scenes.Clear();
                return;
            }
            SceneManager.LoadScene(scenes[sceneIndex]);
        }
    }

    /*!
    \fn loadSceneDictionary();
    \brief Function to be called on startup to read the scene_config csv
    */
    private void loadSceneDictionary()
    {
        StreamReader strReader = new StreamReader("Config/scene_config.csv");
        bool endOfFile = false;
        while (!endOfFile)
        {
             var line = strReader.ReadLine();
            if(line == null)
            {
                endOfFile = true;
                break;  
            }
            var values = line.Split(',');
            SceneDictionary.Add(values[0], values[1]);
            assignedSceneIndex.Add(values[0]);
        }
    }

    /*!
    \fn loadEHMIDictionary();
    \brief Function to be called on startup to read the ehmi_config csv
    */
    private void loadEHMIDictionary()
    {
        StreamReader strReader = new StreamReader("Config/ehmi_config.csv");
        bool endOfFile = false;
        while (!endOfFile)
        {
            var line = strReader.ReadLine();
            if (line == null)
            {
                endOfFile = true;
                break;
            }
            var values = line.Split(',');
            EHMIDictionary.Add(values[0], values[1]);
            assignedEHMIIndex.Add(values[0]);
        }
    }

    /*!
    \fn getCurrentSceneName();
    \return Current Scene string
    \brief Getter to get the running scene name
    */
    public string getCurrentSceneName() 
    {
        return scenes[sceneIndex];
    }

    /*!
    \fn getCurrentEHMIName();
    \return Current EHMI string
    \brief Getter to get the current active EHMI
    */
    public string getCurrentEHMIName() 
    {
        return ehmiSequence[sceneIndex];
    }
}