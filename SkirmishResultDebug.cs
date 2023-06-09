using UnityEngine;

public class SkirmishResultDebug : MonoBehaviour
{
	public Vector3 From;

	public Vector3 To;

	public string Aggressor;

	public string Victim;

	public void Init(Vector3 from, Vector3 to, string aggressor, string victim)
	{
		Aggressor = aggressor;
		Victim = victim;
		From = from;
		To = to;
		base.transform.position = (from + to) * 0.5f;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(From, 0.2f);
		Gizmos.DrawWireSphere(From, 0.2f);
		Gizmos.DrawLine(From, To);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(To, 0.2f);
		Gizmos.DrawWireSphere(To, 0.2f);
	}
}
