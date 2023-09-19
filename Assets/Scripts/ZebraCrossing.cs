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

/*!
\file ZebraCrossing.cs
\brief Class to attach to ZebraCrossing to know if a player is on it.
*/

public class ZebraCrossing : MonoBehaviour
{
    [HideInInspector] public bool isPlayer = false; /**Check if player is on the ZebraCrossing*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayer = false;
    }
}
