using UnityEngine;


public class GizmoShow : MonoBehaviour
{
	public Color SphereColor = Color.white, BoxColor = Color.white;

	public float SphereRadius = 0.0f;
	public Vector3 BoxSize = Vector3.zero;

	public Vector3 BoxOffset = Vector3.zero,
				   SphereOffset = Vector3.zero;

	public bool OnlyWhenSelected = true;


	void OnDrawGizmos()
	{
		if (!OnlyWhenSelected)
			Draw();
	}
	void OnDrawGizmosSelected()
	{
		if (OnlyWhenSelected)
			Draw();
	}

	private void Draw()
	{
		Vector3 pos = transform.position;

		Gizmos.color = SphereColor;
		Gizmos.DrawSphere(pos + SphereOffset, SphereRadius);

		Gizmos.color = BoxColor;
		Gizmos.DrawCube(pos + BoxOffset, BoxSize);
	}
}