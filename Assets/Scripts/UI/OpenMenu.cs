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
\file OpenMenu.cs
\brief Class to open the menu UI in the scenarios
*/

public class OpenMenu : MonoBehaviour
{
    bool isOpen = false;                        /**If the UI is open*/
    [SerializeField] GameObject button;         /**The button UI object*/
    [SerializeField] GameObject window;         /**The window UI object to open*/

    /*!
    \fn Click();
    \brief Button click callback function, an OnClick event listener. Enables/disables the window UI object 
    */
    public void Click()
    {
        if (isOpen)
        {
            window.SetActive(false);
            isOpen = false;
            button.transform.rotation = Quaternion.identity;
        }
        else
        {
            window.SetActive(true);
            isOpen = true;
            button.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
    }
}
