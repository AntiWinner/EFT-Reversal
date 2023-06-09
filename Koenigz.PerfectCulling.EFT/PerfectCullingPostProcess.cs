using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingPostProcess : MonoBehaviour
{
	[SerializeField]
	private bool _remove = true;

	public bool ShouldRemove
	{
		get
		{
			return _remove;
		}
		set
		{
			_remove = value;
		}
	}
}
