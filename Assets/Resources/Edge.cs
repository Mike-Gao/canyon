using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Edge
{
	public PhysicsBody vertex_1;
	public PhysicsBody vertex_2;
	public float length;
	public LineRenderer line;
	public void UpdatePosition()
	{
		line.SetPosition(0, vertex_1.Position);
		line.SetPosition(1, vertex_2.Position);
	}
	public bool Collides(PhysicsBody b)
	{
		return (HandleUtility.DistancePointToLineSegment(b.Position, vertex_1.Position, vertex_2.Position) < 0.86f);
	}
	public void AddForce(Vector3 v)
	{
		vertex_1.Position += v * Time.deltaTime * 5f;
		vertex_2.Position += v * Time.deltaTime * 5f;
	}
    
}
