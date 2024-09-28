using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRadarController : MonoBehaviour
{
    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart;
    private void Start()
    {
        Stats stats = new Stats(5, 5, 5, 5, 5);

        uiStatsRadarChart.SetStats(stats);

    }
}
