using System;
using System.Collections.Generic;
using UnityEngine;

namespace CW2.Animations;

public class PhysicsSimulator : MonoBehaviour
{
	[Serializable]
	public class RandomDevice
	{
		public float Frequency = 1f;

		public float Intensity = 1f;

		public float IntensityShift;

		public float TimeShift;

		public bool RandomTimeShift;

		public bool SmoothStart;

		public Val ValueConfiguration;

		private float _rndSeed;

		private float _startT;

		public void Initialize()
		{
			if (RandomTimeShift)
			{
				TimeShift = UnityEngine.Random.value * 300f;
			}
			_rndSeed = UnityEngine.Random.value * 10f;
			ValueConfiguration.Initialize();
		}

		public void Process(PhysicsSimulator physicsSimulator)
		{
			float num = (Mathf.PerlinNoise(TimeShift + Time.time * Frequency, _rndSeed) - 0.5f) * Intensity;
			if (SmoothStart)
			{
				if (_startT < 0f)
				{
					_startT = Time.time + 3f;
				}
				float num2 = (Time.time - _startT) * (1f / 3f);
				if (num2 > 3f)
				{
					SmoothStart = false;
					_startT = -1f;
				}
				else
				{
					num *= num2;
				}
			}
			ValueConfiguration.Process(physicsSimulator, num + IntensityShift);
		}
	}

	[Serializable]
	public class CurveDevice
	{
		public AnimationCurve Curve;

		public float Frequency = 1f;

		public float Intensity = 1f;

		public float TimeShift;

		public float IntensityShift;

		public Val ValueConfiguration;

		public void Initialize()
		{
			ValueConfiguration.Initialize();
		}

		public void Process(PhysicsSimulator physicsSimulator)
		{
			float time = TimeShift + Time.time * Frequency;
			float val = IntensityShift + Curve.Evaluate(time) * Intensity;
			ValueConfiguration.Process(physicsSimulator, val);
		}
	}

	[Serializable]
	public class InputDevice
	{
		public string AxisName;

		public float Intensity = 1f;

		public float IntensityShift;

		public Val ValueConfiguration;

		public void Initialize()
		{
			ValueConfiguration.Initialize();
		}

		public void Process(PhysicsSimulator physicsSimulator)
		{
			float val = IntensityShift + Input.GetAxis(AxisName) * Intensity;
			ValueConfiguration.Process(physicsSimulator, val);
		}
	}

	[Serializable]
	public class ScriptValueDevice
	{
		public string ValueName;

		public float Intensity = 1f;

		public float IntensityShift;

		public Val ValueConfiguration;

		public void Initialize()
		{
			ValueConfiguration.Initialize();
		}

		public void Add(PhysicsSimulator physicsSimulator, float value)
		{
			value = IntensityShift + value * Intensity;
			ValueConfiguration.Process(physicsSimulator, value);
		}
	}

	[Serializable]
	public class UnityValueDevice
	{
		public enum Values
		{
			MousePosX,
			MousePosY
		}

		public Values ValueType;

		public float Intensity = 1f;

		public float IntensityShift;

		public Val ValueConfiguration;

		private static Func<float>[] _values;

		public void Initialize()
		{
			ValueConfiguration.Initialize();
		}

		static UnityValueDevice()
		{
			_values = new Func<float>[2];
			_values[0] = () => Mathf.Clamp01(Input.mousePosition.x / (float)Screen.width) * 2f - 1f;
			_values[1] = () => Mathf.Clamp01(Input.mousePosition.y / (float)Screen.height) * 2f - 1f;
		}

		public void Process(PhysicsSimulator physicsSimulator)
		{
			float val = IntensityShift + _values[(int)ValueType]() * Intensity;
			ValueConfiguration.Process(physicsSimulator, val);
		}
	}

	[Serializable]
	public class Spring
	{
		[HideInInspector]
		public Vector3 Zero;

		public float ReturnSpeed = 1f;

		public float Damping = 0.1f;

		public Vector3 Min = new Vector3(-180f, -180f, -180f);

		public Vector3 Max = new Vector3(180f, 180f, 180f);

		public float VelocityMax = 10f;

		public void FixedUpdate(ref Vector3 velocity, ref Vector3 current)
		{
			velocity += (Zero - current) * ReturnSpeed;
			velocity *= Damping;
			velocity = _E000(velocity, VelocityMax);
			current += velocity;
			current = Vector3.Min(Vector3.Max(current, Min), Max);
		}

		private static Vector3 _E000(Vector3 vec, float limit)
		{
			if (vec.x > limit)
			{
				vec.x = limit;
			}
			if (vec.y > limit)
			{
				vec.y = limit;
			}
			if (vec.z > limit)
			{
				vec.z = limit;
			}
			if (vec.x < 0f - limit)
			{
				vec.x = 0f - limit;
			}
			if (vec.y < 0f - limit)
			{
				vec.y = 0f - limit;
			}
			if (vec.z < 0f - limit)
			{
				vec.z = 0f - limit;
			}
			return vec;
		}
	}

	[Serializable]
	public class Val
	{
		public enum TargetType
		{
			Position,
			PositionVelocity,
			Rotation,
			RotationVelocity
		}

		public enum ComponentType
		{
			X,
			Y,
			Z
		}

		public enum OperationType
		{
			Set,
			Add,
			Mult,
			Clamp,
			Min,
			Max
		}

		public TargetType Target;

		public OperationType Operation;

		public ComponentType Component;

		private Func<float, float, float> _operation;

		private Func<PhysicsSimulator, Vector3> _targetGet;

		private Action<PhysicsSimulator, Vector3> _targetSet;

		private int _component;

		private static Func<float, float, float>[] _operations;

		private static Func<PhysicsSimulator, Vector3>[] _targetGets;

		private static Action<PhysicsSimulator, Vector3>[] _targetSets;

		static Val()
		{
			_operations = new Func<float, float, float>[6];
			_operations[0] = (float f, float f1) => f1;
			_operations[1] = (float f, float f1) => f + f1;
			_operations[2] = (float f, float f1) => f * f1;
			_operations[3] = (float f, float f1) => Mathf.Clamp(f, 0f - f1, f1);
			_operations[4] = Mathf.Min;
			_operations[5] = Mathf.Max;
			_targetGets = new Func<PhysicsSimulator, Vector3>[4];
			_targetGets[0] = (PhysicsSimulator s) => s.Position;
			_targetGets[1] = (PhysicsSimulator s) => s.PositionVelocity;
			_targetGets[2] = (PhysicsSimulator s) => s.Rotation;
			_targetGets[3] = (PhysicsSimulator s) => s.RotationVelocity;
			_targetSets = new Action<PhysicsSimulator, Vector3>[4];
			_targetSets[0] = delegate(PhysicsSimulator s, Vector3 v)
			{
				s.Position = v;
			};
			_targetSets[1] = delegate(PhysicsSimulator s, Vector3 v)
			{
				s.PositionVelocity = v;
			};
			_targetSets[2] = delegate(PhysicsSimulator s, Vector3 v)
			{
				s.Rotation = v;
			};
			_targetSets[3] = delegate(PhysicsSimulator s, Vector3 v)
			{
				s.RotationVelocity = v;
			};
		}

		public void Initialize()
		{
			_component = (int)Component;
			_operation = _operations[(int)Operation];
			_targetGet = _targetGets[(int)Target];
			_targetSet = _targetSets[(int)Target];
		}

		public void Process(PhysicsSimulator physicsSimulator, float val)
		{
			Vector3 arg = _targetGet(physicsSimulator);
			arg[_component] = _operation(arg[_component], val);
			_targetSet(physicsSimulator, arg);
		}
	}

	public Vector3 Pivot;

	public Spring PositionSpring;

	public Spring RotationSpring;

	public RandomDevice[] RandomDevices;

	public CurveDevice[] CurveDevices;

	public InputDevice[] InputDevices;

	public ScriptValueDevice[] ScriptValueDevices;

	public UnityValueDevice[] UnityValueDevices;

	[HideInInspector]
	public Vector3 Position;

	[HideInInspector]
	public Vector3 PositionVelocity;

	[HideInInspector]
	public Vector3 Rotation;

	[HideInInspector]
	public Vector3 RotationVelocity;

	private Dictionary<string, ScriptValueDevice> _E000 = new Dictionary<string, ScriptValueDevice>();

	[ContextMenu("Awake")]
	private void Awake()
	{
		RandomDevice[] randomDevices = RandomDevices;
		for (int i = 0; i < randomDevices.Length; i++)
		{
			randomDevices[i].Initialize();
		}
		CurveDevice[] curveDevices = CurveDevices;
		for (int i = 0; i < curveDevices.Length; i++)
		{
			curveDevices[i].Initialize();
		}
		InputDevice[] inputDevices = InputDevices;
		for (int i = 0; i < inputDevices.Length; i++)
		{
			inputDevices[i].Initialize();
		}
		foreach (KeyValuePair<string, ScriptValueDevice> item in _E000)
		{
			item.Value.Initialize();
		}
		UnityValueDevice[] unityValueDevices = UnityValueDevices;
		for (int i = 0; i < unityValueDevices.Length; i++)
		{
			unityValueDevices[i].Initialize();
		}
		ScriptValueDevice[] scriptValueDevices = ScriptValueDevices;
		foreach (ScriptValueDevice scriptValueDevice in scriptValueDevices)
		{
			_E000.Add(scriptValueDevice.ValueName, scriptValueDevice);
		}
		Position = (PositionSpring.Zero = base.transform.localPosition - Pivot);
		Rotation = (RotationSpring.Zero = base.transform.localEulerAngles);
		PositionVelocity = Vector3.zero;
		RotationVelocity = Vector3.zero;
	}

	public void Add(string name, float value)
	{
		if (_E000.TryGetValue(name, out var value2))
		{
			value2.Add(this, value);
		}
	}

	private void FixedUpdate()
	{
		RandomDevice[] randomDevices = RandomDevices;
		for (int i = 0; i < randomDevices.Length; i++)
		{
			randomDevices[i].Process(this);
		}
		CurveDevice[] curveDevices = CurveDevices;
		for (int i = 0; i < curveDevices.Length; i++)
		{
			curveDevices[i].Process(this);
		}
		InputDevice[] inputDevices = InputDevices;
		for (int i = 0; i < inputDevices.Length; i++)
		{
			inputDevices[i].Process(this);
		}
		UnityValueDevice[] unityValueDevices = UnityValueDevices;
		for (int i = 0; i < unityValueDevices.Length; i++)
		{
			unityValueDevices[i].Process(this);
		}
		PositionSpring.FixedUpdate(ref PositionVelocity, ref Position);
		RotationSpring.FixedUpdate(ref RotationVelocity, ref Rotation);
		Quaternion quaternion = Quaternion.Euler(Rotation);
		base.transform.localPosition = Position + quaternion * Pivot;
		base.transform.localRotation = quaternion;
	}
}
