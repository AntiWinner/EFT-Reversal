using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public abstract class AmbianceObject<T> : SerializedMonoBehaviour, _E809
{
	[SerializeField]
	private bool _isActive = true;

	[SerializeField]
	private EAreaType _areaType = EAreaType.NotSet;

	[SerializeField]
	protected Dictionary<ELightStatus, Pattern<T>> Patterns;

	private Dictionary<ELightStatus, float> _E000 = new Dictionary<ELightStatus, float>();

	public EAreaType AreaType => _areaType;

	public void Init(AreaTemplate areaTemplate)
	{
		this._E000 = areaTemplate.PowerDelays;
	}

	protected virtual bool ImmediateInteraction(ELightStatus status)
	{
		if (!_isActive)
		{
			return false;
		}
		float probability = Patterns[status].Probability;
		if (probability <= 0f)
		{
			return false;
		}
		if (!(probability >= 100f))
		{
			return Random.value < probability / 100f;
		}
		return true;
	}

	public virtual async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!ImmediateInteraction(status))
		{
			return false;
		}
		this._E000.TryGetValue(status, out var value);
		await Task.Delay((int)((Patterns[status].Delay.Result + value) * 1000f));
		return this != null;
	}
}
