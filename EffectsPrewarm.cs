using System;
using System.Linq;
using Systems.Effects;
using Comfort.Common;
using EFT.Ballistics;
using UnityEngine;

public class EffectsPrewarm : MonoBehaviour
{
	private GameObject m__E000;

	private Camera m__E001;

	private RenderTexture m__E002;

	private void Start()
	{
		_E000();
		_E001();
		_E002();
	}

	private void _E000()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new GameObject(_ED3E._E000(43484), typeof(Camera));
			this.m__E000.transform.SetParent(base.transform);
			this.m__E000.transform.position = new Vector3(0f, -1000f, 0f);
		}
		this.m__E001 = this.m__E000.GetComponent<Camera>();
		this.m__E002 = new RenderTexture(this.m__E001.pixelWidth, this.m__E001.pixelHeight, 24);
		this.m__E001.name = _ED3E._E000(43465);
		this.m__E001.targetTexture = this.m__E002;
	}

	private void _E001()
	{
		ParticleSystem[] array = _E3AA.FindUnityObjectsOfType<ParticleSystem>();
		Vector3 position = this.m__E001.transform.position + this.m__E001.transform.forward;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Emit(new ParticleSystem.EmitParams
			{
				position = position,
				startLifetime = 1f,
				velocity = Vector3.zero,
				startColor = Color.white,
				startSize = 1f
			}, 1);
		}
		foreach (MaterialType item in Enum.GetValues(typeof(MaterialType)).Cast<MaterialType>())
		{
			Singleton<Effects>.Instance.PrewarmEmit(item, null, position, Vector3.one);
		}
	}

	private void _E002()
	{
		UnityEngine.Object.DestroyImmediate(this.m__E000);
		UnityEngine.Object.DestroyImmediate(this.m__E002);
	}
}
