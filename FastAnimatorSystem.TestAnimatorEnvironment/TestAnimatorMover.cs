using System;
using System.Collections.Generic;
using EFT;
using UnityEngine;
using UnityEngine.UI;

namespace FastAnimatorSystem.TestAnimatorEnvironment;

public class TestAnimatorMover : MonoBehaviour
{
	public TestAnimatorSkeleton[] skeletons;

	public Slider aimAngleSlider;

	public Slider speedSlider;

	public Slider levelSlider;

	public Slider tiltSlider;

	public Slider deltaRotationSlider;

	public Slider firstActionSlider;

	public Text frameLabel;

	public bool showDeltaLog;

	public bool thirdPersonParam = true;

	public bool replayRecord;

	public bool applyDeltaPositionOnRecord;

	public string recordFilePath;

	public bool autoMove;

	public bool autoProne;

	public bool autoDeltaRotation;

	public bool autoJump;

	public int targetFrameRate;

	private readonly int m__E000 = Animator.StringToHash(_ED3E._E000(63706));

	private readonly int m__E001 = Animator.StringToHash(_ED3E._E000(63699));

	private readonly int m__E002 = Animator.StringToHash(_ED3E._E000(36788));

	private readonly int _E003 = Animator.StringToHash(_ED3E._E000(127290));

	private readonly int _E004 = Animator.StringToHash(_ED3E._E000(130727));

	private readonly int _E005 = Animator.StringToHash(_ED3E._E000(127276));

	private readonly int _E006 = Animator.StringToHash(_ED3E._E000(63684));

	private readonly int _E007 = Animator.StringToHash(_ED3E._E000(49297));

	private readonly int _E008 = Animator.StringToHash(_ED3E._E000(63682));

	private readonly int _E009 = Animator.StringToHash(_ED3E._E000(130783));

	private readonly int _E00A = Animator.StringToHash(_ED3E._E000(49572));

	private readonly int _E00B = Animator.StringToHash(_ED3E._E000(127273));

	private readonly int _E00C = Animator.StringToHash(_ED3E._E000(130774));

	private readonly int _E00D = Animator.StringToHash(_ED3E._E000(63736));

	private readonly int _E00E = Animator.StringToHash(_ED3E._E000(50050));

	private readonly int _E00F = Animator.StringToHash(_ED3E._E000(130762));

	private readonly int _E010 = Animator.StringToHash(_ED3E._E000(130814));

	private readonly int _E011 = Animator.StringToHash(_ED3E._E000(130807));

	private List<IAnimator> _E012 = new List<IAnimator>();

	private float _E013;

	private float _E014;

	private bool _E015;

	private bool _E016;

	private bool _E017;

	private bool _E018;

	private int _E019;

	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFrameRate;
		_E012.Clear();
		TestAnimatorSkeleton[] array = skeletons;
		foreach (TestAnimatorSkeleton testAnimatorSkeleton in array)
		{
			testAnimatorSkeleton.Init();
			_E012.Add(testAnimatorSkeleton.Animator);
		}
	}

	private void Update()
	{
		TestAnimatorSkeleton[] array;
		if (Input.GetKeyDown(KeyCode.R))
		{
			array = skeletons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.position = Vector3.zero;
			}
		}
		float num = (autoMove ? 1f : 0f);
		float num2 = (autoMove ? 1f : 0f);
		float value = tiltSlider.value;
		float value2 = levelSlider.value;
		float value3 = aimAngleSlider.value;
		if (Input.GetKey(KeyCode.W))
		{
			num2 = 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			num2 = -1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			num = 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			num = -1f;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			value = -5f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			value = 5f;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			value2 = 0f;
		}
		_E016 = Input.GetKey(KeyCode.LeftShift);
		if (Input.GetKeyDown(KeyCode.C))
		{
			_E015 = !_E015;
		}
		if (autoProne)
		{
			_E015 = true;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			_E017 = !_E017;
		}
		bool flag = Input.GetKey(KeyCode.Space) || autoJump;
		bool flag2 = Math.Abs(num) > Mathf.Epsilon || Math.Abs(num2) > Mathf.Epsilon || flag;
		EMovementDirection eMovementDirection = _E6D6.ConvertToMovementDirection(new Vector2(0f - num, num2));
		if (!flag2)
		{
			num = _E013;
			num2 = _E014;
		}
		foreach (IAnimator item in _E012)
		{
			if (!(item is _E502) || !autoMove || _E018)
			{
				item.SetFloat(this.m__E000, num);
				item.SetFloat(this.m__E001, num2);
				item.SetBool(this.m__E002, flag2);
				item.SetBool(_E003, flag);
				item.SetBool(_E004, !flag);
				item.SetFloat(_E008, speedSlider.value);
				item.SetInteger(_E00F, (int)firstActionSlider.value);
				item.SetFloat(_E005, value);
				item.SetFloat(_E006, value2);
				item.SetFloat(_E007, value3);
				item.SetBool(_E00E, _E016);
				item.SetFloat(_E00D, autoDeltaRotation ? (-87.42857f) : deltaRotationSlider.value);
				item.SetBool(_E00B, _E015);
				item.SetBool(_E00C, _E017);
				item.SetBool(_E00A, thirdPersonParam);
				item.SetBool(_ED3E._E000(130674), Input.GetKey(KeyCode.K));
				if (Input.GetKeyDown(KeyCode.L))
				{
					item.SetBool(_ED3E._E000(130671), value: true);
					item.SetBool(_ED3E._E000(130659), value: true);
				}
				item.SetBool(_E011, value: true);
				if (item is _E502)
				{
					item.SetFloat(_E010, 0.1032162f);
				}
				item.SetFloat(_E009, 2f);
				if (eMovementDirection != EMovementDirection.None && flag2)
				{
					float value4 = eMovementDirection.InvertDirection();
					item.SetFloat(_E009, value4);
				}
			}
		}
		_E018 = autoMove;
		array = skeletons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Process(Time.deltaTime);
		}
		_E000();
	}

	private void _E000()
	{
		if (!showDeltaLog)
		{
			return;
		}
		float num = Vector3.Distance(skeletons[0].transform.position, skeletons[1].transform.position);
		string text = _ED3E._E000(130710) + num + _ED3E._E000(18502);
		bool flag = false;
		TestAnimatorSkeleton[] array = skeletons;
		foreach (TestAnimatorSkeleton testAnimatorSkeleton in array)
		{
			AnimatorStateInfoWrapper currentState = testAnimatorSkeleton.GetCurrentState();
			if (testAnimatorSkeleton.Animator.IsInTransition(0))
			{
				flag = true;
			}
			float num2 = 0f;
			if (flag)
			{
				num2 = testAnimatorSkeleton.GetNextState().normalizedTime;
			}
			Vector3 deltaPosition = testAnimatorSkeleton.GetDeltaPosition();
			float deltaTime = Time.deltaTime;
			Quaternion deltaRotation = testAnimatorSkeleton.Animator.deltaRotation;
			new Quaternion(deltaRotation.x / deltaTime, deltaRotation.y / deltaTime, deltaRotation.z / deltaTime, deltaRotation.w / deltaTime);
			text = text + _ED3E._E000(130701) + currentState.normalizedTime + _ED3E._E000(130694) + num2 + _ED3E._E000(130747) + _E001(deltaPosition) + _ED3E._E000(130728);
		}
		Debug.Log(text);
	}

	private string _E001(Vector3 vec)
	{
		return _ED3E._E000(27312) + vec.x + _ED3E._E000(10270) + vec.y + _ED3E._E000(10270) + vec.z + _ED3E._E000(54241);
	}

	private string _E002(Quaternion vec)
	{
		return _ED3E._E000(27312) + vec.x + _ED3E._E000(10270) + vec.y + _ED3E._E000(10270) + vec.z + _ED3E._E000(10270) + vec.w + _ED3E._E000(54241);
	}
}
