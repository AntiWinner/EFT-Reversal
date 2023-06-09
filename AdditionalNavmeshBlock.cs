using UnityEngine;

public class AdditionalNavmeshBlock : MonoBehaviour
{
	public bool Reverse;

	public void EnableAll()
	{
		if (Reverse)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void DisableAll()
	{
		if (Reverse)
		{
			base.gameObject.SetActive(value: true);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
