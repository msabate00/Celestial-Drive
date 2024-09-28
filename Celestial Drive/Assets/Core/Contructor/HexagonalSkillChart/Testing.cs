using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    private void Start()
    {
        Stats stats = new Stats(150, 200, 100, 150, 250);

        uiStatsRadarChart.SetStats(stats);

    }
}

 

