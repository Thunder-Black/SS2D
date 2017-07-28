using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(AI))]
public class DrawLineEditor : Editor
{
    void OnSceneGUI()
    {
        AI t = target as AI;

        if (t == null || t.waypointTr == null)
            return;

        Vector3 center = t.transform.position;
        Handles.color = Color.green;
        for (int i = 0; i < t.waypointTr.Length; i++)
        {
            if (t.waypointTr[i] != null)
                Handles.DrawLine(center, t.waypointTr[i].transform.position);
        }
        Handles.color = Color.clear;
    }
}