using UnityEngine;

namespace EFT.Hideout;

public class HideoutAreaDefaultLevel : MonoBehaviour
{
	public void Enable()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Disable()
	{
		base.gameObject.SetActive(value: false);
	}
}
