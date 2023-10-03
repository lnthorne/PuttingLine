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
        meshManager.enabled = true;
        isCollecting = true;
        Debug.Log("StartCollection");
    }

    public void StopCollection()
    {
        meshManager.enabled = false;
        isCollecting = false;
        Debug.Log(collectedMeshData.Count);
    }


    public void RemoveAllMeshes()
    {
        var arSession = FindObjectOfType<ARSession>();
        if(arSession)
        {
            arSession.Reset();
        }
        if (meshManager)
        {
            Debug.Log("Number of meshes before destruction: " + meshManager.meshes.Count);

            meshManager.subsystem.Stop();
            meshManager.DestroyAllMeshes();
            meshManager.subsystem.Start();
        }
    }
}
