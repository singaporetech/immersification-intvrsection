#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
\file WaypointManagerWindow.cs
\brief Class for creating an editor window to create and delete waypoints
*/

public class WaypointManagerWindow : EditorWindow
{
    /*!
    \fn WaypointEditor();
    \brief Static function for creating the window
    */
    [MenuItem("Tools/Waypoint Editor")]
    public static void WaypointEditor()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;      /**The root parent object to store all the waypoints as children in*/

    /*!
    \fn OnGUI();
    \brief Called on GUI event
    */
    private void OnGUI()
    {
        SerializedObject serializedObject = new SerializedObject(this);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }
        serializedObject.ApplyModifiedProperties();
    }

    /*!
    \fn DrawButtons();
    \brief Function for creating the buttons in the window
    */
    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Delete Waypoint"))
            {
                DeleteWaypoint();
            }
        }
    }

    /*!
    \fn CreateWaypoint();
    \brief Function for creating a waypoint
    */
    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if (waypointRoot.childCount > 1)
        {
            waypoint.prevWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.prevWaypoint.nextWaypoint = waypoint;
            waypoint.transform.position = waypoint.prevWaypoint.transform.position;
        }
        Selection.activeObject = waypoint.gameObject;
    }

    /*!
    \fn CreateWaypoint();
    \brief Function for creating a waypoint after the selected waypoint
    */
    private void CreateWaypointAfter()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
        if (selectedWaypoint.nextWaypoint == null)
            return;

        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Vector3 d = selectedWaypoint.nextWaypoint.transform.position - selectedWaypoint.transform.position;
        float distance = d.magnitude;
        distance *= 0.5f;
        waypointObject.transform.position = selectedWaypoint.transform.position + d.normalized * distance;

        waypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        waypoint.prevWaypoint = selectedWaypoint;
        selectedWaypoint.nextWaypoint.prevWaypoint = waypoint;
        selectedWaypoint.nextWaypoint = waypoint;

        waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex() + 1);

        Selection.activeObject = waypoint.gameObject;
        UpdateWaypointNames();
    }

    /*!
    \fn CreateWaypoint();
    \brief Function for deleting a waypoint
    */
    private void DeleteWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.prevWaypoint = selectedWaypoint.prevWaypoint;
        }
        if (selectedWaypoint.prevWaypoint != null)
        {
            selectedWaypoint.prevWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        }
        if (selectedWaypoint.prevWaypoint)
            Selection.activeObject = selectedWaypoint.prevWaypoint;
        DestroyImmediate(selectedWaypoint.gameObject);
        UpdateWaypointNames();
    }

    /*!
    \fn CreateWaypoint();
    \brief Function for updating the waypoint names in the hierachy
    */
    private void UpdateWaypointNames()
    {
        int count = 0;
        foreach (Transform waypoint in waypointRoot)
        {
            waypoint.name = "Waypoint " + count++;
        }
    }
}
#endif
