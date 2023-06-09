using EFT;
using UnityEngine;

public class LookDirections : MonoBehaviour
{
	public bool UseLookSide;

	public Vector3 dir = Vector3.zero;

	public Vector3 Position => base.transform.position;

	public bool HaveLookSide => UseLookSide;

	public int LookIndexes
	{
		get
		{
			if (!UseLookSide)
			{
				return 0;
			}
			return 1;
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (UseLookSide)
		{
			Gizmos.DrawRay(base.transform.position, dir);
		}
	}

	public int GetOwnerId()
	{
		return -1;
	}

	public void SetOwner(BotOwner owner)
	{
	}

	public void NormalizeLookSide()
	{
		dir = _E39C.NormalizeFastSelf(dir);
	}
}
