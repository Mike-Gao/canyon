using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
	public Vector3[] land;
    public Vector3[] water;
    public float Restitution = 0.8f;
    // Start is called before the first frame update
    private readonly float bulletRadius = 0.086f;
    bool bounced = false;
    float stationaryDuration = 0;

    void Start()
    {
    	var mgr = FindObjectOfType<Manager>();
    	land = mgr.landVertices;
    	water = mgr.waterVertices;
    }

    // Update is called once per frame
    void Update()
    {
    	bounced = false;
    	for (int i = 0 ; i < land.Length; i++)
    	{
    		if (Vector3.Distance(land[i], transform.position) < bulletRadius)
    		{
    			Bounce(i);
    			return;
    		}
    	}

    	for (int i = 0; i < water.Length; i++)
    	{
            // If it hits the water
    		if (Vector3.Distance(water[i], transform.position) < bulletRadius)
    		{
    			Destroy(gameObject);
    			return;
    		}
    	}
        
    }

    void Bounce(int idx)
    {
    	var physicsBody = gameObject.GetComponent<PhysicsBody>();
    	if (!bounced)
    	{
    		stationaryDuration = 0;
    	} 
    	else 
    	{
    		stationaryDuration += Time.deltaTime;
    		if (stationaryDuration > 2.0)
    		{
    			// destroy stucked object
    			Destroy(gameObject);
    			return;
    		}
    	}
    	bounced = true;
    	Vector3[] nearby = new Vector3[7];
    	for (int i = 0; i < 7; i++)
    	{
    		nearby[i] = land[idx - 3 + i];
    	}
    	Vector3 norm = Vector2.Perpendicular(Regression(nearby));
    	if (Mathf.Abs(Vector2.SignedAngle(physicsBody.Velocity, norm)) < 90)
    	{
    		// ball going away from the line
    		return;
    	}
    	var u = Vector3.Dot(physicsBody.Velocity, norm) / Vector3.Dot(norm, norm) * norm;
    	var v = physicsBody.Velocity - u;

    	physicsBody.Velocity = (v - u) * Restitution;

    }

    // Using Linear Regression to find the vector of the terrain (used to calculate bounce)
    Vector3 Regression(Vector3[] vertex)
    {
    	var mean = new Vector3();
    	var vec = new Vector3();
    	for (int i = 0; i < vertex.Length; i++)
    	{
    		mean += vertex[i];
    	}
    	mean /= vertex.Length;
    	for (int i = 0; i < vertex.Length; i++)
    	{
    		var tmp = vertex[i] - mean;
    		vec.x += tmp.x * tmp.x;
    		vec.y += tmp.y * tmp.x;
    	}
    	return vec.normalized;
    }
}

