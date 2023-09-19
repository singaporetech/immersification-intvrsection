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
\file WaypointEditor.cs
\brief Class for gizmos functionality for paths and path nodes in the scene window
*/

[InitializeOnLoad()]
public class WaypointEditor
{
    /*!
    \fn OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType);
    \brief Static function for drawing the gizmos in the scene window
    \param waypoint A waypoint to draw
    \param gizmoType The type of gizmo to draw
    */
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(waypoint.transform.position, waypoint.nextWaypoint.transform.position);
        }
    }
}
#endif
