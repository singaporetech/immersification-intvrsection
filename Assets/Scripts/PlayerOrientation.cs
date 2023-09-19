using System.Collections;
using System.Collections.Generic;
using TMPro;
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
\file PlayerOrientation.cs
\brief Script for ensuring that the player faces the correct direction before restarting the level
*/

public class PlayerOrientation : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;   /**The main camera*/
    [SerializeField] private GameObject deadText;       /**The text object that appears when the player is dead*/
    [HideInInspector] public GameObject dot;            /**The dot that the player has to look at when in position at the start of the level*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        dot = transform.GetChild(0).gameObject;
    }

    /*!
    \fn OnTriggerEnter(Collider other);
    \brief Function for triggering if the player has entered the start zone
    \param other The object's collider
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dot.SetActive(true);
            deadText.GetComponent<TMP_Text>().text = "Look at the red dot";
        }
    }

    /*!
    \fn OnTriggerExit(Collider other);
    \brief Function for triggering if the player has left the start zone
    \param other The object's collider
    */
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dot.SetActive(false);
            deadText.GetComponent<TMP_Text>().text = "Follow the arrows";
        }
    }

    /*!
    \fn OnTriggerStay(Collider other);
    \brief Function for triggering if the player is in the correct position at the start of the level
    \param other The object's collider
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Vector3.Angle(other.transform.forward, transform.forward) < 10.0f)
            {
                dot.SetActive(false);
                deadText.SetActive(false);
                gameObject.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(playerCamera.GetComponent<PlayerCollision>().FadeEmptyWorldToScene());
            }
        }
    }
}
