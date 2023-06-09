using System;
using UnityEngine;

namespace EFT.CameraControl;

public class CameraLodBiasController : MonoBehaviour
{
	[SerializeField]
	public float LodBiasFactor = 1f;

	private float _E000 = 75f;

	private float _E001 = 1f;

	public void Awake()
	{
		_E000 = GetComponent<Camera>().fieldOfView;
	}

	public void SetMaxFov(float fov)
	{
		_E000 = fov;
	}

	public void SetBiasByFov(float fov)
	{
		LodBiasFactor = Mathf.Tan((float)Math.PI / 180f * fov * 0.5f) / Mathf.Tan((float)Math.PI / 180f * _E000 * 0.5f);
	}

	private void OnPreCull()
	{
		_E001 = QualitySettings.lodBias;
		QualitySettings.lodBias = _E001 * LodBiasFactor;
	}

	private void OnPostRender()
	{
		QualitySettings.lodBias = _E001;
	}
}
