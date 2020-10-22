using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
	public Vector3 Velocity = new Vector3();
	public Vector3 Acceleration{get;set;}
	public Vector3 Position
	{
		get {
			return gameObject.transform.position;
		}
		set {
			gameObject.transform.position = value;
		}
	}
	public Vector3 prevPosition = new Vector3();
	float stationaryDuration = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	prevPosition = Position;
    	Vector3 NewVelocity = Velocity + Acceleration * Time.deltaTime;
		Position += ((Velocity + NewVelocity) / 2 * Time.deltaTime);
		Velocity = NewVelocity;
		if (Vector3.Magnitude (Position - prevPosition) < 0.002)
		{
			stationaryDuration += Time.deltaTime;
		} else {
			stationaryDuration = 0;
		}
		if (Position.x > 10 || Position.x < -10 || Position.y > 15 || Position.y < -5 || stationaryDuration > 2.0)
		{
			Destroy(gameObject);
		}
        
    }

    public void SetVelocity(float v, float d)
    {
        this.Velocity.x = Mathf.Cos(Mathf.Deg2Rad * d) * v;
        this.Velocity.y = Mathf.Sin(Mathf.Deg2Rad * d) * v;
    }
}
