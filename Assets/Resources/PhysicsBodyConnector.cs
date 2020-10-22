using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBodyConnector : MonoBehaviour
{
	public List<PhysicsBody> vertex_1;
	public List<PhysicsBody> vertex_2;
	public List<PhysicsBody> balloon_tail;
	public float tail_length;
	public List<float> length;
	public GameObject line;

	private readonly List<Edge> body = new List<Edge>();
	private readonly List<Edge> tail = new List<Edge>();

    // Start is called before the first frame update
    void Start()
    {
    	for(int i = 0; i < vertex_1.Count; i++)
    	{
    		body.Add(ConnectPhysicsBody(vertex_1[i],vertex_2[i],length[i]));
    	}
        for(int i = 0; i < balloon_tail.Count; i++)
        {
        	tail.Add(ConnectPhysicsBody(balloon_tail[i - 1], balloon_tail[i], tail_length));
        }
    }

    // Update is called once per frame
    void Update()
    {
    	for(int i = 0; i < body.Count; i++)
    	{
    		Vector3 v1_to_v2 = body[i].vertex_2.Position - body[i].vertex_1.Position;
    		float l = v1_to_v2.magnitude;
    		float diff = l - body[i].length;
    		v1_to_v2.Normalize();
    		body[i].vertex_1.Position += v1_to_v2 * diff * 0.5f;
    		body[i].vertex_2.Position -= v1_to_v2 * diff * 0.5f;
    		body[i].UpdatePosition();
    	}

    	for(int i = 0; i < tail.Count; i++)
    	{
    		Vector3 v1_to_v2 = tail[i].vertex_2.Position - tail[i].vertex_1.Position;
    		float l = v1_to_v2.magnitude;
    		float diff = l - body[i].length;
    		v1_to_v2.Normalize();
    		tail[i].vertex_1.Position += v1_to_v2 * diff * 0.5f;
    		tail[i].vertex_2.Position -= v1_to_v2 * diff * 0.5f;
    		tail[i].UpdatePosition();
    	}

    	Collision[] bullets = FindObjectsOfType<Collision>();
    	for (int i = 0; i < bullets.Length; i++)
    	{
    		PhysicsBody pb = bullets[i].gameObject.GetComponent<PhysicsBody>();
    		for(int j = 0; j < tail.Count; j++) {
    			if (tail[j].Collides(pb))
    			{
    				tail[j].AddForce(pb.Velocity);
    			}
    		}

    		for(int j = 0; j < body.Count; j++)
    		{
    			if (body[j].Collides(pb))
    			{
    				print("Balloon Destroyed: body hit");
    				Destroy(gameObject);
    			}
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

    	GameObject ins = Instantiate(line);
        LineRenderer lineRenderer = ins.GetComponent<LineRenderer>();
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
