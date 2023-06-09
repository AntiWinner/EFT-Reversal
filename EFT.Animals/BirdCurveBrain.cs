using BezierSplineTools;
using UnityEngine;

namespace EFT.Animals;

[RequireComponent(typeof(Bird))]
public class BirdCurveBrain : UpdateInEditorSystemComponent<BirdCurveBrain>
{
	[SerializeField]
	protected BezierSpline _spline;

	[SerializeField]
	protected float _baseSpeed = 6f;

	[SerializeField]
	protected float _gForceMult = 0.1f;

	protected Vector3 _gForce = new Vector3(0f, -9.8f, 0f);

	protected Bird _bird;

	protected float _splineLength;

	protected float _currentSpeed;

	protected float _currentDuration;

	protected float _currentSplineTime;

	protected void Awake()
	{
		if (_spline != null)
		{
			Init(_spline);
		}
	}

	protected void RecalculateSpeed(Vector3 direction)
	{
		Vector3 vector = direction * _baseSpeed;
		_currentSpeed = (vector + _gForce * _gForceMult).magnitude;
		_currentDuration = _splineLength / _currentSpeed;
	}

	public override void ManualUpdate(float deltaTime)
	{
		_currentSplineTime += deltaTime / _currentDuration;
		if (_currentSplineTime > 1f)
		{
			_currentSplineTime = 1f;
		}
		Vector3 pointWithCurvesLengthCache = _spline.GetPointWithCurvesLengthCache(_currentSplineTime);
		Vector3 directionWithCurvesLengthCache = _spline.GetDirectionWithCurvesLengthCache(_currentSplineTime);
		_bird.transform.position = pointWithCurvesLengthCache;
		_bird.SetDirection(directionWithCurvesLengthCache, _currentSpeed);
		if (_currentSplineTime >= 1f)
		{
			_currentSplineTime = 0f;
		}
		RecalculateSpeed(directionWithCurvesLengthCache);
	}

	public void Init(BezierSpline spline)
	{
		_spline = spline;
		_bird = GetComponent<Bird>();
		_spline.CreateCurvesLengthCache(0.1f);
		_splineLength = _spline.GetLengthFromChache();
		RecalculateSpeed(Vector3.forward);
	}
}
