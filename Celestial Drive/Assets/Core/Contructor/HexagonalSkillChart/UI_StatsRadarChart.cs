using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRadarChart : MonoBehaviour
{

    [SerializeField] private Material renderMaterial;
    [SerializeField] private Texture2D renderTexture2D;

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;

    private void Awake()
    {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();   
    }

    public void SetStats(Stats stats) {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, EventArgs e)
    {
        UpdateStatsVisual();
    }
    private void Update()
    {
        //stats.AddStatAmount(Stats.Type.Health, 1);
        //stats.AddStatAmount(Stats.Type.Damage , -1);
    }


    private void UpdateStatsVisual() {

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];


        float angleIncrement = -360f / 5;
        float radarChartSize = 100f;


        Vector3 healthVertex = Quaternion.Euler(0, 0, angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Health);
        int healthVertexIndex = 1;


        Vector3 damageVertex = Quaternion.Euler(0, 0, angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Damage);
        int damageVertexIndex = 2;

        Vector3 powerVertex = Quaternion.Euler(0, 0, angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Power);
        int powerVertexIndex = 3;

        Vector3 mobilityVertex = Quaternion.Euler(0, 0, angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Mobility);
        int mobilityVertexIndex = 4;

        Vector3 speedVertex = Quaternion.Euler(0, 0, angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Speed);
        int speedVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[healthVertexIndex]     = healthVertex;
        vertices[damageVertexIndex]     = damageVertex;
        vertices[powerVertexIndex]      = powerVertex;
        vertices[mobilityVertexIndex]   = mobilityVertex;
        vertices[speedVertexIndex]      = speedVertex;

        uv[0] = Vector2.zero;
        uv[healthVertexIndex] = Vector2.one;
        uv[damageVertexIndex]       = Vector2.one;
        uv[powerVertexIndex]        = Vector2.one;
        uv[mobilityVertexIndex]     = Vector2.one;
        uv[speedVertexIndex]        = Vector2.one;



        triangles[0] = 0;
        triangles[1] = healthVertexIndex;
        triangles[2] = damageVertexIndex;

        triangles[3] = 0;
        triangles[4] = damageVertexIndex;
        triangles[5] = powerVertexIndex;

        triangles[6] = 0;
        triangles[7] = powerVertexIndex;
        triangles[8] = mobilityVertexIndex;

        triangles[9] = 0;
        triangles[10] = mobilityVertexIndex;
        triangles[11] = speedVertexIndex;

        triangles[12] = 0;
        triangles[13] = speedVertexIndex;
        triangles[14] = healthVertexIndex;


        



        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(renderMaterial, renderTexture2D);


        //transform.Find("attackBar").localScale = new Vector3(1, stats.GetStatAmountNormalized(Stats.Type.Attack), 1);
        //transform.Find("defenceBar").localScale = new Vector3(1, stats.GetStatAmountNormalized(Stats.Type.Defense), 1);
    }


    public void GetAndUpdateAll()
    {
        StatsObject[] gl = GameObject.FindObjectsOfType<StatsObject>();

        stats.SetStatAmount(Stats.Type.Health, 5);
        stats.SetStatAmount(Stats.Type.Damage, 5);
        stats.SetStatAmount(Stats.Type.Power, 5);
        stats.SetStatAmount(Stats.Type.Mobility, 5);
        stats.SetStatAmount(Stats.Type.Speed, 5);

        for (int i = 0; i < gl.Length; i++)
        {
            stats.AddStatAmount(Stats.Type.Health, gl[i].health);
            stats.AddStatAmount(Stats.Type.Damage, gl[i].damage);
            stats.AddStatAmount(Stats.Type.Power, gl[i].power);
            stats.AddStatAmount(Stats.Type.Mobility, gl[i].mobility);
            stats.AddStatAmount(Stats.Type.Speed, gl[i].speed);
        }
    }


}
