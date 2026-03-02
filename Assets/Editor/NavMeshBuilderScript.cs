#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

public class NavMeshBuilderScript
{
    [MenuItem("Tools/Build NavMesh Now")]
    public static void BuildNavMesh()
    {
        // 1. First, make sure the Plane is marked as Navigation Static
        GameObject plane = GameObject.Find("Plane");
        if (plane != null)
        {
            GameObjectUtility.SetStaticEditorFlags(plane, StaticEditorFlags.NavigationStatic);
            Debug.Log("Set Plane to Navigation Static.");
        }
        else
        {
            Debug.LogError("Could not find a GameObject named 'Plane' to set as Navigation Static.");
        }
        
        // 2. Build the NavMesh
        NavMeshBuilder.BuildNavMesh();
        Debug.Log("NavMesh built successfully!");
    }
}
#endif
