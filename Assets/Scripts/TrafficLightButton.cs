using System;
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
\file TrafficLightButton.cs
\brief Class for handling player interaction with the traffic light button
*/

public class TrafficLightButton : MonoBehaviour
{
    //[HideInInspector] public bool touched = false;              /**Bool lock so that the player cannot retrigger the button*/
    public static event Action buttonPressEvent;                /**Event for pressing the button*/
    [HideInInspector] public MeshRenderer buttonLight;          /**Mesh component of the button's light*/
    [HideInInspector] public AudioSource buttonAudioSource;     /**Audio source component of the button*/
    public AudioClip[] buttonAudioClips;                        /**An array of AudioClips for the traffic light button to play*/
    public AudioClip buttonClickAudio;                          /**AudioClip for the button click sound*/
    private bool buttonPressedAudio = true;                    /**Bool lock so that the the Audio will not loop*/

    /*!
    \fn Init();
    \brief Called once at the start to initialise the varaibles
    */
    public void Init()
    {
        buttonLight = transform.GetChild(0).GetComponent<MeshRenderer>();
        buttonAudioSource = gameObject.GetComponent<AudioSource>();
    }

    /*!
    \fn OnTriggerEnter(Collider other);
    \brief Called when anything collides with the button's trigger volume
    \param other The other object that the button collided with
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand")
        {
            if (buttonPressedAudio)
            {
                buttonAudioSource.PlayOneShot(buttonClickAudio, 1.0f);
                buttonPressedAudio = false;   //Lock the audio so that it does not loop
            }
            buttonPressEvent?.Invoke();
        }
    }
    
    /*!
    \fn OnTriggerExit(Collider other);
    \brief Called when anything exits the button's trigger volume
    \param other The other object that the button collided with
    */
    private void OnTriggerExit(Collider other)
    {
        buttonPressedAudio = true;    //Unlock the audio so that it can play again when the player re-enters the trigger volume
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            buttonPressEvent?.Invoke();
        }
    }
}