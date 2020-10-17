using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject line;                             // In scene line. 
    public Color landColor,waterColor;

	public List<Transform> land = new List<Transform>();
	public List<Transform> water = new List<Transform>();

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
    	var baseLayer = NoiseLayer();
    	for(int i = 0; i < t.Count; ++i){
    		NoiseLayer
    	}
    	
    }
}
