using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public Vector2 border;
	public Transform target;
	Vector2 size;


	void Start()
	{
		Camera cam = GetComponent<Camera>();
		size.y = cam.orthographicSize;
		size.x = cam.aspect * size.y;

	}
	// Update is called once per frame
	void LateUpdate()
	{
		Vector2 distance = target.position - transform.position;
		Vector3 v = transform.position;
		Vector2 s = size - border;
		if (Mathf.Abs(distance.x) > s.x)
		{

			v.x += (Mathf.Abs(distance.x) - s.x) * Mathf.Sign(distance.x);

		}
		if (Mathf.Abs(distance.y) > s.y)
		{
			v.y += (Mathf.Abs(distance.y) - s.y) * Mathf.Sign(distance.y);

		}
		transform.position = v;
	}
}
