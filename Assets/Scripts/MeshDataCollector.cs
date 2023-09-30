using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class MeshDataCollector : MonoBehaviour
{
    private ARMeshManager meshManager;

    public List<MeshData> collectedMeshData = new List<MeshData>();

    private bool isCollecting = false;

    void Start() 
    {
        meshManager = gameObject.GetComponent<ARMeshManager>();
    }


    void Update()
    {
        if (meshManager && isCollecting)
        {
            UpdateMeshData();
        }
    }

    private void UpdateMeshData()
    {
        // Loop over each ARMesh
        foreach (var mesh in meshManager.meshes)
        {
            MeshData data = new MeshData();
            Mesh arMesh = mesh.GetComponent<MeshFilter>().mesh;

            data.vertices = arMesh.vertices;
            data.triangles = arMesh.triangles;
            data.normals = arMesh.normals;

            collectedMeshData.Add(data);
        }
    }

    public void StartCollection() 
    {
        collectedMeshData.Clear();
        meshManager.subsystem?.Start();
        meshManager.enabled = true;
        isCollecting = true;
        Debug.Log("StartCollection");
    }

    public void StopCollection()
    {
        meshManager.enabled = false;
        meshManager.subsystem?.Stop();
        isCollecting = false;
    }

    // TODO: Find out how to remove the previous meshes https://forum.unity.com/threads/how-to-reset-meshes-generated-by-armeshmanager.1451887/

    public void RemoveAllMeshes()
    {
        if (meshManager)
        {
            meshManager.DestroyAllMeshes();
            meshManager.subsystem?.Stop();
            meshManager.subsystem?.Start();
        }
    }
}
