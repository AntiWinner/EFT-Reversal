using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancer;

public class PrefabsWithoutGameObjects : MonoBehaviour
{
	public GPUInstancerPrefabManager prefabManager;

	public GPUInstancerPrefab prefab;

	public int bufferSize = 10000;

	public Button addSphere;

	public Button removeSphere;

	public Button clearSphere;

	public Text sphereCountText;

	public Text positionUpdateFrequencyText;

	public Text scaleUpdateFrequencyText;

	public Text colorUpdateFrequencyText;

	private Matrix4x4[] m__E000;

	private int m__E001;

	private float m__E002 = 1f;

	private float m__E003 = 1f;

	private float m__E004 = 1f;

	private Vector4[] m__E005;

	private string _E006 = _ED3E._E000(40500);

	private void Awake()
	{
		if (bufferSize < 1000)
		{
			bufferSize = 1000;
		}
		this.m__E000 = new Matrix4x4[bufferSize];
		_E003(1000);
		_E4BD.InitializeWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000);
		_E4BD.SetInstanceCount(prefabManager, prefab.prefabPrototype, this.m__E001);
		this.m__E005 = new Vector4[bufferSize];
		for (int i = 0; i < bufferSize; i++)
		{
			this.m__E005[i] = Random.ColorHSV();
		}
		_E4BD.DefineAndAddVariationFromArray(prefabManager, prefab.prefabPrototype, _E006, this.m__E005);
		_E005();
		StartCoroutine(_E000());
		StartCoroutine(_E001());
		StartCoroutine(_E002());
	}

	private IEnumerator _E000()
	{
		while (true)
		{
			if (this.m__E001 > 100 && this.m__E002 > 0f)
			{
				int num = Random.Range(0, this.m__E001 - 100);
				for (int i = num; i < num + 100; i++)
				{
					Vector3 vector = Random.insideUnitSphere * 20f;
					this.m__E000[i].SetColumn(3, new Vector4(vector.x, vector.y, vector.z, this.m__E000[i][3, 3]));
				}
				_E4BD.UpdateVisibilityBufferWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000, num, num, 100);
			}
			yield return new WaitForSeconds(1f - this.m__E002 + 0.01f);
		}
	}

	private IEnumerator _E001()
	{
		while (true)
		{
			if (this.m__E001 > 100 && this.m__E003 > 0f)
			{
				int num = Random.Range(0, this.m__E001 - 100);
				for (int i = num; i < num + 100; i++)
				{
					Matrix4x4 matrix4x = this.m__E000[i];
					Vector3 pos = matrix4x.GetColumn(3);
					Quaternion q = Quaternion.LookRotation(matrix4x.GetColumn(2), matrix4x.GetColumn(1));
					Vector3 s = Vector3.one * Random.Range(0.5f, 1.5f);
					this.m__E000[i] = Matrix4x4.TRS(pos, q, s);
				}
				_E4BD.UpdateVisibilityBufferWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000, num, num, 100);
			}
			yield return new WaitForSeconds(1f - this.m__E003 + 0.01f);
		}
	}

	private IEnumerator _E002()
	{
		while (true)
		{
			if (this.m__E001 > 100 && this.m__E004 > 0f)
			{
				int num = Random.Range(0, this.m__E001 - 100);
				for (int i = num; i < num + 100; i++)
				{
					this.m__E005[i] = Random.ColorHSV();
				}
				_E4BD.UpdateVariationFromArray(prefabManager, prefab.prefabPrototype, _E006, this.m__E005, num, num, 100);
			}
			yield return new WaitForSeconds(1f - this.m__E004 + 0.01f);
		}
	}

	private void _E003(int instanceCount)
	{
		int num = this.m__E001;
		this.m__E001 += instanceCount;
		for (int i = num; i < this.m__E001; i++)
		{
			this.m__E000[i] = Matrix4x4.TRS(Random.insideUnitSphere * 20f, Quaternion.identity, Vector3.one * Random.Range(0.5f, 1.5f));
		}
	}

	private void _E004(int instanceCount)
	{
		int num = this.m__E001;
		this.m__E001 -= instanceCount;
		for (int i = this.m__E001; i < num; i++)
		{
			this.m__E000[i] = Matrix4x4.zero;
		}
	}

	private void _E005()
	{
		if (this.m__E001 + 1000 > bufferSize)
		{
			addSphere.interactable = false;
		}
		else
		{
			addSphere.interactable = true;
		}
		if (this.m__E001 - 1000 < 0)
		{
			removeSphere.interactable = false;
		}
		else
		{
			removeSphere.interactable = true;
		}
		if (this.m__E001 <= 0)
		{
			clearSphere.interactable = false;
		}
		else
		{
			clearSphere.interactable = true;
		}
		sphereCountText.text = _ED3E._E000(74696) + this.m__E001;
	}

	public void SetPositionUpdateFrequency(float updateInterval)
	{
		this.m__E002 = updateInterval;
		if (this.m__E002 <= 0f)
		{
			positionUpdateFrequencyText.text = _ED3E._E000(74751);
		}
		else
		{
			positionUpdateFrequencyText.text = _ED3E._E000(76828) + (1f - this.m__E002 + 0.01f).ToString(_ED3E._E000(29354)) + _ED3E._E000(76806);
		}
	}

	public void SetScaleUpdateFrequency(float updateInterval)
	{
		this.m__E003 = updateInterval;
		if (this.m__E003 <= 0f)
		{
			scaleUpdateFrequencyText.text = _ED3E._E000(76863);
		}
		else
		{
			scaleUpdateFrequencyText.text = _ED3E._E000(76833) + (1f - this.m__E003 + 0.01f).ToString(_ED3E._E000(29354)) + _ED3E._E000(76806);
		}
	}

	public void SetColorUpdateFrequency(float updateInterval)
	{
		this.m__E004 = updateInterval;
		if (this.m__E004 <= 0f)
		{
			colorUpdateFrequencyText.text = _ED3E._E000(76872);
		}
		else
		{
			colorUpdateFrequencyText.text = _ED3E._E000(76914) + (1f - this.m__E004 + 0.01f).ToString(_ED3E._E000(29354)) + _ED3E._E000(76806);
		}
	}

	public void AddSpheres()
	{
		int num = this.m__E001;
		_E003(1000);
		_E005();
		_E4BD.UpdateVisibilityBufferWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000, num, num, 1000);
		_E4BD.SetInstanceCount(prefabManager, prefab.prefabPrototype, this.m__E001);
	}

	public void RemoveSpheres()
	{
		_E004(1000);
		_E005();
		_E4BD.UpdateVisibilityBufferWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000, this.m__E001, this.m__E001, 1000);
		_E4BD.SetInstanceCount(prefabManager, prefab.prefabPrototype, this.m__E001);
	}

	public void ClearSpheres()
	{
		this.m__E001 = 0;
		_E005();
		this.m__E000 = new Matrix4x4[bufferSize];
		_E4BD.UpdateVisibilityBufferWithMatrix4x4Array(prefabManager, prefab.prefabPrototype, this.m__E000);
		_E4BD.SetInstanceCount(prefabManager, prefab.prefabPrototype, this.m__E001);
	}
}
