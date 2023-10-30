using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData : MonoBehaviour
{
    public float Moisture { get; } // wilgotnoœæ powietrza
    public float Wind { get; } // prêdkoœæ wiatru 

    public EnvironmentData(float moisture, float wind) {
        Moisture = moisture;
        Wind = wind;
    }
}
