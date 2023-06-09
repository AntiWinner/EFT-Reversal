using UnityEngine;

public class ShuttleMoveDebugObject : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(base.transform.position, 0.1f);
		Gizmos.DrawWireSphere(base.transform.position, 0.5f);
		Gizmos.DrawWireSphere(base.transform.position, 0.6f);
	}
}
