using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProcedureGenerator))]
public class ProGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProcedureGenerator proGen = (ProcedureGenerator)target;

        if(GUILayout.Button("Generate"))
        {
            proGen.GenerateBuilding();
        }

        if(GUI.changed)
        {
            //proGen.GenerateBuilding();
        }
    }
}
