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
    // Start is called before the first frame update
    void Start()
    {
        landVertices = DrawLine(land, landColor);
        waterVertices = DrawLine(water, waterColor);
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
                bulletVelocity.text = $"{initV}";
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (initV < 8)
            {
                initV++;
                bulletVelocity.text = $"{initV}";
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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
