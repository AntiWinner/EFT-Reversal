using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GPUInstancer;

public class GrassInitialization : MonoBehaviour
{
	[SerializeField]
	private Transform GrassParent;

	[SerializeField]
	private GPUInstancerPrefab prefab;

	[SerializeField]
	private List<GameObject> PrefabList = new List<GameObject>();

	[SerializeField]
	private List<Color> DryColor = new List<Color>();

	[SerializeField]
	private List<Color> HealthyColor = new List<Color>();

	private static readonly string m__E000 = _ED3E._E000(117188);

	private static string _E001 = Application.streamingAssetsPath + _ED3E._E000(117238);

	private static string _E002 = _E001 + _ED3E._E000(117230);

	private void Awake()
	{
		byte[] array = File.ReadAllBytes(_E002);
		_E4CB obj = default(_E4CB);
		obj.bytes = array;
		_E4CB obj2 = obj;
		int num = Marshal.SizeOf(typeof(_E4CC));
		int num2 = array.Length / num;
		int[] array2 = new int[num2];
		GPUInstancerPrefabManager[] components = base.gameObject.GetComponents<GPUInstancerPrefabManager>();
		Matrix4x4[] array3 = new Matrix4x4[num2];
		for (int i = 0; i < num2; i++)
		{
			array3[i] = obj2.prefabs[i].matrix;
			array2[i] = obj2.prefabs[i].arrayIndex;
		}
		GPUInstancerPrefabManager[] array4 = components;
		foreach (GPUInstancerPrefabManager obj3 in array4)
		{
			obj3.SetColorBuffers(DryColor, HealthyColor);
			_E4BD.InitializeWithMatrix4x4Array(obj3, prefab.prefabPrototype, array3);
			_E4BD.SetInstanceCount(obj3, prefab.prefabPrototype, num2);
			_E4BD.DefineAndAddVariationFromArray(obj3, prefab.prefabPrototype, GrassInitialization.m__E000, array2);
		}
		_E000();
	}

	private void _E000()
	{
	}
}
