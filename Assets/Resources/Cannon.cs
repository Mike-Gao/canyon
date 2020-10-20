using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public Transform c;
    public float Angle
    {
        get { 
            return transform.rotation.eulerAngles.z; 
        }
    }
    public Vector3 Position 
    { 
        get { 
            return c.position;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAngle(float deg)
    {
        if (Angle + deg < 90 && Angle + deg > 0) transform.Rotate(0, 0, deg);
    }
}
