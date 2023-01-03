using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;
using System.Numerics;

public class FilterFrustum
{
    private static bool isEnable = false;
    [MenuItem("Tools/Visualize Frustum")]
    public static void ToggleEnable()
    {
        isEnable= !isEnable;
        if(isEnable)
        {
            EditorApplication.update += OnSceneGUI;
        }
        else
        {
            EditorApplication.update -= OnSceneGUI;
            CancelHide();
        }
    }
    [MenuItem("Tools/Visualize Frustum", true)]
    public static bool ToggleEnableValidate()
    {
        Menu.SetChecked("Tools/Visualize Frustum", isEnable);
        return true;
    }
    public static void SelectObjectInFrustum(Camera camera)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        var hideObjs = new List<GameObject>();
        var showObjs = new List<GameObject>();
        foreach(var render in Object.FindObjectsOfType<Renderer>()) 
        {
            if (!GeometryUtility.TestPlanesAABB(planes, render.bounds))
            {
                hideObjs.Add(render.gameObject);
            }
            else
            {
                showObjs.Add(render.gameObject);
            }
        }

        SceneVisibilityManager.instance.Hide(hideObjs.ToArray(), true);
        SceneVisibilityManager.instance.Show(showObjs.ToArray(), true);
        //Selection.objects = hideObjs.ToArray();
    }

    private static void OnSceneGUI()
    {
        if (Selection.activeGameObject.TryGetComponent<Camera>(out var camera))
        {
            SelectObjectInFrustum(camera);
        }
    }

    private static void CancelHide()
    {
        var showObjs = new List<GameObject>();
        foreach (var render in Object.FindObjectsOfType<Renderer>())
        {
            showObjs.Add(render.gameObject);
        }
        SceneVisibilityManager.instance.Show(showObjs.ToArray(), true);
    }
}
