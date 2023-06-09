using BSG.CameraEffects;
using EFT.PostEffects;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace EFT.CameraControl;

public class OpticSight : MonoBehaviour
{
	public struct _E000
	{
		public OpticSight OpticSight;

		public bool IsEnabled;
	}

	public static _ECF5<_E000> OpticSightState = new _ECF5<_E000>();

	public Renderer LensRenderer;

	public Transform ScopeTransform;

	[SerializeField]
	public float DistanceToCamera;

	[SerializeField]
	public Camera TemplateCamera;

	[SerializeField]
	public OpticCullingMask OpticCullingMask;

	[SerializeField]
	public ChromaticAberration ChromaticAberration;

	[SerializeField]
	public BloomOptimized BloomOptimized;

	[SerializeField]
	public ThermalVision ThermalVision;

	[SerializeField]
	public CC_FastVignette FastVignette;

	[SerializeField]
	public UltimateBloom UltimateBloom;

	[SerializeField]
	public Tonemapping Tonemapping;

	[SerializeField]
	public NightVision NightVision;

	[SerializeField]
	public Fisheye Fisheye;

	[SerializeField]
	public CameraLodBiasController CameraLodBiasController;

	[SerializeField]
	[Tooltip("ALARM! Consumes a lot of CPU!")]
	public bool IsThermalSightAvailableAt45Degrees;

	private readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(179387));

	public void Awake()
	{
		TemplateCamera.gameObject.SetActive(value: false);
	}

	public void LensFade(bool isHide = true)
	{
		LensRenderer.material.SetFloat(this.m__E000, isHide ? 0.97f : 0f);
	}

	private void OnEnable()
	{
		LensFade(isHide: false);
		OpticSightState.Value = new _E000
		{
			IsEnabled = true,
			OpticSight = this
		};
	}

	private void OnDisable()
	{
		LensFade();
		OpticSightState.Value = new _E000
		{
			IsEnabled = false,
			OpticSight = this
		};
	}

	[ContextMenu("Calc Distance")]
	public void CalcDistance()
	{
		if (ScopeTransform != null)
		{
			DistanceToCamera = Vector3.Distance(ScopeTransform.position, LensRenderer.transform.position);
			return;
		}
		Transform transform = base.transform.Find(_ED3E._E000(134736));
		if (transform == null)
		{
			Debug.Log(_ED3E._E000(179340) + base.name);
		}
		else
		{
			DistanceToCamera = Vector3.Distance(transform.position, LensRenderer.transform.position);
		}
	}

	public void LookAt(Vector3 point, Vector3 worldUp)
	{
		TemplateCamera.transform.LookAt(point, worldUp);
		if (IsThermalSightAvailableAt45Degrees && TemplateCamera.transform.localRotation.eulerAngles.z - 1f > 0f)
		{
			TemplateCamera.transform.Rotate(0f, 0f, 0f - TemplateCamera.transform.localRotation.eulerAngles.z);
		}
	}
}
