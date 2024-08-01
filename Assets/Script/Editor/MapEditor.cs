using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Rendering;

public class MapEditor : EditorWindow
{
    private GameObject _spawnPrefab;
    private GameObject _curOnObject;
    private float _gap = 1;

    [MenuItem("Tools/Object Place Tool")]
    public static void ShowWindow()
    {
        GetWindow<MapEditor>("Object Place Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Place Objects in Scene", EditorStyles.boldLabel);
        _spawnPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _spawnPrefab, typeof(GameObject), false);
        _gap = EditorGUILayout.FloatField("Gap", _gap);
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
        if (_spawnPrefab == null) return;
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject == _curOnObject) { }
                else
                {
                    Vector3 spawnPos = hit.transform.position;
                    GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(_spawnPrefab);

                    //SetPosition
                    float x = Mathf.Abs(hit.normal.x);
                    float y = Mathf.Abs(hit.normal.y);
                    float z = Mathf.Abs(hit.normal.z);
                    float max = Mathf.Max(x, y, z);

                    if (x == max) spawnPos.x += Mathf.Sign(hit.normal.x) * _gap;
                    else if (y == max) spawnPos.y += Mathf.Sign(hit.normal.y) * _gap;
                    else spawnPos.z += Mathf.Sign(hit.normal.z) * _gap;
                    newObject.transform.position = spawnPos;

                    //SetRotation
                    float value = Vector3.Dot(newObject.transform.forward, SceneView.lastActiveSceneView.camera.transform.forward);
                    Vector3 crossProduct = Vector3.Cross(newObject.transform.forward, SceneView.lastActiveSceneView.camera.transform.forward);
                    if (Mathf.Abs(value) >= 0.5f)
                    {
                        if (crossProduct.y > 0) newObject.transform.Rotate(Vector3.up, 0.0f);
                        else newObject.transform.Rotate(Vector3.up, 180.0f);
                    }
                    else if (Mathf.Abs(value) < 0.5f)
                    {
                        if (crossProduct.y > 0) newObject.transform.Rotate(Vector3.up, 90.0f);
                        else newObject.transform.Rotate(Vector3.up, -90.0f);
                    }   

                    Undo.RegisterCreatedObjectUndo(newObject, "Create " + newObject.name);

                    _curOnObject = newObject;
                }
            }

            e.Use();
        }

        else _curOnObject = null;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
