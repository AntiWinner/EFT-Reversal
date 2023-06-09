using System.Runtime.CompilerServices;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public abstract class PerfectCullingCrossSceneContent : MonoBehaviour, _E4B1
{
	public const int INVALID_CONTENT_GROUP_ID = int.MinValue;

	[SerializeField]
	[HideInInspector]
	protected int _contentGroupId = int.MinValue;

	[SerializeField]
	[HideInInspector]
	protected int _contentHash;

	[SerializeField]
	protected PerfectCullingBakeGroup[] _bakeGroups;

	[SerializeField]
	protected Vector2Int _indexSpan;

	[CompilerGenerated]
	private PerfectCullingCrossSceneGroup _E000;

	public int ContentGroupId => _contentGroupId;

	public int ContentHash => _contentHash;

	public PerfectCullingBakeGroup[] BakeGroups => _bakeGroups;

	public Vector2Int IndexSpan => _indexSpan;

	public PerfectCullingCrossSceneGroup RuntimeCullingGroup
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	public virtual void OnBakedLODVisbilityChanged(ScreenDistanceSwitcher bakeLOD, bool isVisible)
	{
		if (!ScreenDistanceSwitcher._E007 || RuntimeCullingGroup == null)
		{
			return;
		}
		lock (RuntimeCullingGroup.lockUpdateVisibilityQueues)
		{
			if (isVisible)
			{
				RuntimeCullingGroup.AddHiddenIndex(_indexSpan);
			}
			else
			{
				RuntimeCullingGroup.RemoveHiddenIndex(_indexSpan);
			}
			if (bakeLOD.NumRenderersSwitch > 0 && !isVisible)
			{
				int numRenderersSwitch = bakeLOD.NumRenderersSwitch;
				for (int i = 0; i < numRenderersSwitch && i < _bakeGroups.Length; i++)
				{
					_bakeGroups[_bakeGroups.Length - 1 - i].IsEnabled = true;
				}
			}
		}
		PerfectCullingCrossSceneSampler.UpdateBakedLODVisibility(RuntimeCullingGroup, bakeLOD);
	}
}
