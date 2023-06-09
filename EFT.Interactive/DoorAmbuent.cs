using System;
using UnityEngine;

namespace EFT.Interactive;

[ExecuteInEditMode]
[_E2E2(10000)]
public class DoorAmbuent : MonoBehaviour
{
	public AnalyticSource Source;

	public Transform DoorTransform;

	public Door DoorScript;

	public float Angle;

	public bool InvertDoorPosition;

	public bool InvertDoorRotation;

	public bool Rotate180;

	public float DoorShift;

	public float MinTurningOffAngle = 0.2f;

	public float MinDeltaAngle = 0.1f;

	private float m__E000 = -666f;

	private float _E001 = -1f;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(206040));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(206035));

	public void PlacePerfect()
	{
		if (!DoorTransform || !DoorScript)
		{
			if (!DoorTransform)
			{
				Debug.Log(_ED3E._E000(205959));
			}
			if (!DoorScript)
			{
				Debug.Log(_ED3E._E000(205997));
			}
		}
		else
		{
			Vector3 position = DoorTransform.position;
			Vector3 vector = _E000(DoorScript.DoorAxis);
			Vector3 rhs = _E000(DoorScript.DoorForward);
			Vector3 vector2 = Vector3.Cross(vector, rhs);
			Source.transform.rotation = DoorTransform.parent.rotation * Quaternion.LookRotation(Rotate180 ? (-vector2) : vector2, vector);
			position -= Source.DoorSize.x * Source.transform.right * ((!Rotate180) ? 1 : (-1));
			position += Source.transform.forward * (1f + DoorShift);
			position.y = Source.transform.position.y;
			Source.transform.position = position;
			Source.UpdateSettings();
		}
	}

	private static Vector3 _E000(WorldInteractiveObject.EDoorAxis doorAxis)
	{
		return doorAxis switch
		{
			WorldInteractiveObject.EDoorAxis.X => new Vector3(1f, 0f, 0f), 
			WorldInteractiveObject.EDoorAxis.Y => new Vector3(0f, 1f, 0f), 
			WorldInteractiveObject.EDoorAxis.Z => new Vector3(0f, 0f, 1f), 
			WorldInteractiveObject.EDoorAxis.XNegative => new Vector3(-1f, 0f, 0f), 
			WorldInteractiveObject.EDoorAxis.YNegative => new Vector3(0f, -1f, 0f), 
			WorldInteractiveObject.EDoorAxis.ZNegative => new Vector3(0f, 0f, -1f), 
			_ => Vector3.zero, 
		};
	}

	private void LateUpdate()
	{
		float num = (DoorTransform ? (Angle = DoorTransform.localEulerAngles.z) : Angle);
		num = ((num > 180f) ? (num - 360f) : num);
		if (InvertDoorRotation)
		{
			num = 0f - num;
		}
		if (Mathf.Abs(this.m__E000 - num) < MinDeltaAngle)
		{
			return;
		}
		this.m__E000 = num;
		float f = num * ((float)Math.PI / 180f);
		if (Mathf.Abs(num) < MinTurningOffAngle)
		{
			Source.enabled = false;
			return;
		}
		Source.enabled = true;
		if (num > 0f)
		{
			float num2 = Mathf.Sin(f);
			float num3 = Mathf.Cos(f);
			Vector2 vector = new Vector2(num3, num2) * Source.DoorSize.x * 2f;
			Vector4 value = new Vector4(Source.DoorSize.x, -1f, Source.DoorSize.x - vector.x, vector.y - 1f);
			Vector2 vector2 = new Vector2(num2, num3 - 1f) * DoorShift;
			value += new Vector4(vector2.x, vector2.y, vector2.x, vector2.y);
			if (InvertDoorPosition)
			{
				value = new Vector4(0f - value.z, value.w, 0f - value.x, value.y);
			}
			Source.MaterialPropertyBlock.SetVector(_E002, value);
			Source.ShaderPath = 1;
		}
		else if (num > -90f)
		{
			float num4 = Mathf.Sin(f);
			Vector2 vector3 = new Vector2((0f - num4) * 2f, (!InvertDoorPosition) ? 1 : (-1));
			Source.MaterialPropertyBlock.SetVector(_E003, vector3);
			Source.ShaderPath = 2;
		}
		else
		{
			Source.ShaderPath = 0;
		}
	}
}
