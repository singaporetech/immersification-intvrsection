using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
\file TrafficLightController.cs
\brief Class for handling the traffic light states
*/

public class TrafficLightController : MonoBehaviour
{
    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    public float trafficLightInterval = 18.0f;
    public float colourSwitchTime = 4.0f;                                   /**The time it takes to switch betwwen colour states*/
    [SerializeField] bool automaticSwitching = false;                       /**If the traffic lights in this scenario switch on a timer*/

    // Start is called before the first frame update
    void Start()
    {
        TrafficLightButton.buttonPressEvent += ButtonPressed;

        GameObject[] trafficLightObjects = GameObject.FindGameObjectsWithTag("Traffic Light");
        foreach (GameObject go in trafficLightObjects)
            trafficLights.Add(go.GetComponent<TrafficLight>());

        foreach (TrafficLight t in trafficLights)
        {
            SetState(t);

            t.greenManTime = trafficLightInterval;
            t.colourSwitchTime = colourSwitchTime;
        }

        if (!automaticSwitching)
            return;

        StartCoroutine(SwitchTimerRecursive());
    }

    private void SetState(TrafficLight light)
    {
        if (light.group == TrafficLight.Group.GROUP1)
        {
            light.colour = TrafficLight.Colour.GREEN;
            light.Green();
        }
        else
        {
            light.colour = TrafficLight.Colour.RED;
            light.Red();
        }
    }

    private void OnDisable()
    {
        TrafficLightButton.buttonPressEvent -= ButtonPressed;
    }

    private void ButtonPressed()
    {
        if (automaticSwitching)
            return;

        SwitchTrafficLightsState();
        StartCoroutine(SwitchTimer());
    }

    private IEnumerator SwitchTimerRecursive()
    {
        SwitchTrafficLightsState();
        yield return new WaitForSeconds(trafficLightInterval + colourSwitchTime);
        
        StartCoroutine(SwitchTimerRecursive());
    }

    private IEnumerator SwitchTimer()
    {
        yield return new WaitForSeconds(trafficLightInterval + colourSwitchTime);
        SwitchTrafficLightsState();
    }

    void SwitchTrafficLightsState()
    {
        foreach (TrafficLight t in trafficLights)
        {
            StartCoroutine(t.SwitchState());
        }
    }
}
