using UnityEngine;

namespace MirzaBeig.Scripting.Effects;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleFlocking : MonoBehaviour
{
	public struct _E000
	{
		public Bounds bounds;

		public int[] particles;

		public int particleCount;
	}

	[Header("N^2 Mode Settings")]
	public float maxDistance = 0.5f;

	[Header("Forces")]
	public float cohesion = 0.5f;

	public float separation = 0.25f;

	[Header("Voxel Mode Settings")]
	public bool useVoxels = true;

	public bool voxelLocalCenterFromBounds = true;

	public float voxelVolume = 8f;

	public int voxelsPerAxis = 5;

	private int m__E000;

	private _E000[] _E001;

	private ParticleSystem _E002;

	private ParticleSystem.Particle[] _E003;

	private Vector3[] _E004;

	private ParticleSystem.MainModule _E005;

	[Range(0f, 1f)]
	[Header("General Performance Settings")]
	public float delay;

	private float _E006;

	public bool alwaysUpdate;

	private bool _E007;

	private void Start()
	{
		_E002 = GetComponent<ParticleSystem>();
		_E005 = _E002.main;
	}

	private void OnBecameVisible()
	{
		_E007 = true;
	}

	private void OnBecameInvisible()
	{
		_E007 = false;
	}

	private void _E000()
	{
		int num = voxelsPerAxis * voxelsPerAxis * voxelsPerAxis;
		_E001 = new _E000[num];
		float num2 = voxelVolume / (float)voxelsPerAxis;
		float num3 = num2 / 2f;
		float num4 = voxelVolume / 2f;
		Vector3 position = base.transform.position;
		int num5 = 0;
		for (int i = 0; i < voxelsPerAxis; i++)
		{
			float x = 0f - num4 + num3 + (float)i * num2;
			for (int j = 0; j < voxelsPerAxis; j++)
			{
				float y = 0f - num4 + num3 + (float)j * num2;
				for (int k = 0; k < voxelsPerAxis; k++)
				{
					float z = 0f - num4 + num3 + (float)k * num2;
					_E001[num5].particleCount = 0;
					_E001[num5].bounds = new Bounds(position + new Vector3(x, y, z), Vector3.one * num2);
					num5++;
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!alwaysUpdate && !_E007)
		{
			return;
		}
		if (useVoxels)
		{
			int num = voxelsPerAxis * voxelsPerAxis * voxelsPerAxis;
			if (_E001 == null || _E001.Length < num)
			{
				_E000();
			}
		}
		int maxParticles = _E005.maxParticles;
		if (_E003 == null || _E003.Length < maxParticles)
		{
			_E003 = new ParticleSystem.Particle[maxParticles];
			_E004 = new Vector3[maxParticles];
			if (useVoxels)
			{
				for (int i = 0; i < _E001.Length; i++)
				{
					_E001[i].particles = new int[maxParticles];
				}
			}
		}
		_E006 += Time.deltaTime;
		if (!(_E006 >= delay))
		{
			return;
		}
		float num2 = _E006;
		_E006 = 0f;
		_E002.GetParticles(_E003);
		int particleCount = _E002.particleCount;
		float num3 = cohesion * num2;
		float num4 = separation * num2;
		for (int j = 0; j < particleCount; j++)
		{
			_E004[j] = _E003[j].position;
		}
		if (useVoxels)
		{
			int num5 = _E001.Length;
			float num6 = voxelVolume / (float)voxelsPerAxis;
			for (int k = 0; k < particleCount; k++)
			{
				for (int l = 0; l < num5; l++)
				{
					if (_E001[l].bounds.Contains(_E004[k]))
					{
						_E001[l].particles[_E001[l].particleCount] = k;
						_E001[l].particleCount++;
						break;
					}
				}
			}
			for (int m = 0; m < num5; m++)
			{
				if (_E001[m].particleCount <= 1)
				{
					continue;
				}
				for (int n = 0; n < _E001[m].particleCount; n++)
				{
					Vector3 vector = _E004[_E001[m].particles[n]];
					Vector3 vector2;
					if (voxelLocalCenterFromBounds)
					{
						vector2 = _E001[m].bounds.center - _E004[_E001[m].particles[n]];
					}
					else
					{
						for (int num7 = 0; num7 < _E001[m].particleCount; num7++)
						{
							if (num7 != n)
							{
								vector += _E004[_E001[m].particles[num7]];
							}
						}
						vector /= (float)_E001[m].particleCount;
						vector2 = vector - _E004[_E001[m].particles[n]];
					}
					float sqrMagnitude = vector2.sqrMagnitude;
					vector2.Normalize();
					Vector3 zero = Vector3.zero;
					zero += vector2 * num3;
					zero -= vector2 * ((1f - sqrMagnitude / num6) * num4);
					Vector3 velocity = _E003[_E001[m].particles[n]].velocity;
					velocity.x += zero.x;
					velocity.y += zero.y;
					velocity.z += zero.z;
					_E003[_E001[m].particles[n]].velocity = velocity;
				}
				_E001[m].particleCount = 0;
			}
		}
		else
		{
			float num8 = maxDistance * maxDistance;
			Vector3 vector4 = default(Vector3);
			for (int num9 = 0; num9 < particleCount; num9++)
			{
				int num10 = 1;
				Vector3 vector3 = _E004[num9];
				for (int num11 = 0; num11 < particleCount; num11++)
				{
					if (num11 != num9)
					{
						vector4.x = _E004[num9].x - _E004[num11].x;
						vector4.y = _E004[num9].y - _E004[num11].y;
						vector4.z = _E004[num9].z - _E004[num11].z;
						if (Vector3.SqrMagnitude(vector4) <= num8)
						{
							num10++;
							vector3 += _E004[num11];
						}
					}
				}
				if (num10 != 1)
				{
					vector3 /= (float)num10;
					Vector3 vector5 = vector3 - _E004[num9];
					float sqrMagnitude2 = vector5.sqrMagnitude;
					vector5.Normalize();
					Vector3 zero2 = Vector3.zero;
					zero2 += vector5 * num3;
					zero2 -= vector5 * ((1f - sqrMagnitude2 / num8) * num4);
					Vector3 velocity2 = _E003[num9].velocity;
					velocity2.x += zero2.x;
					velocity2.y += zero2.y;
					velocity2.z += zero2.z;
					_E003[num9].velocity = velocity2;
				}
			}
		}
		_E002.SetParticles(_E003, particleCount);
	}

	private void OnDrawGizmosSelected()
	{
		float num = voxelVolume / (float)voxelsPerAxis;
		float num2 = num / 2f;
		float num3 = voxelVolume / 2f;
		Vector3 position = base.transform.position;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(position, Vector3.one * voxelVolume);
		Gizmos.color = Color.white;
		for (int i = 0; i < voxelsPerAxis; i++)
		{
			float x = 0f - num3 + num2 + (float)i * num;
			for (int j = 0; j < voxelsPerAxis; j++)
			{
				float y = 0f - num3 + num2 + (float)j * num;
				for (int k = 0; k < voxelsPerAxis; k++)
				{
					float z = 0f - num3 + num2 + (float)k * num;
					Gizmos.DrawWireCube(position + new Vector3(x, y, z), Vector3.one * num);
				}
			}
		}
	}
}
