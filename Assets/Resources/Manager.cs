using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject line;                             // In scene line. 
    public Color landColor,waterColor;

	public List<Transform> land = new List<Transform>();
	public List<Transform> water = new List<Transform>();

	public float lineWidth = 1;

    // Start is called before the first frame update
    void Start()
    {
        DrawLine(land, landColor);
        DrawLine(water, waterColor);
    }



    // Update is called once per frame
    void Update()
    {

    }

    void DrawLine(List<Transform> t, Color c)
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
    	
    }
}
