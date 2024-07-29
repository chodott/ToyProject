using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;

public class MapEditor : EditorWindow
{
    private GameObject SpawnPrefab;
    private GameObject CurOnObject;
    private float Gap;

    [MenuItem("Tools/Object Place Tool")]
    public static void ShowWindow()
    {
        GetWindow<MapEditor>("Object Place Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Place Objects in Scene", EditorStyles.boldLabel);
        SpawnPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", SpawnPrefab, typeof(GameObject), false);
        Gap = EditorGUILayout.FloatField("Gap", Gap);
        if (GUILayout.Button("Enable Placement Mode"))
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        if (GUILayout.Button("Disable Placement Mode"))
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.button != 0) return;
        if (SpawnPrefab == null) return;
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject == CurOnObject) { }
                else
                {
                    Vector3 spawnPos = hit.transform.position;
                    GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(SpawnPrefab);

                    float x = Mathf.Abs(hit.normal.x);
                    float y = Mathf.Abs(hit.normal.y);
                    float z = Mathf.Abs(hit.normal.z);
                    float max = Mathf.Max(x, y, z);

                    if (x == max) spawnPos.x += Mathf.Sign(hit.normal.x) * Gap;
                    else if (y == max) spawnPos.y += Mathf.Sign(hit.normal.y) * Gap;
                    else spawnPos.z += Mathf.Sign(hit.normal.z) * Gap;

                    newObject.transform.position = spawnPos;
                    Undo.RegisterCreatedObjectUndo(newObject, "Create " + newObject.name);

                    CurOnObject = newObject;
                }
            }

            e.Use();
        }
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
