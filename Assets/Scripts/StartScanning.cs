using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartScanning : MonoBehaviour
{
    public MeshDataCollector meshDataCollector;
    public TextMeshProUGUI buttonText;
    private bool is_collecting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onScanningSelected() {
        if (!is_collecting) {
            is_collecting = true;
            meshDataCollector.StartCollection();
            buttonText.text = "Stop Scanning";
        } else {
            is_collecting = false;
            meshDataCollector.StopCollection();
            buttonText.text = "Start Scanning";
        }
    }
}
