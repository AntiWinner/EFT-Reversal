using UnityEngine;

public class ThowGrenadePlaceInner : MonoBehaviour
{
	public ThrowGrenadePlace ThrowGrenadePlace;

	public void OnDrawGizmosSelected()
	{
		if (ThrowGrenadePlace != null)
		{
			ThrowGrenadePlace.DrawGizmos();
		}
	}
}
