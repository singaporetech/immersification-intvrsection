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
\file SceneCounter.cs
\brief Class used to count scenes
*/

public class SceneCounter : MonoBehaviour
{
    public Text textbox;
    private void OnEnable()
    {
        string test = (SceneController.instance.sceneIndex + 1) + "/" + SceneController.instance.scenes.Count;
        textbox.text = test;
    }
}
