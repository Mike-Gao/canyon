using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBodyConnector : MonoBehaviour
{
	public List<PhysicsBody> vertex_1;
	public List<PhysicsBody> vertex_2;
	public List<float> length;
	public GameObject line;

	private readonly List<Edge> edges = new List<Edge>();

    // Start is called before the first frame update
    void Start()
    {
    	for(int i = 0; i < vertex_1.Count; i++)
    	{
    		edges.Add(ConnectPhysicsBody(vertex_1[i],vertex_2[i],length[i]));
    	}
        
    }

    // Update is called once per frame
    void Update()
    {
    	for(int i = 0; i < edges.Count; i++)
    	{
    		Vector3 v1_to_v2 = edges[i].vertex_2.Position - edges[i].vertex_1.Position;
    		float l = v1_to_v2.magnitude;
    		float diff = l - edges[i].length;
    		v1_to_v2.Normalize();
    		edges[i].vertex_1.Position += v1_to_v2 * diff * 0.5f;
    		edges[i].vertex_2.Position -= v1_to_v2 * diff * 0.5f;
    		edges[i].UpdatePosition();
    	}

    	Collision[] bullets = FindObjectsOfType<Collision>();
    	for (int i = 0; i < bullets.Length; i++)
    	{
    		PhysicsBody pb = bullets[i].gameObject.GetComponent<PhysicsBody>();
    		if(Vector3.Magnitude(transform.position - pb.Position) > 2)
    		{
    			// do not compute when its far away
    			break;
    		}
    	}
    }  

    Edge ConnectPhysicsBody(PhysicsBody vertex_1, PhysicsBody vertex_2, float l)
    {
    	Edge e = new Edge
    	{
    		vertex_1 = vertex_1,
    		vertex_2 = vertex_2,
    		length = l,
    	};

    	var ret = Instantiate(line);
        var lineRenderer = ret.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] {vertex_1.Position, vertex_2.Position});
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        e.line = lineRenderer;
        return e;
    }
}
