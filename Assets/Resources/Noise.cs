using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{

	List<Vector2> points;
	public float First {
		get {
			return points[0].x;
		}
	}
	public float Last {
		get {
			return points[points.Count - 1].x;
		}
	}

	public Noise(List<Vector2> points) {
		this.points = points;
		this.points.Sort((s,t) => s.x.CompareTo(t.x));
	}

	// get y value from a given x position
	public float GetValue(float x) {
		Vector2 start = points[0];
		Vector2 end = points[0];
		for(int i = 0 ; i < points.Count; i++) {
			if (x == points[i].x) {
				return points[i].y;
			}
			else if (x < points[i].x) {
				end = points[i];
				break;
			}
			start = points[i];
		}

		float y_max = end.y - start.y;
		float y_diff = Mathf.Sin((x - start.x) * Mathf.PI / (end.x - start.x) - Mathf.PI / 2) * y_max / 2 + y_max / 2;

		return start.y + y_diff;
	}

	static public Noise CreatePerlin(float min_x, float max_x, float max_y, int freq) {
		List<Vector2> tmp = new List<Vector2>(freq + 1);
		for (int i = 0; i <= freq; i++){
			tmp.Add(new Vector2(min_x + (max_x - min_x) / freq * i, Random.Range(0f, max_y)));
		}
		return new Noise(tmp);
	}

	static public Noise Import(Transform p) {
		var tmp = new List<Vector2>(p.childCount);
		foreach (Transform pt in p) {
			tmp.Add(pt.position);
		}
		return new Noise(tmp);
	}

}
