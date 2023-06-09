using UnityEngine;

public class DistortRenderer : MonoBehaviour
{
	private Renderer m__E000;

	private Material _E001;

	public Renderer Renderer
	{
		get
		{
			if (this.m__E000 != null)
			{
				return this.m__E000;
			}
			_E000();
			return this.m__E000;
		}
	}

	public Material MeterialToRender
	{
		get
		{
			if (_E001 != null)
			{
				return _E001;
			}
			_E000();
			return _E001;
		}
	}

	private void Start()
	{
		if (this.m__E000 == null || _E001 == null)
		{
			_E000();
		}
	}

	private void _E000()
	{
		this.m__E000 = GetComponent<Renderer>();
		this.m__E000.enabled = GetComponent<ParticleSystem>() != null;
		Material material = new Material(_E3AC.Find(_ED3E._E000(37921)));
		material.hideFlags = HideFlags.HideAndDontSave;
		_E001 = new Material(this.m__E000.sharedMaterial);
		this.m__E000.sharedMaterial = material;
		DistortCameraFX.AddRenderer(this);
	}

	private void OnDestroy()
	{
		DistortCameraFX.RemoveRenderer(this);
	}

	private void OnEnable()
	{
		DistortCameraFX.AddRenderer(this);
	}

	private void OnDisable()
	{
		DistortCameraFX.RemoveRenderer(this);
	}
}
