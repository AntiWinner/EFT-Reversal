using BezierSplineTools;
using UnityEngine;

namespace EFT.Interactive;

public class CarExtraction : ExfiltrationSubscriber
{
	public BezierSpline Spline;

	public WheelDrive Driver;

	public float _previousT;

	public float Step = 0.1f;

	private float _E000;

	private float _E001;

	private const float _E002 = 45f;

	private bool _E003 = true;

	public override void Dispose()
	{
		if (Driver != null)
		{
			Driver.AudioSource.transform.parent = null;
			Driver.AudioSource.gameObject.SetActive(value: false);
		}
		base.Dispose();
		Object.Destroy(base.gameObject);
		if (Driver != null)
		{
			Driver.enabled = false;
		}
	}

	public override void Play(bool force = false)
	{
		if (force)
		{
			Dispose();
			return;
		}
		_E003 = false;
		Driver.enabled = true;
		Driver.AudioSource.PlayOneShot(Driver.OnStartSound);
		_E001 = 45f;
	}

	private void Update()
	{
		if (!_E003)
		{
			_E000 = Spline.ClosestTimeOnBezier(Driver.transform.position, _E000, 0.02f, _E000 + 0.041f);
			float num = _E000 + Step;
			if (num < 1f)
			{
				Driver.Steer(Spline.GetPoint(num));
			}
			else
			{
				Dispose();
			}
			_E001 -= Time.deltaTime;
			if (_E001 <= 0f)
			{
				Dispose();
			}
		}
	}
}
