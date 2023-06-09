using UnityEngine;

public class UnspawnPoint : MonoBehaviour
{
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0.7f, 0.2f, 0.2f, 0.4f);
		_E395.DrawCube(base.transform.position + Vector3.up * 60f * 0.5f, base.transform.rotation, new Vector3(1f, 60f, 1f));
	}
}
