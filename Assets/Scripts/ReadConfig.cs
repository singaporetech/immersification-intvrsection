using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
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
\file SceneController.cs
\brief Class to read Config File (Config/avhmi_ID_List.csv)
*/

public class ReadConfig : MonoBehaviour
{
    public static ReadConfig instance { get; private set; }

    int currentID = -1;
    List<string> currentData = new List<string>();

    public GameObject textBox;
    public GameObject textInfo;


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
    \fn GetCurrentID();
    \return Requested int ID
    \brief Getter for the current ID set in the system
    */
    public int GetCurrentID()
    {
        return currentID;
    }

    /*!
    \fn GetCurrentData();
    \return Requested List of data assigned to the set ID
    \brief Getter for the current Data assigned to the ID set in the system
    */
    public List<string> GetCurrentData()
    {
        return currentData;
    }

    /*!
    \fn FindID();
    \param ID to search database for 
    \return true if the ID can be found, false if not
    \brief Searches the database for an ID and the associated data and set them in the system.
    */
    private bool FindID(int ID)
    {
        StreamReader strReader = new StreamReader("Config/avhmi_ID_List.csv");
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
            int thisID = int.Parse(values[0]);
                
            if(ID == thisID)
            {
                currentID = thisID;
                currentData = new List<string>();
                for(int i = 1; i < values.Length; ++i)
                {
                    currentData.Add(values[i]);
                }
                return true;
           }    
        }
        currentID = ID;
        currentData = new List<string>();
        return false;
    }

    /*!
    \fn SetData();
    \brief Gets the int in the textbox and begin a search.
    */
    public void SetData()
    {
        if(textBox != null)
        {
            int value;
            bool test = int.TryParse(textBox.GetComponent<InputField>().text, out value);
            if (test)
            {
                //Debug.Log("Valid Number input: " + value);
                if (FindID(value))
                {
                    textInfo.GetComponent<Text>().text = "ID Found!";
                    textInfo.GetComponent<Text>().color = Color.black;
                }
                else
                {
                    textInfo.GetComponent<Text>().text = "ID Not Found!";
                    textInfo.GetComponent<Text>().color = Color.red;
                }
            }
            else 
            {
                currentID = -1;
                textInfo.GetComponent<Text>().text = "Invalid ID!";
                textInfo.GetComponent<Text>().color = Color.red;
            }

        }
    }

    /*!
    \fn InvalidIDWarning();
    \param ID to check if valid
    \brief Function called to change text in UI if inserted ID is invalid.
    */
    public void InvalidIDWarning(int i) 
    {
        if (textBox != null)
        {
            if (i == 0)
            {
                textInfo.GetComponent<Text>().text = "Invalid ID! Please input a valid ID";
                textInfo.GetComponent<Text>().color = Color.red;
            }
            else if (i == 1) 
            {
                textInfo.GetComponent<Text>().text = "No scenarios selected!";
                textInfo.GetComponent<Text>().color = Color.red;
            }
            
        }
    }
}
