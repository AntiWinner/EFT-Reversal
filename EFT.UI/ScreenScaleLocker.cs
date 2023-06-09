using UnityEngine;

namespace EFT.UI;

public class ScreenScaleLocker : MonoBehaviour
{
	[SerializeField]
	private Vector3 _scaleLocked = Vector3.one;

	private void Update()
	{
		if (base.transform.lossyScale != _scaleLocked)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x / base.transform.lossyScale.x * _scaleLocked.x, base.transform.localScale.y / base.transform.lossyScale.y * _scaleLocked.y, base.transform.localScale.y / base.transform.lossyScale.y * _scaleLocked.z);
		}
	}
}
