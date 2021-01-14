using UnityEngine;
using System.Collections;

public class SphereGizmo : MonoBehaviour
{
	public Color color;
	public float radius;

	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
