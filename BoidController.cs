using GPUInstancer;
using UnityEngine;

public class BoidController : MonoBehaviour
{
	public static class _E000
	{
		public static readonly int BP_boidsData = Shader.PropertyToID(_ED3E._E000(40488));

		public static readonly int BP_bufferSize = Shader.PropertyToID(_ED3E._E000(40482));

		public static readonly int BP_controllerTransform = Shader.PropertyToID(_ED3E._E000(40533));

		public static readonly int BP_controllerVelocity = Shader.PropertyToID(_ED3E._E000(40513));

		public static readonly int BP_controllerVelocityVariation = Shader.PropertyToID(_ED3E._E000(40556));

		public static readonly int BP_controllerRotationCoeff = Shader.PropertyToID(_ED3E._E000(40592));

		public static readonly int BP_controllerNeighborDist = Shader.PropertyToID(_ED3E._E000(40632));

		public static readonly int BP_time = Shader.PropertyToID(_ED3E._E000(40615));

		public static readonly int BP_deltaTime = Shader.PropertyToID(_ED3E._E000(40668));

		public static readonly int BP_noiseTexture = Shader.PropertyToID(_ED3E._E000(40662));
	}

	public int spawnCount = 10;

	public float spawnRadius = 4f;

	[Range(0.1f, 20f)]
	public float velocity = 6f;

	[Range(0f, 0.9f)]
	public float velocityVariation = 0.5f;

	[Range(0.1f, 20f)]
	public float rotationCoeff = 4f;

	[Range(0.1f, 10f)]
	public float neighborDist = 2f;

	private Matrix4x4[] m__E000;

	private Vector4[] _E001;

	public Transform centerTransform;

	public Texture2D noiseTexture;

	private GPUInstancerPrefabManager _E002;

	private ComputeShader _E003;

	private string _E004 = _ED3E._E000(40500);

	private float[] _E005 = new float[16];

	private ComputeBuffer _E006;

	private void Start()
	{
		this.m__E000 = new Matrix4x4[spawnCount];
		_E001 = new Vector4[spawnCount];
		for (int i = 0; i < spawnCount; i++)
		{
			Spawn(i);
		}
		_E002 = Object.FindObjectOfType<GPUInstancerPrefabManager>();
		if (_E002 != null && _E002.prototypeList.Count > 0)
		{
			GPUInstancerPrefabPrototype prototype = (GPUInstancerPrefabPrototype)_E002.prototypeList[0];
			_E4BD.InitializeWithMatrix4x4Array(_E002, prototype, this.m__E000);
			_E4BD.DefineAndAddVariationFromArray(_E002, prototype, _E004, _E001);
			_E006 = _E4BD.GetTransformDataBuffer(_E002, prototype);
		}
	}

	public void Spawn(int index)
	{
		this.m__E000[index] = Matrix4x4.TRS(centerTransform.position + Random.insideUnitSphere * spawnRadius, Random.rotation, Vector3.one);
		_E001[index] = Random.ColorHSV();
	}

	private void Update()
	{
		if (_E003 == null)
		{
			_E003 = Resources.Load<ComputeShader>(_ED3E._E000(40450));
		}
		if (!(_E003 == null) && _E006 != null)
		{
			centerTransform.localToWorldMatrix.Matrix4x4ToFloatArray(_E005);
			_E003.SetBuffer(0, BoidController._E000.BP_boidsData, _E006);
			_E003.SetTexture(0, BoidController._E000.BP_noiseTexture, noiseTexture);
			_E003.SetInt(BoidController._E000.BP_bufferSize, spawnCount);
			_E003.SetFloats(BoidController._E000.BP_controllerTransform, _E005);
			_E003.SetFloat(BoidController._E000.BP_controllerVelocity, velocity);
			_E003.SetFloat(BoidController._E000.BP_controllerVelocityVariation, velocityVariation);
			_E003.SetFloat(BoidController._E000.BP_controllerRotationCoeff, rotationCoeff);
			_E003.SetFloat(BoidController._E000.BP_controllerNeighborDist, neighborDist);
			_E003.SetFloat(BoidController._E000.BP_time, Time.time);
			_E003.SetFloat(BoidController._E000.BP_deltaTime, Time.deltaTime);
			_E003.Dispatch(0, Mathf.CeilToInt((float)spawnCount / _E4BF.COMPUTE_SHADER_THREAD_COUNT), 1, 1);
		}
	}
}
