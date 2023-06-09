using UnityEngine;

namespace EFT.Visual;

[ExecuteInEditMode]
public class Flicker : MonoBehaviour, _E3A7
{
	protected enum ECurveType
	{
		SelectTypeForGenerate,
		Random,
		Sin,
		Triangle,
		Saw,
		Square
	}

	[SerializeField]
	protected float Frequency = 1f;

	[SerializeField]
	protected float Intensity = 1f;

	[SerializeField]
	protected float IntensityShift;

	[SerializeField]
	protected float TimeShift;

	[SerializeField]
	protected AnimationCurve Curve;

	[SerializeField]
	protected ECurveType Generate;

	[SerializeField]
	protected bool RandomTimeShift;

	[SerializeField]
	protected bool FullRandomCurve;

	public float CullingCoef = 1f;

	protected float RandomSeed;

	protected virtual void Awake()
	{
		if (RandomTimeShift)
		{
			TimeShift = Random.value * 300f;
		}
		RandomSeed = Random.value * 10f;
	}

	protected virtual void OnEnable()
	{
		_E3A3.RegisterInSystem(this);
	}

	protected virtual void OnDisable()
	{
		_E3A3.UnregisterInSystem(this);
	}

	protected virtual void OnDestroy()
	{
	}

	public void SetCurve(AnimationCurve curve)
	{
		Curve = curve;
	}

	public virtual void ManualUpdate()
	{
	}
}
