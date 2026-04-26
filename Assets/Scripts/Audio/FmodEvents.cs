using System;
using FMODUnity;
using UnityEngine;

public class FmodEvents : MonoBehaviour
{
    [Header ("Ambience")]
    public EventReference ambience;
    [Header ("PlayerSFX")]
    public EventReference steps;
    [Header("Car")]
    public EventReference carWheels;
    public EventReference handBreak;
    public EventReference carDoorOpens;
    public EventReference carDoorCloses;
    public static FmodEvents Instance { private set; get;}

    void Awake()
    {
        if (Instance!=null&&Instance!=this)
        {
            Debug.LogWarning("More than one FmodEvents");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
