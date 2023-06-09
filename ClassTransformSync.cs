using System;

[Serializable]
[_EBED]
public class ClassTransformSync
{
	public ClassVector3 Position;

	public ClassQuaternion Rotation;

	public _E53B ToUnity()
	{
		_E53B result = default(_E53B);
		result.Position = Position.ToUnityVector3();
		result.Rotation = Rotation.ToUnityQuaternion();
		return result;
	}

	public static ClassTransformSync FromUnity(_E53B transformSync)
	{
		return new ClassTransformSync
		{
			Position = transformSync.Position,
			Rotation = transformSync.Rotation
		};
	}

	public static _E53B[] ToUnity(ClassTransformSync[] syncs)
	{
		_E53B[] array = new _E53B[syncs.Length];
		for (int i = 0; i < syncs.Length; i++)
		{
			ClassTransformSync classTransformSync = syncs[i];
			array[i] = classTransformSync.ToUnity();
		}
		return array;
	}

	public static ClassTransformSync[] FromUnity(_E53B[] syncs)
	{
		ClassTransformSync[] array = new ClassTransformSync[syncs.Length];
		for (int i = 0; i < syncs.Length; i++)
		{
			_E53B transformSync = syncs[i];
			array[i] = FromUnity(transformSync);
		}
		return array;
	}
}
