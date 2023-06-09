using System.Runtime.CompilerServices;
using UnityEngine;

namespace Audio.SpatialSystem;

public abstract class AudioFilter : MonoBehaviour
{
	[CompilerGenerated]
	private float _E000;

	public float FilterLevel
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		protected set
		{
			_E000 = value;
		}
	}

	public abstract void ResetFilter();

	public abstract void SetFilterParams(float value, bool applyImmediately = false, ESoundOcclusionType occlusionType = ESoundOcclusionType.FullOcclusion);

	protected abstract void Initialize();
}
