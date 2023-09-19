using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

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
\file NextSceneButton.cs
\brief Class defining the behaviour for the interactable button to progress the player to the next scene
*/

public class NextSceneButton : MonoBehaviour
{
    public int delayTime;                                       /**Amount of delay before transitioning to the next scene*/
    int seconds;                                                /**current amount of seconds elapsed since button pressed*/
    TMP_Text text;                                              /**The button's text component*/
    bool touched = false;                                       /**Bool lock so the player cannot retrigger the button*/
    bool readyToSwitch = false;                                 /**when the scene is ready to load the next scene*/
    [SerializeField] private PostProcessVolume globalVolume;    /**The global post processing volume*/
    private ColorGrading colourGrading;                         /**The Colour Grading effect, for post processing*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    void Start()
    {
        seconds = delayTime;
        text = transform.GetChild(0).GetComponent<TMP_Text>();
        globalVolume.profile.TryGetSettings(out colourGrading);
    }

    private void Update()
    {
        if (readyToSwitch && Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.instance.SwitchScenario();
        }
    }

    /*!
    \fn OnTriggerEnter(Collider collider);
    \brief Called when anything touches the button
    \param collider The other object's collider
    */
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Hand")
        {
            if (!touched)
            {
                StartCoroutine(Countdown());
                touched = true;
                TimerScenario.instance.StopTimer();
            }
        }
    }

    /*!
    \fn Countdown();
    \brief Coroutine to count down the time to the scene transition
    */
    private IEnumerator Countdown()
    {

        text.text = "Changing Scenario\nin " + seconds + "...";
        yield return new WaitForSeconds(1.0f);
        --seconds;

        if (seconds <= 0)
        {
            StartCoroutine(FadeOut());
            yield break;
        }
        StartCoroutine(Countdown());
    }

    /*!
    \fn FadeOut();
    \brief Coroutine to fade out the world for the scene transition
    */
    private IEnumerator FadeOut()
    {
        for (int c = 0; c <= 100; ++c)
        {
            colourGrading.contrast.value = -c;
            yield return new WaitForSeconds(0.02f);
        }
        GameObject.Find("Car Pool").SetActive(false); // Find Car pool object and deactivate it

        GameObject[] trafficlightButtons = GameObject.FindGameObjectsWithTag("Traffic Light Button");
        foreach (GameObject trafficLightButton in trafficlightButtons)
        {
            trafficLightButton.GetComponent<AudioSource>().enabled = false;
        }

        readyToSwitch = true;
    }
}
