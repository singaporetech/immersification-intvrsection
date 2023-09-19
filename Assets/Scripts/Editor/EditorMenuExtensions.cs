#if UNITY_EDITOR
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

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
\file EditorMenuExtensions.cs
\brief Static class containing static functions for all custom menu extentions in the editor
*/

public static class EditorMenuExtensions
{
    /*!
    \fn Doxygen();
    \brief Called when the user clicks on Tools > Generate Doxygen
        Generates the Doxygen index file in avhmi > Doxygen > output > html
    */
    [MenuItem("Tools/Generate Doxygen")]
    public static void Doxygen()
    {
        string directory = Application.dataPath.Replace(@"/", @"\") + "\\..\\Doxygen";
        string batFile = "\\Run.bat";
        string command = directory + batFile + " " + directory;
        command = "-NoExit " + command;

        UnityEngine.Debug.Log(command);

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "powershell.exe";
        startInfo.Arguments = command;
        Process process = Process.Start(startInfo);
        process.WaitForExit();
        process.Close();
    }
}
#endif