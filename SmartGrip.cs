using RootMotion.FinalIK;
using UnityEngine;

public class SmartGrip : GripPose
{
	public Quaternion _initial;

	public Transform Targets;

	public Transform Pivot;

	public bool IkEnabled;

	private LimbIK[] _E000;

	public Transform DEBUG_TARGET;

	public float Weight = 1f;

	public override void CacheAndDestroy()
	{
	}

	public override void Awake()
	{
		if (Pivot == null)
		{
			Debug.LogFormat(Pivot, _ED3E._E000(46406), base.name);
			return;
		}
		base.transform.parent = Pivot;
		_initial = Pivot.localRotation;
		_E000 = GetComponentsInChildren<LimbIK>();
		if (_E000.Length < 5)
		{
			Debug.LogFormat(this, _ED3E._E000(46482), base.name, _E000.Length);
		}
		if (Targets.childCount < 5)
		{
			Debug.LogFormat(this, _ED3E._E000(46512), base.name, Targets.childCount);
		}
		for (int i = 0; i < Targets.childCount; i++)
		{
			_E000[i].solver.target = Targets.GetChild(i);
		}
		Reset();
	}

	public void LookAt(Vector3 target, bool debug = false)
	{
		if (!IkEnabled)
		{
			EnableIK();
		}
		Pivot.LookAt(target);
		Pivot.localRotation = Quaternion.Lerp(_initial, Pivot.localRotation, Weight);
	}

	public void EnableIK()
	{
		LimbIK[] array = _E000;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
	}

	public void Reset()
	{
		Pivot.localRotation = _initial;
		LimbIK[] array = _E000;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		IkEnabled = false;
	}
}
