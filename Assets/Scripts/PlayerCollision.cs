using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;
using TMPro;

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
\file PlayerCollision.cs
\brief Class for handling vehicle - player collision logic
*/

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameObject deadText;               /**A text object for telling the player they have been hit*/
    [SerializeField] private PostProcessVolume globalVolume;    /**The global post processing volume*/
    [SerializeField] private GameObject carPool;                /**The Car Pool object*/
    [SerializeField] private GameObject emptyWorldGroundPlane;  /**The Empty world's ground plane object*/
    [SerializeField] private GameObject pathNode;               /**A path node prefab, to draw the direction back to the start*/
    [SerializeField] private GameObject startPoint;             /**The start point prefab, to show where the player has to go*/
    [SerializeField] private GameObject spawnPoint;             /**The player's Spawnpoint object*/
    [SerializeField] private GameObject pedestal;               /**The next scene pedestal object*/
    [SerializeField] private GameObject spawnCarForPlayer;      /**The spawn car for player when they walk up to the curb logic object*/
    private bool knocked = false;                               /**Bool lock so the player cannot retrigger the respawn code*/
    private ColorGrading colourGrading;                         /**The Colour Grading effect, for post processing*/
    private List<GameObject> path = new List<GameObject>();     /**A list holding all the path nodes, for ease of removal after respawn*/

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    private void Start()
    {
        globalVolume.profile.TryGetSettings(out colourGrading);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!knocked)
            {
                knocked = true;
                StartCoroutine(FadeSceneToEmptyWorld());
            }
        }
    }

    /*!
    \fn OnTriggerEnter(Collider col);
    \brief Function Runs whenever this vehicle collides with anything
    \param col The object's collider
    */
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Auto Car") ||
            col.gameObject.CompareTag("Normal Car"))
        {
            float currentSpeed = col.GetComponent<Rigidbody>().velocity.sqrMagnitude;
            if (!knocked && currentSpeed > 0.1f)
            {
                knocked = true;
                StartCoroutine(FadeSceneToEmptyWorld());
            }
        }
    }

    /*!
    \fn FadeSceneToEmptyWorld();
    \brief Coroutine for fading out the Scenario and fading in the empty world
    */
    IEnumerator FadeSceneToEmptyWorld()
    {
        deadText.SetActive(true);
        deadText.GetComponent<TMP_Text>().text = "You were hit by a vehicle\r\nResetting player...";
        for (int c = 0; c <= 100; ++c)
        {
            colourGrading.contrast.value = -c;
            yield return new WaitForSeconds(0.02f);
        }

        carPool.GetComponent<CarPool>().DisableAllCars();
        carPool.SetActive(false);

        pedestal.SetActive(false);

        TimerWalkAcrossRoad.instance.StopTimer();
        spawnCarForPlayer.GetComponent<SpawnCarForPlayer>().spawned = false;

        MeshRenderer[] meshes = GameObject.FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            if (mesh.gameObject.CompareTag("Hand") ||
                mesh.gameObject.CompareTag("Auto Car") ||
                mesh.gameObject.CompareTag("Normal Car") ||
                mesh.gameObject.CompareTag("Text"))
                continue;
            mesh.enabled = false;
        }

        GameObject[] trafficlightButtons = GameObject.FindGameObjectsWithTag("Traffic Light Button");
        foreach (GameObject trafficLightButton in trafficlightButtons)
        {
            trafficLightButton.GetComponent<AudioSource>().enabled = false;
        }

        emptyWorldGroundPlane.SetActive(true);
        deadText.GetComponent<TMP_Text>().text = "Follow the arrows";

        CreatePath();

        for (int c = 0; c <= 100; ++c)
        {
            colourGrading.contrast.value = c - 100;
            yield return new WaitForSeconds(0.02f);
        }
        spawnPoint.GetComponent<BoxCollider>().enabled = true;
    }

    /*!
    \fn CreatePath();
    \brief Function for creating the path (Arrows) back to the Start point
    */
    private void CreatePath()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = spawnPoint.transform.position;
        startPos.y = 0.0f;
        endPos.y = 0.0f;

        Vector3 d = endPos - startPos;
        float distance = d.magnitude;
        Vector3 dnorm = d.normalized;

        float step = 2.0f;
        
        for (float i = -6.0f; i < distance; i += step)
        {
            GameObject new_point = Instantiate(pathNode, startPos + dnorm * i, Quaternion.LookRotation(d, Vector3.up));
            path.Add(new_point);
        }
        GameObject last_point = Instantiate(startPoint, endPos, spawnPoint.transform.rotation);
        path.Add(last_point);
    }

    /*!
    \fn FadeEmptyWorldToScene();
    \brief Coroutine for fading out the empty world and fading in the Scenario
    */
    public IEnumerator FadeEmptyWorldToScene()
    {
        for (int c = 0; c <= 100; ++c)
        {
            colourGrading.contrast.value = -c;
            yield return new WaitForSeconds(0.02f);
        }

        foreach(GameObject g in path)
            Destroy(g);

        carPool.SetActive(true);
        carPool.GetComponent<CarSpawnController>().trySpawn = true;

        pedestal.SetActive(true);

        MeshRenderer[] meshes = GameObject.FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }

        GameObject[] trafficlightButtons = GameObject.FindGameObjectsWithTag("Traffic Light Button");
        foreach (GameObject trafficLightButton in trafficlightButtons)
        {
            trafficLightButton.GetComponent<AudioSource>().enabled = true;
        }

        emptyWorldGroundPlane.SetActive(false);

        for (int c = 0; c <= 100; ++c)
        {
            colourGrading.contrast.value = c - 100;
            yield return new WaitForSeconds(0.02f);
        }

        knocked = false;
        SceneController.instance.ReloadScenario();
    }
}
