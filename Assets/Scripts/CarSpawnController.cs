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
\file CarSpawnController.cs
\brief Class to cotrol the logic of car spawning in the scene
*/

public class CarSpawnController : MonoBehaviour
{
    [HideInInspector] public bool trySpawn = true;      /**Lock for trying to spawn cars*/
    float totalSpawnTime;                               /**Total time it takes for a pack of cars to spawn*/

    public float spawnDelay;                            /**Delay time betwwen pack spawns*/
    public float spawnTime;                             /**Delay time between car spawns*/
    public int spawnCount;                              /**Number of cars in a pack*/
    [Range(0f, 1f)]
    public float spawnRate;                             /**Percentage likelyhood of spawns, between 0 to 1*/

    CarPool pool;                                       /**Cache memory for the script*/
    bool isAuto = true;                                /**Bool for if the car to spawn is autonomous or normal*/

    /*!
    \fn UpdateSceneVariables();
    \brief Function to update the carpool variables with the proper settings
    */
    void UpdateSceneVariables()
    {
        pool.carAmount = spawnCount;
        pool.timeDelay = spawnDelay;
    }

    /*!
    \fn Start();
    \brief Called once in the first frame
    */
    void Start()
    {
        totalSpawnTime = spawnTime * spawnCount + spawnDelay;
        pool = gameObject.GetComponent<CarPool>();
        UpdateSceneVariables();
    }

    /*!
    \fn Update();
    \brief Called once per frame
    */
    void Update()
    {
        if (trySpawn && pool)
            StartCoroutine(CarSpawnChance(spawnRate));
    }

    /*!
    \fn CarSpawnChance(float percentChance);
    \brief Function that tries to spawn a pack of cars every spawnDelay seconds
    \      20/02/23 - WZF, Modified to make sure it only spawns AVs.
    */
    private IEnumerator CarSpawnChance(float percentChance)
    {
        trySpawn = false;

        float res = Random.Range(0.0f, 1.0f);
        if (res < percentChance)
        {
            pool.SpawnCars(isAuto);
            //isAuto = !isAuto;

            yield return WaitForCarSpawns();
        }
        else
        {
            yield return new WaitForSeconds(spawnDelay);
            trySpawn = true;
        }
    }

    /*!
    \fn WaitForCarSpawns();
    \brief Coroutine that waits so that cars do not spawn on top of one another
    */
    private IEnumerator WaitForCarSpawns()
    {
        yield return new WaitForSeconds(totalSpawnTime);
        trySpawn = true;
    }
}
