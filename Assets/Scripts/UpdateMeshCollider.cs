using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class UpdateMeshCollider : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    private void LateUpdate()
    {
        if (meshFilter.sharedMesh != null)
        {
            meshCollider.sharedMesh = null; // Clear the current mesh
            meshCollider.sharedMesh = meshFilter.sharedMesh; // Update with the new mesh
        }
    }
}
