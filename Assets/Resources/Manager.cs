using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject line;                             // In scene line. 
    public Color landColor;
    public Color waterColor;
    public Text bulletVelocity;
    public Text windDirection;
    public GameObject bullet;
    public Transform land;
    public Transform water;
    public Cannon left;
    public Cannon right;

    public Cannon selected
    {
        get { return isLeft ? left : right; }
    }
	public float lineWidth = 1;
    public float deltaDeg = 60;

    public Vector3[] waterVertices;
    public Vector3[] landVertices;
    public bool isLeft = true;
    public int initV = 1;
    private float curV { get { return 2 + initV; } }

    public float maxY = -5;
    public float windDir = 0;
    // Start is called before the first frame update
    void Start()
    {
        landVertices = DrawLine(land, landColor);
        waterVertices = DrawLine(water, waterColor);
        for(int i = 0; i < landVertices.Length; i++){
            if (landVertices[i].y > maxY)
            {
                maxY = landVertices[i].y;
            }
        }
        InvokeRepeating("updateWind",0,1.0f);
    }

    void updateWind()
    {
        windDir = UnityEngine.Random.Range(-0.3f,0.3f);
        if(windDir < 0){
            windDirection.text = $"Wind Direction: left @ {Mathf.Abs(windDir)}";
        } else {
            windDirection.text = $"Wind Direction: right @ {Mathf.Abs(windDir)}";
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isLeft = !isLeft;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            selected.SetAngle(deltaDeg * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            selected.SetAngle(-deltaDeg * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (initV > 1)
            {
                initV--;
                bulletVelocity.text = $"Muzzle Velocity: {initV}";
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (initV < 8)
            {
                initV++;
                bulletVelocity.text = $"Muzzle Velocity: {initV}";
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        PhysicsBodyConnector[] balloons_above_the_mountain = FindObjectsOfType<PhysicsBodyConnector>();
        for (int i = 0; i < balloons_above_the_mountain.Length; i++)
        {
            List<Edge> mountain_edges = balloons_above_the_mountain[i].body;
            for (int j = 0; j < mountain_edges.Count; j++){
                if(mountain_edges[j].vertex_1.Position.y > maxY)
                {
                    mountain_edges[j].AddForce(new Vector3(windDir,0f,0f));
                }
            }
        }
        

    }


    void Shoot()
    {
        var obj = Instantiate(bullet).GetComponent<PhysicsBody>();
        obj.SetVelocity(curV, selected.Angle);
        if (!isLeft)
        {
            obj.Velocity.x = -obj.Velocity.x;
        }
        obj.Position = selected.Position;
        // add gravity
        obj.Acceleration = new Vector3(0, -4, 0);
    }

    Vector3[] DrawLine(Transform t, Color c)
    {
    	var baseLayer = Noise.Import(t);
    	var layers = new List<Noise> {baseLayer};
    	int freq = (int) (baseLayer.Last - baseLayer.First);
    	float amp = 0.4f;
    	for(int i = 0; i < 6; ++i){
    		layers.Add(Noise.CreatePerlin(baseLayer.First, baseLayer.Last, amp, freq));
    		freq*=2;
    		amp/=2;
    	}
    	var pointsAmnt = Mathf.FloorToInt((baseLayer.Last - baseLayer.First) / 0.05f) + 1;
    	var points = new Vector3[pointsAmnt];
    	for (int i = 0; i < pointsAmnt; i++) {
    		float x = baseLayer.First + 0.05f * i;
    		float y = 0f;
    		foreach (var layer in layers) {
    			y+= layer.GetValue(x);
    		}
    		points[i] = new Vector3(x,y,0);
    	}

    	var ret = Instantiate(line);
        var lineRenderer = ret.GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsAmnt;
        lineRenderer.SetPositions(points);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    	return points;
    }
}
