using System;
using UnityEngine;

[Serializable]
[_EBED]
public sealed class ClassQuaternion
{
	public float x;

	public float y;

	public float z;

	public float w;

	public Quaternion ToUnityQuaternion()
	{
		return new Quaternion(x, y, z, w);
	}

	public static ClassQuaternion FromUnityQuaternion(Quaternion q)
	{
		return new ClassQuaternion
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}

	public static implicit operator Quaternion(ClassQuaternion q)
	{
		return q.ToUnityQuaternion();
	}

	public static implicit operator ClassQuaternion(Quaternion q)
	{
		return new ClassQuaternion
		{
			x = q.x,
			y = q.y,
			z = q.z,
			w = q.w
		};
	}
}
