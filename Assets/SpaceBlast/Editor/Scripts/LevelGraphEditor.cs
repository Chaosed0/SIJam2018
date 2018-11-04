using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LevelGraph))]
public class LevelGraphEditor : Editor
{
    SerializedProperty serializedEdges;

    private void OnEnable()
    {
        serializedEdges = serializedObject.FindProperty("edges");
    }

    private void OnSceneGUI()
    {
        LevelGraph levelGraph = (LevelGraph)target;

        // Can't use GetNodes in edit mode
        foreach (Node node in levelGraph.GetComponentsInChildren<Node>())
        {
            Vector3 newPosition = Handles.PositionHandle(node.transform.position, Quaternion.identity);
            if (node.transform.position != newPosition)
            {
                Undo.RecordObject(node.transform, "Move node of LevelGraph");
                node.transform.position = newPosition;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Bake"))
        {
            BakeNav();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void BakeNav()
    {
        LevelGraph levelGraph = (LevelGraph)target;
        List<Node> nodes = new List<Node>(levelGraph.GetComponentsInChildren<Node>());

        serializedEdges.ClearArray();

        HashSet<Node> passedNodes = new HashSet<Node>();

        foreach (Node n1 in nodes)
        {
            passedNodes.Add(n1);
            foreach (Node n2 in nodes)
            {
                if (n1 == n2)
                {
                    continue;
                }

                if (passedNodes.Contains(n2))
                {
                    continue;
                }

                Vector3 relative = n2.transform.position - n1.transform.position;

                bool hit = Physics.BoxCast(n1.transform.position, new Vector3(0.5f, 0.5f, 0.01f), relative.normalized, Quaternion.LookRotation(relative.normalized), relative.magnitude, levelGraph.bakeLayerMask, QueryTriggerInteraction.Ignore);
                // bool hit = Physics.BoxCast(n1.transform.position, new Vector3(0.6f, 0.3f, 0.01f), relative.normalized, Quaternion.LookRotation(relative.normalized), relative.magnitude, ~0, QueryTriggerInteraction.Ignore);
                //bool hit = Physics.Raycast(n1.transform.position, relative.normalized, relative.magnitude, ~0, QueryTriggerInteraction.Ignore);
                if (hit)
                {
                    continue;
                }

                serializedEdges.InsertArrayElementAtIndex(serializedEdges.arraySize);
                SerializedProperty prop = serializedEdges.GetArrayElementAtIndex(serializedEdges.arraySize - 1);
                prop.FindPropertyRelative("node1").objectReferenceValue = n1;
                prop.FindPropertyRelative("node2").objectReferenceValue = n2;
            }
        }
    }
}

