using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

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
\file CarEngine.cs
\brief Class to describe the operations of vehicles
*/

public class CarEngine : MonoBehaviour
{
    #region class_variables
    [HideInInspector] public WheelCollider wheelFL;                             /**Front left wheel's collider component*/
    [HideInInspector] public WheelCollider wheelFR;                             /**Front right wheel's collider component*/
    [HideInInspector] public WheelCollider wheelRL;                             /**Rear left wheel's collider component*/
    [HideInInspector] public WheelCollider wheelRR;                             /**Rear right wheel's collider component*/
    [HideInInspector] public Vector3 centerOfMass;                              /**The vehicle's center of mass*/
    [HideInInspector] public Rigidbody rb;                                      /**The vehicle's Rigidbody component*/

    [SerializeField] SensorBoundingBox sensorNear;
    [SerializeField] SensorBoundingBox sensorFar;

    public GameObject FLWheel;                                                  /**Front left wheel*/
    public GameObject FRWheel;                                                  /**Front right wheel*/
    public GameObject RLWheel;                                                  /**Rear left wheel*/
    public GameObject RRWheel;                                                  /**Rear right wheel*/

    [HideInInspector] public AudioSource horn;                                  /**The vehicle's AudioSource component*/
    public AudioClip clip;                                                      /**The AudioClip for the car's horn*/
    public int secondsBetweenHorn;                                              /**The seconds inbetween each car horn*/
    public AudioClip carScreech;												/**The AudioClip for the car's screech*/
	private bool screechFlag = true;											/**The flag to check if the car is screeching*/
    private bool hornFlag = true;
    public GameObject headLightLeft;                                            /**The vehicle's left light*/
    public GameObject headLightRight;                                           /**The vehicle's right light*/

    public float maxSpeed = 50.0f;                                              /**The maximum speed*/
    [HideInInspector] public float maxSteerAngle;                               /**The max steer angle*/
    [HideInInspector] public float currentSteerAngle;                           /**The current steer angle*/
    [HideInInspector] public float initialSpeed;                                /**The initial speed*/
    [HideInInspector] public float slowSpeed;                                   /**The speed in slow zones*/
    [HideInInspector] public float brakeSpeed;                                  /**The brake speed*/
    [HideInInspector] public float currentMaxSpeed;                             /**Stores the current maximum speed the vehicle can travel at*/
    [HideInInspector] public float maxBrakeSpeed;                               /**The maximum brake speed*/
    [HideInInspector] public float acceleration;                                /**The acceleration for this vehicle*/
    [HideInInspector] public float actualSpeed;                                 /**Stores the current actual speed of the vehicle*/
    [HideInInspector] public bool isBraking = false;                            /**Bool for if this vehicle is braking*/
    [HideInInspector] public bool slowDown = false;                             /**Bool for if this vehicle is slowing down*/
    [HideInInspector] public bool emergencyBrake = false;                       /**Bool for if this vehicle is E-braking*/

    [HideInInspector] public GameObject path;                                   /**The path this vehicle will take*/
    [HideInInspector] public List<Waypoint> nodes;
    [HideInInspector] public int currentNode;                                   /**index of the current node in the nodes list*/

    [HideInInspector] public GameObject trafficLight;                           /**The traffic light along the vehicle's path*/
    [HideInInspector] public float trafficDist = 0.0f;                          /**The stopping distance at the traffic light*/
    [HideInInspector] public float carToCarBrakingDistance = 6.0f;              /**The braking distance between cars*/

    [HideInInspector] public GameObject zebraCrossing;

    [HideInInspector] public List<GiveWay> giveWay = new List<GiveWay>();       /**List of bounding volumes to check for giving way logic*/
    [HideInInspector] public bool waitingToTurn = false;                        /**If this vehicle is currently waiting to turn*/
    [HideInInspector] public bool willTurn = false;                             /**If this vehicle is going to turn on this path*/
    #endregion class_variables

    /*!
    \fn ApplySteer();
    \brief Function for handling the steering logic for vehicles, mainly for path-finding to find the nearest node
    */
    protected void ApplySteer()
    {
        Vector3 carTransform = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 nodeTransform = new Vector3(nodes[currentNode].transform.position.x, 0, nodes[currentNode].transform.position.z);
        Vector3 carForward = new Vector3(-transform.forward.x, 0, -transform.forward.z);
        Vector3 dirToNode = (nodeTransform - carTransform).normalized;
        Vector3 cross = Vector3.Cross(carForward, dirToNode).normalized;
        //new steer contains the real angle
        float newSteer = Vector3.Angle(carForward, dirToNode);
        if (cross.y < 0)
        {
            newSteer = -newSteer;
        }

        if (Math.Abs(newSteer) > maxSteerAngle)
        {
            if (newSteer < 0)
            {
                newSteer = -maxSteerAngle;
            }
            else
            {
                newSteer = maxSteerAngle;
            }
        }

        currentSteerAngle = newSteer;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    /*!
    \fn SetPathNodes();
    \brief Function for populating the carpath list with nodes
    */
    public void SetPathNodes()
    {
        if (path.transform.childCount > 0)
        {
            Waypoint current = path.transform.GetChild(0).GetComponent<Waypoint>();
            currentNode = 0;
            nodes.Clear();
            nodes = new List<Waypoint>();

            while (current)
            {
                nodes.Add(current);
                current = current.nextWaypoint;
            }
        }
    }

    /*!
    \fn CarTrafficLightLogic();
    \brief Function for handling trafficlight logic
    */
    public void CarTrafficLightLogic()
    {
        if (!trafficLight)
            return;

        //car length is 5.5u
        float halfCarLength = 2.4f;

        //get car direction  -to know if its towards or opposite of the traffic light
        TrafficLight t = trafficLight.GetComponent<TrafficLight>();

        //get distance from the front of the car to the stop line
        Vector3 d = trafficLight.transform.position - transform.position;
        float d_len = Vector3.Dot(d, trafficLight.transform.right) - t.stopLineOffset - halfCarLength;
        Debug.Log("Length: " + d_len);
        if (d_len < 0.0f)
        {
            return;
        }

        //when traffic light turn yellow, car slowdown
        if (t.colour == TrafficLight.Colour.YELLOW)
        {
            if (d_len <= trafficDist * 2.0f)
            {
                slowDown = true;
            }
        }
        //when traffic light turn red, car starts to stop
        if (t.colour == TrafficLight.Colour.RED)
        {
            if (d_len <= trafficDist * 2.0f)
            {
                slowDown = true;
            }
            if (d_len <= trafficDist)
            {
                Debug.Log("EBraking!");
                emergencyBrake = true;
            }
        }
    }
    
    private void CarZebraCrossingLogic() 
    {
        
        if (!zebraCrossing)
            return;

        ZebraCrossing zc = zebraCrossing.GetComponent<ZebraCrossing>();

        if (sensorNear.isZC)
        {
            if (zc.isPlayer)
            {
                emergencyBrake = true;
            }
        }

        else if (sensorFar.isZC) 
        {
            if (zc.isPlayer)
            {
                slowDown = true;
            }
        }
    }

    /*!
    \fn CarLogic();
    \brief Function for handling vehicle logic
    */
    private void CarLogic()
    {
        //Normal car moving or if exit all boxes
        actualSpeed = rb.velocity.sqrMagnitude;
        //Max car speed is 50f
        if (actualSpeed >= currentMaxSpeed)
        {
            //Debug.Log("Limiting speed to 50");
            wheelFL.motorTorque = 0f;
            wheelFR.motorTorque = 0f;
            wheelRL.motorTorque = 0f;
            wheelRR.motorTorque = 0f;
        }
        else
        {
            CarInitialSpeed();
        }
    }

    /*!
    \fn CarPlayerLogic();
    \brief Function for handling vehicle-player logic
    */
    private void CarPlayerLogic()
    {
        if (sensorNear.isPlayer)
            emergencyBrake = true;
        else if 
            (sensorFar.isPlayer)
            slowDown = true;
    }

    /*!
    \fn CarHorningLogic();
    \brief Function for handling vehicle's horning logic
    */
    protected void CarHorningLogic()
    {
        if (trafficLight)
        {
            TrafficLight t = trafficLight.GetComponent<TrafficLight>();

            bool isPlayer = (sensorFar.isPlayer || sensorNear.isPlayer);

            if (t.colour == TrafficLight.Colour.GREEN && isPlayer)
            {
                if (gameObject.activeSelf && !horn.isPlaying)
				{
	                if(screechFlag)
					{
	                	horn.PlayOneShot(carScreech, 1.0f);
						screechFlag = false;
					}
                    if (hornFlag) 
                    {
                        horn.PlayOneShot(clip, 1.0f);
                        hornFlag = false;
                        StartCoroutine(HornFlagTimer(secondsBetweenHorn));
                    }
                    
				}
            }
            if (!isPlayer || t.colour != TrafficLight.Colour.GREEN)
            {
                if (horn.isPlaying)
				{
                    horn.Stop();
			        screechFlag = true;
                    hornFlag = true;
				}
            }
        }
    }

    private IEnumerator HornFlagTimer(int seconds) 
    {
        yield return new WaitForSeconds(seconds);
        hornFlag = true;
    }

    /*!
    \fn CarSignalLogic();
    \brief Function for handling vehicle's signalling logic when making turns
    */
    private void CarSignalLogic()
    {
        if (!headLightLeft || !headLightRight)
            return;

        if (currentNode >= nodes.Count)
            return;

        if (nodes[currentNode].leftTurnStart)
        {
            headLightLeft.GetComponent<CarSignal>().On();
        }
        if (nodes[currentNode].rightTurnStart)
        {
            headLightRight.GetComponent<CarSignal>().On();
        }
        if (nodes[currentNode].turnEnd)
        {
            headLightLeft.GetComponent<CarSignal>().Off();
            headLightRight.GetComponent<CarSignal>().Off();
        }
    }

    /*!
    \fn CarBrakeLogic();
    \brief Function for handling vehicle braking logic
    */
    public void CarBrakeLogic()
    {
        slowDown = false;
        isBraking = false;
        emergencyBrake = false;

        CarSensors();
        CarGiveWayLogic();
        CarTrafficLightLogic();
        CarZebraCrossingLogic();
        CarPlayerLogic();

        if (emergencyBrake)
            CarEBraking();
        else if (slowDown)
            CarBraking();
        else
            CarLogic();
    }

    /*!
    \fn GiveWayLogic();
    \brief Function for handling vehicle checking other lanes to give right of way
    */
    protected void CarGiveWayLogic()
    {
        if (!waitingToTurn)
            return;

        bool brake = false;
        foreach (GiveWay g in giveWay)
        {
            if (g.carIsPresent)
            {
                if (g.cars.Count == 1 && g.cars.Contains(gameObject))
                {
                    break;
                }

                brake = true;
                break;
            }
        }
        if (brake)
        {
            slowDown = true;
        }
    }

    /*!
    \fn CarSensors();
    \brief Function for performing sensor checking infront of the vehicle
    */
    private void CarSensors()
    {
        Vector3 direction1 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;
        Vector3 direction2 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;
        Vector3 direction3 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;
        Vector3 startPos = transform.position - transform.forward * 2.0f;
        actualSpeed = rb.velocity.sqrMagnitude;
        float baseDistance = 3.0f; // need to add some base valus if not the ray cast is length 0 when the car is not moving
        carToCarBrakingDistance = actualSpeed * 0.06f + baseDistance;
        float halfCarWidth = 0.95f;
        Vector3 startPos2 = startPos + transform.right * halfCarWidth;
        Vector3 startPos3 = startPos - transform.right * halfCarWidth;

        RaycastHit hitPayload;
        RaycastHit hitPayload2;
        RaycastHit hitPayload3;
        bool hit1 = Physics.Raycast(startPos, direction1, out hitPayload, carToCarBrakingDistance, -1, QueryTriggerInteraction.Ignore);
        bool hit2 = Physics.Raycast(startPos2, direction2, out hitPayload2, carToCarBrakingDistance, -1, QueryTriggerInteraction.Ignore);
        bool hit3 = Physics.Raycast(startPos3, direction3, out hitPayload3, carToCarBrakingDistance, -1, QueryTriggerInteraction.Ignore);

        if (hit2)
            hitPayload = hitPayload2;
        else if (hit3)
            hitPayload = hitPayload3;

        if (hit1 || hit2 || hit3)
        {
            if (hitPayload.collider.CompareTag("Auto Car") ||
                hitPayload.collider.CompareTag("Normal Car"))
            {
                if (hitPayload.distance < carToCarBrakingDistance)
                {
                    slowDown = true;
                }
                else if (hitPayload.distance < carToCarBrakingDistance * 0.5f)
                {
                    emergencyBrake = true;
                }
            }
        }
    }

    /*!
    \fn OnDrawGizmos();
    \brief Called for drawing in gizmos for debugging
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 startPos = transform.position - transform.forward * 2.0f;
        Vector3 direction1 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;
        Vector3 direction2 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;
        Vector3 direction3 = Quaternion.Euler(0.0f, currentSteerAngle, 0.0f) * -transform.forward;

        float halfCarWidth = 0.95f;
        Vector3 startPos2 = startPos + transform.right * halfCarWidth;
        Vector3 startPos3 = startPos - transform.right * halfCarWidth;

        Gizmos.DrawLine(startPos, startPos + direction1 * carToCarBrakingDistance);
        Gizmos.DrawLine(startPos2, startPos2 + direction2 * carToCarBrakingDistance);
        Gizmos.DrawLine(startPos3, startPos3 + direction3 * carToCarBrakingDistance);
    }

    /*!
    \fn CheckWaypointDistance();
    \brief Function for checking the distance of the current node and if the node is lower than 1f then switch to the next node
    */
    public void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].transform.position) < 2f)
        {
            if (currentNode < nodes.Count)
            {
                CarSignalLogic(); // check if need to signal
                currentNode++;

                if (currentNode == nodes.Count)
                {
                    gameObject.SetActive(false);
                    currentNode = 0;
                }
            }
        }
    }

    /*!
    \fn WheelUpdate();
    \brief Function for turning the wheel mesh according to the rotation of the wheel collider
    */
    public void WheelUpdate()
    {
        FLWheel.transform.Rotate(wheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        FRWheel.transform.Rotate(wheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        RLWheel.transform.Rotate(wheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        RRWheel.transform.Rotate(wheelRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }

    /*!
    \fn CarEBraking();
    \brief Function for changing all torques to 0 and brake to maxBrakeSpeed, Simulates a stoppage
    */
    private void CarEBraking()
    {
        wheelFL.motorTorque = 0f;
        wheelFR.motorTorque = 0f;
        wheelRL.motorTorque = 0f;
        wheelRR.motorTorque = 0f;
        wheelFL.brakeTorque = maxBrakeSpeed;
        wheelFR.brakeTorque = maxBrakeSpeed;
        wheelRL.brakeTorque = maxBrakeSpeed;
        wheelRR.brakeTorque = maxBrakeSpeed;

        isBraking = true;
    }

    /*!
    \fn CarBraking();
    \brief Function for changing all torques to 0 and brake to normalbrakeSpeed, Simulates a normal braking
    */
    public void CarBraking()
    {
        wheelFL.motorTorque = 0f;
        wheelFR.motorTorque = 0f;
        wheelRL.motorTorque = 0f;
        wheelRR.motorTorque = 0f;
        wheelFL.brakeTorque = brakeSpeed;
        wheelFR.brakeTorque = brakeSpeed;
        wheelRL.brakeTorque = brakeSpeed;
        wheelRR.brakeTorque = brakeSpeed;

        isBraking = true;
    }

    /*!
    \fn CarInitialSpeed();
    \brief Function for moving the car at the initial car speed
    */
    private void CarInitialSpeed()
    {
        wheelFL.motorTorque = initialSpeed;
        wheelFR.motorTorque = initialSpeed;
        wheelRL.motorTorque = initialSpeed;
        wheelRR.motorTorque = initialSpeed;
        wheelFL.brakeTorque = 0f;
        wheelFR.brakeTorque = 0f;
        wheelRL.brakeTorque = 0f;
        wheelRR.brakeTorque = 0f;

        isBraking = false;
    }
}
