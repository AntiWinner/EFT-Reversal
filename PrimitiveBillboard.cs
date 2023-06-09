using UnityEngine;

public class PrimitiveBillboard : MonoBehaviour
{
	private void Update()
	{
		if (_E8A8.Instance.Camera != null)
		{
			base.transform.forward = -_E8A8.Instance.Camera.transform.forward;
		}
	}
}
