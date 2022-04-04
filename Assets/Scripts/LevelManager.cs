using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    [Header("PostProcess")]
    public Volume postProcessVolcano;
    public Volume postProcessForest;
    public Volume postProcessCity;

    void Start()
    {
        
    }
}
