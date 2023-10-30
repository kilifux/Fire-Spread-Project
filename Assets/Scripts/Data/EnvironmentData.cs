using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData : MonoBehaviour
{
    public float Moisture { get; }
    public float Wind { get; }

    public EnvironmentData(float moisture, float wind) {
        Moisture = moisture;
        Wind = wind;
    }
}
