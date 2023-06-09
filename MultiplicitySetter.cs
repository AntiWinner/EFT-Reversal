using System;
using UnityEngine;

public class MultiplicitySetter : MonoBehaviour
{
	public Camera Camera;

	public float BaseFov;

	public float Multiplicity;

	private float m__E000;

	private void Start()
	{
		this.m__E000 = Mathf.Tan(BaseFov * ((float)Math.PI / 180f));
		if (Camera == null)
		{
			Camera = GetComponent<Camera>();
		}
		if (!(Camera == null))
		{
			SetMultiplicity(Multiplicity);
		}
	}

	private void OnValidate()
	{
		Start();
	}

	public void SetMultiplicity(float multiplicity)
	{
		Camera.fieldOfView = _E000(multiplicity);
	}

	private float _E000(float multiplicity)
	{
		return 57.29578f * Mathf.Atan(this.m__E000 / multiplicity);
	}

	private float _E001(float fov)
	{
		return this.m__E000 / Mathf.Tan(fov * ((float)Math.PI / 180f));
	}
}
