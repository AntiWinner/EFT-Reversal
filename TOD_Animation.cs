using System.Runtime.CompilerServices;
using UnityEngine;

public class TOD_Animation : MonoBehaviour
{
	[Tooltip("Wind direction in degrees.")]
	public float WindDegrees;

	[Tooltip("Speed of the wind that is acting on the clouds.")]
	public float WindSpeed = 1f;

	[Tooltip("Adjust the cloud coordinates when the sky dome moves.")]
	public bool WorldSpaceCloudUV = true;

	[Tooltip("Randomize the cloud coordinates at startup.")]
	public bool RandomInitialCloudUV = true;

	[CompilerGenerated]
	private Vector4 _E000;

	public Vector2 CloudPosition;

	private TOD_Sky _E001;

	public Vector4 CloudUV
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

	public Vector4 OffsetUV
	{
		get
		{
			if (!WorldSpaceCloudUV)
			{
				return Vector4.zero;
			}
			Vector3 position = base.transform.position;
			Vector3 lossyScale = base.transform.lossyScale;
			Vector3 vector = new Vector3(position.x / lossyScale.x, 0f, position.z / lossyScale.z);
			vector = Quaternion.Euler(0f, 0f - base.transform.rotation.eulerAngles.y, 0f) * vector;
			return new Vector4(vector.x, vector.z, vector.x, vector.z);
		}
	}

	protected void Start()
	{
		_E001 = GetComponent<TOD_Sky>();
	}

	protected void Update()
	{
		Vector2 vector = new Vector2(0.0995037f, 0.9950371f);
		Vector2 vector2 = new Vector2(0f - vector.y, vector.x);
		Vector2 vector3 = new Vector2(0f - vector.x, vector.y);
		Vector2 vector4 = new Vector2(0f - vector3.y, vector3.x);
		Vector2 vector5 = CloudPosition.y * vector + CloudPosition.x * vector2;
		Vector2 vector6 = CloudPosition.y * vector3 + CloudPosition.x * vector4;
		CloudUV = new Vector4(vector5.x, vector5.y, vector6.x, vector6.y);
		CloudUV = new Vector4(CloudUV.x % _E001.Clouds.Scale1.x, CloudUV.y % _E001.Clouds.Scale1.y, CloudUV.z % _E001.Clouds.Scale2.x, CloudUV.w % _E001.Clouds.Scale2.y);
	}
}
