using UnityEngine;

namespace Koenigz.PerfectCulling;

public abstract class SamplingProviderBase : MonoBehaviour, _E4A5
{
	private _E49C _E000;

	public abstract string Name { get; }

	protected virtual void OnEnable()
	{
		if (_E000 == null)
		{
			_E000 = GetComponent<_E49C>();
		}
		_E000.AddSamplingProvider(this);
	}

	protected virtual void OnDisable()
	{
		if (_E000 == null)
		{
			_E000 = GetComponent<_E49C>();
		}
		_E000.RemoveSamplingProvider(this);
	}

	public abstract void InitializeSamplingProvider();

	public abstract bool IsSamplingPositionActive(PerfectCullingBakingBehaviour bakingBehaviour, Vector3 pos);
}
