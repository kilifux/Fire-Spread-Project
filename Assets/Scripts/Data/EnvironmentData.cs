using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData : MonoBehaviour
{
    public float Moisture { get; } // wilgotno�� powietrza
    public float Wind { get; } // pr�dko�� wiatru 

    public EnvironmentData(float moisture, float wind) {
        Moisture = moisture;
        Wind = wind;
    }
}
