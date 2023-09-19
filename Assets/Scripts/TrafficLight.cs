using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
\file TrafficLight.cs
\brief Class for handling the traffic light logic 
*/

public class TrafficLight : MonoBehaviour
{
    public enum Colour                                          /**Enum for the colour state of the traffic light*/
    {
        GREEN = 0,                                              /**Green state of the traffic light*/
        YELLOW,                                                 /**Yellow state of the traffic light*/
        RED                                                     /**Red state of the traffic light*/
    }

    public MeshRenderer[] pedestrianLight;                      /**MeshRender component of the pedestrian crossing lights*/
    public MeshRenderer[] RedLight;                             /**MeshRender component of the red lamp of the traffic lights*/
    public MeshRenderer[] YellowLight;                          /**MeshRender component of the yellow lamp of the traffic lights*/
    public MeshRenderer[] GreenLight;                           /**MeshRender component of the green lamp of the traffic lights*/

    public Material m_Indicator;                                /**Traffic light button indicator light material*/
    public Material m_Glass;                                    /**Glass material*/
    public Material m_Green;                                    /**Lit green material*/
    public Material m_Yellow;                                   /**Lit yellow material*/
    public Material m_Red;                                      /**Lit red material*/
    public Material m_Black;                                    /**Black material*/

    public GameObject button;                                   /**The traffic light button object*/
    TrafficLightButton buttonScript;                            /**Cache memory for the button script*/

    [HideInInspector] public Colour colour;                     /**The current colour state of the traffic light*/
    [HideInInspector] public float colourSwitchTime = 4.0f;     /**The time it takes to switch betwwen colour states*/

    public float stopLineOffset;                                /**The offset from this traffic light for the stop line, to be set in the editor*/
    public float stopLineLength = 2.0f;                         /**The stop line length, just for debugging purposes*/

    public float greenManTime;                                  /**The amount of time the green man is active*/
    private bool buttonPressed = false;                         /**Tf this traffic light's button was pressed*/
    public bool greenMan = false;                               /**If the pedestrian light is currently a green man*/

    public enum Group                                           /**Groups the traffic light inot either 1 or 2, this is for the handling of a 2 4 way intersection and traffic lights have to alternate in states*/
    {
        GROUP1 = 0,                                             /**Group 1*/
        GROUP2                                                  /**Group 2*/
    }
    public Group group;                                         /**The group this traffic light belongs to*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        TrafficLightButton.buttonPressEvent += ButtonPress;
        if (button)
        {
            buttonScript = button.GetComponent<TrafficLightButton>();
            buttonScript.Init();
        }
        Green();
        PedestrianRedMan();
    }

    /*!
    \fn OnDisable();
    \brief Called when this object is disabled
    */
    private void OnDisable()
    {
        TrafficLightButton.buttonPressEvent -= ButtonPress;
    }

    /*!
    \fn ButtonPress();
    \brief This function is called when the traffic light button is pressed
    */
    void ButtonPress()
    {
        if (!button || buttonPressed)
            return;

        buttonPressed = true;

        buttonScript.buttonLight.material = m_Red;
    }

    /*!
    \fn OnDrawGizmos();
    \brief This function draws debugging gizmos in the editor
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position;
        origin.y = 0.5f;
        origin += -transform.right * stopLineOffset;
        Gizmos.DrawLine(origin, origin + transform.up * stopLineLength);
    }

    /*!
    \fn SwitchState();
    \brief Coroutine for switching the state of the traffic light
    */
    public IEnumerator SwitchState()
    {
        yield return new WaitForSeconds(colourSwitchTime);

        if (colour == Colour.GREEN)
        {
            colour = Colour.YELLOW;
            Yellow();
            yield return new WaitForSeconds(colourSwitchTime);
            colour = Colour.RED;
            Red();
            yield return new WaitForSeconds(colourSwitchTime);

            if (buttonPressed)
            {
                for (int i = 0; i < pedestrianLight.Length; i++)
                {
                    StartCoroutine(PedestrianGreenMan(pedestrianLight[i]));
                }
            }   
        }
        else if (colour == Colour.RED)
        {
            yield return new WaitForSeconds(colourSwitchTime * 2.0f);
            colour = Colour.GREEN;
            Green();
        }
        yield break;
    }

    /*!
    \fn Red();
    \brief Function to change the materials for the trafic light lamps for the red state
    */
    public void Red()
    {
        if (RedLight.Length == 0 || 
            YellowLight.Length == 0 || 
            GreenLight.Length == 0)
            return;

        foreach (MeshRenderer m in RedLight)
            m.material = m_Red;
        foreach (MeshRenderer m in YellowLight)
            m.material = m_Glass;
        foreach (MeshRenderer m in GreenLight)
            m.material = m_Glass;
    }

    /*!
    \fn Yellow();
    \brief Function to change the materials for the trafic light lamps for the yellow state
    */
    public void Yellow()
    {
        if (RedLight.Length == 0 ||
            YellowLight.Length == 0 ||
            GreenLight.Length == 0)
            return;

        foreach (MeshRenderer m in RedLight)
            m.material = m_Glass;
        foreach (MeshRenderer m in YellowLight)
            m.material = m_Yellow;
        foreach (MeshRenderer m in GreenLight)
            m.material = m_Glass;
    }

    /*!
    \fn Green();
    \brief Function to change the materials for the trafic light lamps for the green state
    */
    public void Green()
    {
        if (RedLight.Length == 0 ||
            YellowLight.Length == 0 ||
            GreenLight.Length == 0)
            return;

        foreach (MeshRenderer m in RedLight)
            m.material = m_Glass;
        foreach (MeshRenderer m in YellowLight)
            m.material = m_Glass;
        foreach (MeshRenderer m in GreenLight)
            m.material = m_Green;
    }

    /*!
    \fn PedestrianRedMan();
    \brief Function to change the materials for the pedestrian light lamps for the red man state
    */
    private void PedestrianRedMan()
    {
        greenMan = false;

        if (pedestrianLight.Length == 0)
            return;

        for (int i = 0; i < pedestrianLight.Length; i++)
        {
            Material[] materialsArray = pedestrianLight[i].materials;
            materialsArray[1] = m_Red;
            materialsArray[2] = m_Black;
            pedestrianLight[i].materials = materialsArray;
            buttonScript.buttonAudioSource.clip = buttonScript.buttonAudioClips[0];
            buttonScript.buttonAudioSource.Play();
        }
    }

    /*!
    \fn PedestrianRedMan();
    \brief Coroutine to change the materials for the pedestrian light lamps and handle the timing logic for the green man state
    */
    IEnumerator PedestrianGreenMan(MeshRenderer m_pedestrianLight)
    {
        if (pedestrianLight.Length == 0)
            yield break;

        greenMan = true;

        float percentTimeSteady = 0.25f;
        float flashingIntervalTime = 2.0f;
        float steadyGreenManTime = greenManTime * percentTimeSteady;
        float flashingGreenManTime = greenManTime - steadyGreenManTime;
        int numberOfFlashes = (int)(flashingGreenManTime / flashingIntervalTime);

        Material[] materialsArray = m_pedestrianLight.materials;

        yield return new WaitForSeconds(2.0f);

        if (button)
        {
            buttonScript.buttonLight.material = m_Indicator;
        }

        materialsArray[1] = m_Black;
        materialsArray[2] = m_Green;
        m_pedestrianLight.materials = materialsArray;

        buttonScript.buttonAudioSource.clip = buttonScript.buttonAudioClips[1];
        buttonScript.buttonAudioSource.Play();
        yield return new WaitForSeconds(steadyGreenManTime);

        buttonScript.buttonAudioSource.clip = buttonScript.buttonAudioClips[2];
        buttonScript.buttonAudioSource.loop = true;
        buttonScript.buttonAudioSource.Play();

        for (int i = 0; i < numberOfFlashes; i++)
        {
            materialsArray[2] = m_Black;
            m_pedestrianLight.materials = materialsArray;
            yield return new WaitForSeconds(flashingIntervalTime * 0.5f);
            materialsArray[2] = m_Green;
            m_pedestrianLight.materials = materialsArray;
            yield return new WaitForSeconds(flashingIntervalTime * 0.5f);
        }
        buttonScript.buttonAudioSource.Stop();

        buttonPressed = false;

        PedestrianRedMan();
    }
}