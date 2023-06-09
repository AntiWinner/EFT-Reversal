using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class AstroidGenerator : MonoBehaviour
{
	[Range(0f, 200000f)]
	public int count = 50000;

	public List<GPUInstancerPrefab> asteroidObjects = new List<GPUInstancerPrefab>();

	public GPUInstancerPrefabManager prefabManager;

	public Transform centerTransform;

	private List<GPUInstancerPrefab> m__E000 = new List<GPUInstancerPrefab>();

	private int m__E001;

	private Vector3 _E002;

	private Vector3 _E003;

	private Quaternion _E004;

	private Vector3 _E005;

	private Vector3 _E006;

	private GPUInstancerPrefab _E007;

	private GameObject _E008;

	private float _E009;

	private int _E00A;

	private int _E00B = 3;

	private void Awake()
	{
		this.m__E001 = 0;
		_E002 = centerTransform.position;
		_E003 = Vector3.zero;
		_E004 = Quaternion.identity;
		_E005 = Vector3.zero;
		_E006 = Vector3.one;
		_E009 = 1f;
		_E008 = new GameObject(_ED3E._E000(74686));
		_E008.transform.position = _E002;
		_E008.transform.parent = base.gameObject.transform;
		_E00A = ((count < 5000) ? 1 : (count / 2500));
		int num = ((count % _E00A > 0) ? (_E00A - 1) : _E00A);
		this.m__E000.Clear();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < Mathf.FloorToInt((float)count / (float)_E00A); j++)
			{
				this.m__E000.Add(_E001(_E002, i));
			}
		}
		if (num != _E00A)
		{
			for (int k = 0; k < count - Mathf.FloorToInt((float)count / (float)_E00A) * num; k++)
			{
				this.m__E000.Add(_E001(_E002, _E00A));
			}
		}
		Debug.Log(_ED3E._E000(74672) + this.m__E001 + _ED3E._E000(74662));
	}

	private void Start()
	{
		if (prefabManager != null && prefabManager.gameObject.activeSelf && prefabManager.enabled)
		{
			_E4BD.RegisterPrefabInstanceList(prefabManager, this.m__E000);
			_E4BD.InitializeGPUInstancer(prefabManager);
		}
	}

	private void _E000(Vector3 center, int column, float radius)
	{
		float num = UnityEngine.Random.value * 360f;
		_E003.x = center.x + radius * Mathf.Sin(num * ((float)Math.PI / 180f));
		_E003.y = center.y - (float)column * (float)_E00B / 2f + (float)(column * _E00B) + UnityEngine.Random.Range(0f, 1f);
		_E003.z = center.z + radius * Mathf.Cos(num * ((float)Math.PI / 180f));
	}

	private GPUInstancerPrefab _E001(Vector3 center, int column)
	{
		_E000(center, column - Mathf.FloorToInt((float)_E00A / 2f), UnityEngine.Random.Range(80f, 150f));
		_E004 = Quaternion.FromToRotation(Vector3.forward, center - _E003);
		_E007 = UnityEngine.Object.Instantiate(asteroidObjects[UnityEngine.Random.Range(0, asteroidObjects.Count)], _E003, _E004);
		_E007.transform.parent = _E008.transform;
		_E005.x = UnityEngine.Random.Range(-180f, 180f);
		_E005.y = UnityEngine.Random.Range(-180f, 180f);
		_E005.z = UnityEngine.Random.Range(-180f, 180f);
		_E007.transform.localRotation = Quaternion.Euler(_E005);
		_E009 = UnityEngine.Random.Range(0.3f, 1.2f);
		_E006.x = _E009;
		_E006.y = _E009;
		_E006.z = _E009;
		_E007.transform.localScale = _E006;
		this.m__E001++;
		return _E007;
	}
}
