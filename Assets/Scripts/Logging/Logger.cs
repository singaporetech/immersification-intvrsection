using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
\brief Class to Log data to a csv and/or json file
*/

public class Logger : MonoBehaviour
{
    public static Logger instance { get; private set; }                                         /**The singleton instance of this class*/

    struct Item<T>                                                                              /**A struct to contain information about the item logged*/
    {
        public List<T> values;                                                                  /**A dictionary hash map storing the scene name as the key and the logged value as the value*/
    }

    Dictionary<string, Item<string>> stringItems = new Dictionary<string, Item<string>>();      /**A hash map of all the string log items, with their names as the keys and the item as the values*/
    Dictionary<string, Item<float>> floatItems = new Dictionary<string, Item<float>>();         /**A hash map of all the float log items, with their names as the keys and the item as the values*/
    Dictionary<string, Item<Vector3>> Vector3Items = new Dictionary<string, Item<Vector3>>();   /**A hash map of all the Vector3 log items, with their names as the keys and the item as the values*/

    public int scenarioNumber;                                                                  /**The scenario number, found in the environment object's name*/
    public bool isControlled;                                                                   /**If the scenraio is controlled by traffic lights*/
    bool logJson = false;                                                                       /**If the logger is generating a Json file*/

    /*!
    \fn Awake();
    \brief Called when the script instance is being loaded
    */
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        if (!SceneController.instance || !SceneController.instance.logData)
            return;
            
        logJson = SceneController.instance.logJson;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Start")
        {
            return;
        }
        scenarioNumber = SceneController.instance.sceneIndex;
        Log("Scenario Number", scenarioNumber);
        if (isControlled)
            Log("Is Controlled", "True");
        else
            Log("Is Controlled", "False");
    }

    /*!
    \fn OnDestroy();
    \brief Called when this object is destroyed
    */
    private void OnDestroy()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Start")
            return;

        if (!SceneController.instance || !SceneController.instance.logData)
            return;

        SaveToCSVFile(ReadConfig.instance.GetCurrentID());

        if (logJson)
        {
            SaveToJSONFile(ReadConfig.instance.GetCurrentID());
        }
    }

    /*!
    \fn Log(string name, string value);
    \brief Function to log a single string item.
        Creates a new entry with the name if the entry does not exist
    \param name The name of the entry
    \param value The value to log
    */
    public void Log(string name, string value)
    {
        if (!stringItems.ContainsKey(name))
        {
            Item<string> newItem = new Item<string>();
            newItem.values = new List<string>();
            stringItems.Add(name, newItem);
        }
        stringItems[name].values.Add(value);
    }

    /*!
    \fn Log(string name, float value);
    \brief Function to log a single float item.
        Creates a new entry with the name if the entry does not exist
    \param name The name of the entry
    \param value The value to log
    */
    public void Log(string name, float value)
    {   
        if (!floatItems.ContainsKey(name))
        {
            Item<float> newItem = new Item<float>();
            newItem.values = new List<float>();
            floatItems.Add(name, newItem);
        }
        floatItems[name].values.Add(value);
    }

    /*!
    \fn Log(string name, Vector3 value);
    \brief Function to log a single Vector3 item.
        Creates a new entry with the name if the entry does not exist
    \param name The name of the entry
    \param value The value to log
    */
    public void Log(string name, Vector3 value)
    {
        if (!Vector3Items.ContainsKey(name))
        {
            Item<Vector3> newItem = new Item<Vector3>();
            newItem.values = new List<Vector3>();
            Vector3Items.Add(name, newItem);
        }
        Vector3Items[name].values.Add(value);
    }

    /*!
    \fn ClearLogFiles(int index);
    \brief Function to clear the logging save files.
    \param index The index of the current experiment
    */
    public void ClearLogFiles(int index)
    {
        string path = "Logger/" + index;

        using (StreamWriter csv_sr = new StreamWriter(path + ".csv"))
        {
            csv_sr.Write(string.Empty);
            csv_sr.Close();
        }

        if (logJson)
        {
            using (StreamWriter json_sr = new StreamWriter(path + ".json"))
            {
                json_sr.Write(string.Empty);
                json_sr.Close();
            }
        }
    }

    /*!
    \fn SaveToCSVFile(int index);
    \brief Function to save the logs to a .csv file.
        Saves to the path "Logger/(index).csv"
    \param index The index of the current experiment
    */
    public void SaveToCSVFile(int index)
    {
        Scene scene = SceneManager.GetActiveScene();
        string ehmiType = "Blank";
        if (SceneController.instance.sceneIndex < SceneController.instance.ehmiSequence.Count)
            ehmiType = SceneController.instance.getCurrentEHMIName();

        string line = scene.name + " (" + ehmiType + ")\n";

        foreach (string itemName in stringItems.Keys)
        {
            line += itemName;
            foreach (string itemValue in stringItems[itemName].values)
            {
                line += "," + itemValue;
            }
            line += "\n";
        }

        foreach (string itemName in floatItems.Keys)
        {
            line += itemName;
            foreach (float itemValue in floatItems[itemName].values)
            {
                line += "," + itemValue;
            }
            line += "\n";
        }

        foreach (string itemName in Vector3Items.Keys)
        {
            line += itemName;
            foreach (Vector3 itemValue in Vector3Items[itemName].values)
            {
                line += ",(" + itemValue.x + ":" + itemValue.y + ":" + itemValue.z + ")";
            }
            line += "\n";
        }

        line += ",\n";

        if (!File.Exists("Logger"))
            Directory.CreateDirectory("Logger");

        string path = "Logger/" + index + ".csv";

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(line);
            sw.Close();
        }
    }

    /*!
    \fn OpenJSONFile(int index);
    \brief Function to open the .json file to log to.
        Saves to the path "Logger/(index).csv"
    \param index The index of the current experiment
    */
    public void OpenJSONFile(int index)
    {
        if (!File.Exists("Logger"))
            Directory.CreateDirectory("Logger");

        string path = "Logger/" + index + ".json";

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine("{");
            sw.Close();
        }
    }

    /*!
    \fn CloseJSONFile(int index);
    \brief Function to close the .json file used for logging.
        Saves to the path "Logger/(index).csv"
    \param index The index of the current experiment
    */
    public void CloseJSONFile(int index)
    {
        string path = "Logger/" + index + ".json";

        string[] lines = File.ReadAllLines(path);

        using (StreamWriter sw = new StreamWriter(path))
        {
            for (int i = 0; i < lines.Length - 1; ++i)
                sw.WriteLine(lines[i]);

            sw.Close();
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine("\t}\n}");
            sw.Close();
        }
    }

    /*!
    \fn SaveToJSONFile(int index);
    \brief Function to save the logs to a .json file.
        Saves to the path "Logger/(index).json"
    \param index The index of the current experiment
    */
    private void SaveToJSONFile(int index)
    {
        Scene scene = SceneManager.GetActiveScene();
        string line = "\t\"" + scene.name + "\":{\n";

        foreach (string itemName in stringItems.Keys)
        {
            line += "\t\t\"" + itemName + "\":[\n";
            foreach (string itemValue in stringItems[itemName].values)
            {
                line += "\t\t\t\"" + itemValue + "\",\n";
            }
            line = line.Remove(line.Length - 2, 1);
            line += "\t\t],\n";
        }

        foreach (string itemName in floatItems.Keys)
        {
            line += "\t\t\"" + itemName + "\":[\n";
            foreach (float itemValue in floatItems[itemName].values)
            {
                line += "\t\t\t" + itemValue + ",\n";
            }
            line = line.Remove(line.Length - 2, 1);
            line += "\t\t],\n";
        }

        foreach (string itemName in Vector3Items.Keys)
        {
            line += "\t\t\"" + itemName + "\":[\n";
            foreach (Vector3 itemValue in Vector3Items[itemName].values)
            {
                line += "\t\t\t\"(" + itemValue.x + ";" + itemValue.y + ";" + itemValue.z + ")\",\n";
            }
            line = line.Remove(line.Length - 2, 1);
            line += "\t\t],\n";
        }

        line = line.Remove(line.Length - 2, 1);
        line += "\t},";

        string path = "Logger/" + index + ".json";

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(line);
            sw.Close();
        }
    }
}
