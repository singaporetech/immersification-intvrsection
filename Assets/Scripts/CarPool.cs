using System.Collections;
using System.Collections.Generic;
using System.IO;
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
\file CarPool.cs
\brief Class to control and define the logic of the object pool of car objects.
    Use of object pooling is to steer away from calling Instantiate and Destroy, 
    having the objects already loaded in the scene and just enabling them when needed should 
    increase performance instead of allocating and deallocating objects in memory often at runtime.
*/

public class CarPool : MonoBehaviour
{
    [SerializeField] GameObject[] autoPool;         /**The object pool of autonomus vehicles*/
    [SerializeField] GameObject[] normalPool;       /**The object pool of normal vehicles*/
    public GameObject[] spawnPoints;                /**A list of spawnpoints in the scene*/

    [HideInInspector] public int carAmount;         /**Number of cars in the scene, set by CarSpawnController class*/
    [HideInInspector] public float timeDelay;       /**Delay between car spawns, set by CarSpawnController class*/
    
    /*!
    \fn SpawnCars(bool isAuto);
    \brief Public function to trigger the spawning of cars into the scene
    \param isAuto Bool for whether to spawn autonomous vehicles or normal vehicles
    */
    public void SpawnCars(bool isAuto)
    {
        StartCoroutine(TrySpawnCarAtRandomSpawnPoint(carAmount, timeDelay, isAuto));
    }

    /*!
    \fn AreSpawnPointsAvailable();
    \brief Queries whether all spawnpoints are blocked
    \return Returns true if there is at least one spawnpoint that is not blocked
    */
    private bool AreSpawnPointsAvailable()
    {
        foreach (GameObject s in spawnPoints)
        {
            if (!s.GetComponent<CarSpawnPoint>().blocked &&
                s.GetComponent<CarSpawnPoint>().automaticSpawning)
                return true;
        }
        return false;
    }

    /*!
    \fn DisableAllCars();
    \brief Disables all the vehicles in the scene
    */
    public void DisableAllCars()
    {
        foreach (GameObject a in autoPool)
            a.SetActive(false);
        foreach (GameObject n in normalPool)
            n.SetActive(false);
    }

    /*!
    \fn TrySpawnCar(int spawnpointIndex, bool isAuto);
    \brief Tries to spawn a vehicle at a given spawnpoint
    \param spawnpointIndex The index of the spawnpoint to spawn a car at
    \param isAuto Bool for whether to spawn autonomous vehicles or normal vehicles
    */
    public void TrySpawnCar(int spawnpointIndex, bool isAuto)
    {
        StartCoroutine(TrySpawnCar(spawnpointIndex, 1.0f, isAuto));
    }

    /*!
    \fn SpawnCar(int spawnpointIndex, bool isAuto);
    \brief Spawns a vehicle at a given spawnpoint
    \param spawnpointIndex The index of the spawnpoint to spawn a car at
    \param isAuto Bool for whether to spawn autonomous vehicles or normal vehicles
    \return Returns the car object spawned
    */
    public GameObject SpawnCar(int spawnpointIndex, bool isAuto)
    {
        GameObject spawnPoint = spawnPoints[spawnpointIndex];
        GameObject car = null;

        if (isAuto)
        {
            for (int i = 0; i < autoPool.Length; i++)
            {
                if (!autoPool[i].activeSelf)
                {
                    car = autoPool[i];
                    AutoCarEngine e = car.GetComponent<AutoCarEngine>();
                    CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                    e.path = c_sp.path;
                    e.SetPathNodes();
                    e.trafficLight = c_sp.trafficLight;
                    e.giveWay = c_sp.giveWay;
                    e.willTurn = c_sp.turn;
                    e.currentMaxSpeed = e.maxSpeed;
                    e.zebraCrossing = c_sp.zebraCrossing;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < normalPool.Length; i++)
            {
                if (!normalPool[i].activeSelf)
                {
                    car = normalPool[i];
                    NormalCarEngine e = car.GetComponent<NormalCarEngine>();
                    CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                    e.path = c_sp.path;
                    e.SetPathNodes();
                    e.trafficLight = c_sp.trafficLight;
                    e.giveWay = c_sp.giveWay;
                    e.willTurn = c_sp.turn;
                    e.currentMaxSpeed = e.maxSpeed;
                    e.zebraCrossing = c_sp.zebraCrossing;
                    break;
                }
            }
        }
        if (car == null)
            return null;

        car.transform.position = spawnPoint.transform.position;
        car.transform.rotation = spawnPoint.transform.rotation;
        car.SetActive(true);
        return car;
    }

    /*!
    \fn TrySpawnCar(int spawnpointIndex, float delay, bool isAuto);
    \brief Coroutine to try to spawning a vehicle at a given spawnpoint every delay seconds
    \param spawnpointIndex The index of the spawnpoint to spawn a car at
    \param delay The timeDelay between spawn tries
    \param isAuto Bool for whether to spawn autonomous vehicles or normal vehicles
    */
    private IEnumerator TrySpawnCar(int spawnpointIndex, float delay, bool isAuto)
    {
        GameObject spawnPoint = spawnPoints[spawnpointIndex];

        if (spawnPoint.GetComponent<CarSpawnPoint>().blocked)
            yield return new WaitForSeconds(delay);

        GameObject car = null;

        if (isAuto)
        {
            for (int i = 0; i < autoPool.Length; i++)
            {
                if (!autoPool[i].activeSelf)
                {
                    car = autoPool[i];
                    AutoCarEngine e = car.GetComponent<AutoCarEngine>();
                    CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                    e.path = c_sp.path;
                    e.SetPathNodes();
                    e.trafficLight = c_sp.trafficLight;
                    e.giveWay = c_sp.giveWay;
                    e.willTurn = c_sp.turn;
                    e.currentMaxSpeed = e.maxSpeed;
                    e.zebraCrossing = c_sp.zebraCrossing;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < normalPool.Length; i++)
            {
                if (!normalPool[i].activeSelf)
                {
                    car = normalPool[i];
                    NormalCarEngine e = car.GetComponent<NormalCarEngine>();
                    CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                    e.path = c_sp.path;
                    e.SetPathNodes();
                    e.trafficLight = c_sp.trafficLight;
                    e.giveWay = c_sp.giveWay;
                    e.willTurn = c_sp.turn;
                    e.currentMaxSpeed = e.maxSpeed;
                    e.zebraCrossing = c_sp.zebraCrossing;
                    break;
                }
            }
        }
        if (car == null)
            yield break;

        car.transform.position = spawnPoint.transform.position;
        car.transform.rotation = spawnPoint.transform.rotation;
        car.SetActive(true);
    }

    /*!
    \fn TrySpawnCarAtRandomSpawnPoint(int count, float delay, bool isAuto);
    \brief Coroutine to handle the spawning of cars with timeDelay delay between spawns
    \param count The number of cars in the spawn pack
    \param delay The timeDelay between spawns
    \param isAuto Bool for whether to spawn autonomous vehicles or normal vehicles
    */
    private IEnumerator TrySpawnCarAtRandomSpawnPoint(int count, float delay, bool isAuto)
    {
        for (int j = 0; j < count; j++)
        {
            if (!AreSpawnPointsAvailable())
                yield break;

            GameObject spawnPoint = null;

            while (spawnPoint == null)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                if (!spawnPoints[randomIndex].GetComponent<CarSpawnPoint>().blocked &&
                    spawnPoints[randomIndex].GetComponent<CarSpawnPoint>().automaticSpawning)
                    spawnPoint = spawnPoints[randomIndex];
            }

            GameObject car = null;

            if (isAuto)
            {
                for (int i = 0; i < autoPool.Length; i++)
                {
                    if (!autoPool[i].activeSelf)
                    {
                        car = autoPool[i];
                        AutoCarEngine e = car.GetComponent<AutoCarEngine>();
                        CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                        e.path = c_sp.path;
                        e.SetPathNodes();
                        e.trafficLight = c_sp.trafficLight;
                        e.giveWay = c_sp.giveWay;
                        e.willTurn = c_sp.turn;
                        e.currentMaxSpeed = e.maxSpeed;
                        e.zebraCrossing = c_sp.zebraCrossing;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < normalPool.Length; i++)
                {
                    if (!normalPool[i].activeSelf)
                    {
                        car = normalPool[i];
                        NormalCarEngine e = car.GetComponent<NormalCarEngine>();
                        CarSpawnPoint c_sp = spawnPoint.GetComponent<CarSpawnPoint>();
                        e.path = c_sp.path;
                        e.SetPathNodes();
                        e.trafficLight = c_sp.trafficLight;
                        e.giveWay = c_sp.giveWay;
                        e.willTurn = c_sp.turn;
                        e.currentMaxSpeed = e.maxSpeed;
                        e.zebraCrossing = c_sp.zebraCrossing;
                        break;
                    }
                }
            }
            if (car == null)
                yield break;

            car.transform.position = spawnPoint.transform.position;
            car.transform.rotation = spawnPoint.transform.rotation;
            car.SetActive(true);

            yield return new WaitForSeconds(delay);
        }
    }
}
