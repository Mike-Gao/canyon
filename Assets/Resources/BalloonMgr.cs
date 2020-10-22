using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMgr : MonoBehaviour
{
	public Transform left;
	public Transform right;
	public GameObject Balloon;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBalloon",0,1.0f);
    }

    void SpawnBalloon()
    {
    	Instantiate(Balloon, new Vector3(Random.Range(left.position.x + 0.5f, right.position.x - 0.5f), left.position.y + 1, 0), Quaternion.identity);
    }
}
