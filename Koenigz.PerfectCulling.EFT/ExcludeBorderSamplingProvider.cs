using System;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(PerfectCullingBakingBehaviour))]
public sealed class ExcludeBorderSamplingProvider : SamplingProviderBase
{
	[SerializeField]
	private LevelBorder _border;

	[NonSerialized]
	private _E3D0 _E003;

	public override string Name => _ED3E._E000(67008);

	public override void InitializeSamplingProvider()
	{
		if (_border == null)
		{
			throw new NullReferenceException(_ED3E._E000(67071));
		}
		_E003 = _border.CreateConcaveBorder();
	}

	public override bool IsSamplingPositionActive(PerfectCullingBakingBehaviour bakingBehaviour, Vector3 pos)
	{
		return _E003.Contains(pos.x, pos.z);
	}
}
