using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
}
