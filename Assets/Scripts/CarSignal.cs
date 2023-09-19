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
\file CarSignal.cs
\brief Class to describe the operations of vehicle signals
*/

public class CarSignal : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Material m_SignalOff;
    public Material m_SignalOn;
    bool on = false;

    public enum CarType
    {
        NORMAL = 0,
        AUTO
    }
    public CarType type;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Off()
    {
        switch (type)    
        { 
            case CarType.NORMAL:
                OffNormalCar();
                break;
            case CarType.AUTO:
                OffAutoCar();
                break;
        };
    }

    public void On()
    {
        on = true;
        switch (type)
        {
            case CarType.NORMAL:
                StartCoroutine(FlashNormalCar());
                break;
            case CarType.AUTO:
                StartCoroutine(FlashAutoCar());
                break;
        };
    }

    private void OffAutoCar()
    {
        Material[] materialsArray = meshRenderer.materials;
        materialsArray[3] = m_SignalOff;
        meshRenderer.materials = materialsArray;
        on = false;
    }

    private void OffNormalCar()
    {
        Material[] materialsArray = meshRenderer.materials;
        for (int i = 0; i < materialsArray.Length; ++i)
            materialsArray[i] = m_SignalOff;
        meshRenderer.materials = materialsArray;
        on = false;
    }

    private IEnumerator FlashAutoCar()
    {
        if (!on)
            yield break;

        Material[] materialsArray = meshRenderer.materials;

        materialsArray[3] = m_SignalOn;
        meshRenderer.materials = materialsArray;
        yield return new WaitForSeconds(0.5f);
        materialsArray[3] = m_SignalOff;
        meshRenderer.materials = materialsArray;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FlashAutoCar());
    }

    private IEnumerator FlashNormalCar()
    {
        if (!on)
            yield break;

        Material[] materialsArray = meshRenderer.materials;

        for (int i = 0; i < materialsArray.Length; ++i)
            materialsArray[i] = m_SignalOn;
        meshRenderer.materials = materialsArray;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < materialsArray.Length; ++i)
            materialsArray[i] = m_SignalOff;
        meshRenderer.materials = materialsArray;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FlashNormalCar());
    }
}
