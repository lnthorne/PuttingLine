using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LineCalculations
{
    public Vector3 startPoint { get;  set; }
    public Vector3 endPoint { get; set; }
    public List<MeshData> collectedMeshData {get; set;}
    public float sampleRate = 0.1f;

    public List<float> sampledHeights = new List<float>();

     public void AnalyzePuttTerrain()
    {
        Vector3 direction = (endPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, endPoint);

        int numberOfSamples = Mathf.FloorToInt(distance / sampleRate);
        Debug.Log("Number of samples: " + numberOfSamples);
        Debug.Log("Distance: " + distance);
        Debug.Log("Direction " + direction);
        Debug.Log("Vertices: " + collectedMeshData[0].vertices.ToString());
        
        sampledHeights.Clear();

        for (int i = 0; i <= numberOfSamples; i++)
        {
            Vector3 samplePosition = startPoint + direction * sampleRate * i;
            float height = GetHeightAtPoint(samplePosition);
            sampledHeights.Add(height);
        }
    }

     private float GetHeightAtPoint(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        Vector3? closestVertex = null;

        // Iterate over collected mesh data
        foreach (var data in collectedMeshData)
        {
            foreach (var vertex in data.vertices)
            {
                float distance = Vector3.Distance(position, vertex);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVertex = vertex;
                }
            }
        }

        if (closestVertex.HasValue)
        {
            return closestVertex.Value.y;
        }
        else
        {
            Debug.LogWarning("No vertex found for sampled position!");
            return position.y;  // Return the original position's height as a fallback
        }
    }

    private List<float> ComputeSlopesFromSampledHeights()
    {
        List<float> slopes = new List<float>();

        for (int i = 0; i < sampledHeights.Count - 1; i++)
        {
            float deltaY = sampledHeights[i + 1] - sampledHeights[i];
            float slope = deltaY / sampleRate;
            slopes.Add(slope);
        }

        return slopes;
    }

    public Vector3 ComputeIdealPuttDirectionAndStrength()
    {
        List<float> slopes = ComputeSlopesFromSampledHeights();

        // Here, the stronger the accumulated value, the stronger the break.
        // Positive values mean the ground slopes upwards (relative to the direction of the putt), suggesting a right break.
        // Negative values mean the ground slopes downwards, suggesting a left break.
        float accumulatedBreak = slopes.Sum();
        Debug.Log("Accumulated Break: " + accumulatedBreak);
        Debug.Log("Start Point: " + startPoint);
        Debug.Log("End Point: " + endPoint);

        Vector3 idealDirection = (endPoint - startPoint).normalized;
        
        // Adjusting for break
        // This can be adjusted further based on how strong you want to react to the calculated break
        idealDirection.x += accumulatedBreak;

        // Strength of putt can be influenced by the slope
        // (For simplicity, considering uphill requires stronger putt and downhill requires softer putt)
        float strengthMultiplier = 1 + accumulatedBreak;

        Debug.Log("idealDirection: " + (idealDirection * strengthMultiplier));

        return idealDirection * strengthMultiplier;
    }
    
    public Vector3 CalculateAverageSlopeDirection()
{
    // Ensure we have sampled positions
    if (sampledHeights.Count < 2)
    {
        Debug.LogWarning("Not enough data to compute average slope direction!");
        return Vector3.zero;
    }

    Vector3 totalDirection = Vector3.zero;
    Vector3 previousPosition = startPoint;

    for (int i = 0; i < sampledHeights.Count; i++)
    {
        Vector3 currentPosition = startPoint + (endPoint - startPoint).normalized * sampleRate * i;
        currentPosition.y = sampledHeights[i];

        Vector3 directionBetweenSamples = currentPosition - previousPosition;
        totalDirection += directionBetweenSamples;

        previousPosition = currentPosition;
    }

    return totalDirection.normalized;
}

// Idea: For the deltaY we really only care about the starting position and ending psoition. 
// For the deltaX we actually do want the values in between to sum the curve
}
