using System;
using UnityEngine;

[Serializable]
[_EBED]
public sealed class ClassVector3
{
	public float x;

	public float y;

	public float z;

	public Vector3 ToUnityVector3()
	{
		return new Vector3(x, y, z);
	}

	public ClassVector3 Clone()
	{
		return new ClassVector3
		{
			x = x,
			y = y,
			z = z
		};
	}

	public static ClassVector3 FromUnityVector3(Vector3 v)
	{
		return new ClassVector3
		{
			x = v.x,
			y = v.y,
			z = v.z
		};
	}

	public static implicit operator Vector3(ClassVector3 vec)
	{
		return vec.ToUnityVector3();
	}

	public static implicit operator ClassVector3(Vector3 vec)
	{
		return FromUnityVector3(vec);
	}

	public static Vector3 operator -(ClassVector3 a, ClassVector3 b)
	{
		return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
	}
}
