using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
\file StartFirstScenario.cs
\brief Class defining behaviour of the Start button in the Start scene
*/

public class StartFirstScenario : MonoBehaviour
{
    /*!
    \fn Click();
    \brief Public function for starting the first scene
    */
    public void Click()
    {
        if (SceneController.instance.scenes.Count != 0)
        {
            if (ReadConfig.instance.GetCurrentID() == -1)
            {
                ReadConfig.instance.InvalidIDWarning(0);
            }
            else
            {
                SceneController.instance.sceneIndex = 0;
                SceneManager.LoadScene(SceneController.instance.scenes[SceneController.instance.sceneIndex]);
            }
        }
        else 
        {
            ReadConfig.instance.InvalidIDWarning(1);
        }
    }
}
