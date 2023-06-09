using System;
using DeferredDecals;
using UnityEngine;

public class DeferredDecalMeshHelper : MonoBehaviour
{
	public const int VertsInCube = 24;

	public const int TrigsInCube = 36;

	public const int CubeSides = 6;

	public static void GenerateTrigs(int[] array)
	{
		int num = array.Length / 36;
		if (array.Length % 36 != 0)
		{
			throw new Exception(_ED3E._E000(44268) + array.Length + _ED3E._E000(44319) + 36 + _ED3E._E000(44337));
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = i * 24;
			for (int j = 0; j < 6; j++)
			{
				int num3 = i * 6 * 6 + j * 6;
				int num4 = 4 * j + num2;
				array[num3] = num4 + 3;
				array[num3 + 1] = num4;
				array[num3 + 2] = num4 + 1;
				array[num3 + 3] = num4 + 3;
				array[num3 + 4] = num4 + 1;
				array[num3 + 5] = num4 + 2;
			}
		}
	}

	public static Vector4[] GenerateTangents(Vector4[] array, DeferredDecalRenderer.SingleDecal decal)
	{
		int num = UnityEngine.Random.Range(0, decal.TileSheetColumns);
		int num2 = UnityEngine.Random.Range(0, decal.TileSheetRows);
		Vector4 value = new Vector4((float)num2 * decal.TileUSize, (float)num * decal.TileVSize, (float)num2 * decal.TileUSize + decal.TileUSize, (float)num * decal.TileVSize + decal.TileVSize);
		return array.FillWith(value);
	}

	public static Vector3[] GenerateVerts(Vector3[] a, Vector3 position, Vector3 size, Vector3 right, Vector3 up, Vector3 fwd)
	{
		Vector3 vector = position - right * size.x - up * size.y + fwd * size.z;
		Vector3 vector2 = position + right * size.x - up * size.y + fwd * size.z;
		Vector3 vector3 = position + right * size.x - up * size.y - fwd * size.z;
		Vector3 vector4 = position - right * size.x - up * size.y - fwd * size.z;
		Vector3 vector5 = position - right * size.x + up * size.y + fwd * size.z;
		Vector3 vector6 = position + right * size.x + up * size.y + fwd * size.z;
		Vector3 vector7 = position + right * size.x + up * size.y - fwd * size.z;
		Vector3 vector8 = position - right * size.x + up * size.y - fwd * size.z;
		a[0] = vector;
		a[1] = vector2;
		a[2] = vector3;
		a[3] = vector4;
		a[4] = vector8;
		a[5] = vector5;
		a[6] = vector;
		a[7] = vector4;
		a[8] = vector5;
		a[9] = vector6;
		a[10] = vector2;
		a[11] = vector;
		a[12] = vector7;
		a[13] = vector8;
		a[14] = vector4;
		a[15] = vector3;
		a[16] = vector6;
		a[17] = vector7;
		a[18] = vector3;
		a[19] = vector2;
		a[20] = vector8;
		a[21] = vector7;
		a[22] = vector6;
		a[23] = vector5;
		return a;
	}
}
