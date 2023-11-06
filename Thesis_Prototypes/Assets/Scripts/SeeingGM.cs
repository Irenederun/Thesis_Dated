using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeeingGM : MonoBehaviour
{
    [Space(10f)]
    public Flowchart convo;

    [Space(10f)]
    public Stage currentStage;
    public List<Stage> stages;

    private void Awake()
    {
        if (Services.seeingGM == null) Services.seeingGM = this;
    }

    void Start()
    {
        currentStage.Entrance();
    }

    void Update()
    {

    }
}
