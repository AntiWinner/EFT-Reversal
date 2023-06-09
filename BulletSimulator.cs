using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Ballistics;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSimulator : MonoBehaviour
{
	private BallisticsCalculator m__E000;

	private _EC26 m__E001;

	private Text m__E002;

	[Header("Ammo data")]
	public AmmoTemplate AmmoTemplate;

	public _EA12 Ammo;

	[Header("Aiming data")]
	public float MaxDistance = 2000f;

	public float CurrentDistance = 10f;

	public float DistanceStep = 5f;

	[Header("Simulation data")]
	public float SimulationStep = 1E-05f;

	public float SimulationTime = 4f;

	public float MarkerScaleStep = 0.003f;

	private GameObject m__E003;

	private async Task Start()
	{
		Ammo = new _EA12("", AmmoTemplate);
		await _E001();
		_E002();
		_E003();
		SimulateShot();
		_E000();
		_E004();
	}

	private void _E000()
	{
		this.m__E003 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		this.m__E003.transform.position = base.transform.position;
		this.m__E003.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
	}

	private static async Task _E001()
	{
		IOperation<_EC35> operation = await _E5DC.CreateAssetsManager();
		if (operation.Succeed)
		{
			_E5DB.Manager = operation.Result;
		}
	}

	private void _E002()
	{
		Canvas canvas = new GameObject(_ED3E._E000(40651)).AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		Text text = new GameObject(_ED3E._E000(40690)).AddComponent<Text>();
		text.transform.SetParent(canvas.transform);
		text.font = Resources.GetBuiltinResource<Font>(_ED3E._E000(29433));
		text.color = Color.green;
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		text.rectTransform.anchorMin = new Vector2(0.7f, 0f);
		text.rectTransform.anchorMax = new Vector2(1f, 0.5f);
		text.rectTransform.offsetMax = Vector2.zero;
		text.rectTransform.offsetMin = Vector2.zero;
		this.m__E002 = text;
	}

	private void _E003()
	{
		this.m__E000 = BallisticsCalculator.Create(base.gameObject, 0, OnShotFinishedHandler);
	}

	public void SimulateShot()
	{
		Debug.Log(_ED3E._E000(40735));
		this.m__E001 = this.m__E000.CreateShot(Ammo, base.transform.position, base.transform.forward, 1, null, null);
		this.m__E000.SimulateShot(this.m__E001, SimulationTime, SimulationStep);
	}

	public void OnShotFinishedHandler(_EC26 shot)
	{
		Debug.Log(_ED3E._E000(40716));
	}

	public void Update()
	{
		if (this.m__E001 != null)
		{
			_E005();
			_E007();
		}
	}

	private void _E004()
	{
		if (this.m__E001 == null)
		{
			this.m__E003.SetActive(value: false);
			return;
		}
		this.m__E003.SetActive(value: true);
		this.m__E003.transform.position = base.transform.position + base.transform.forward * CurrentDistance;
		List<Vector3> positionHistory = this.m__E001.PositionHistory;
		int i;
		for (i = 0; i < positionHistory.Count && !(Vector3.Dot(positionHistory[i], base.transform.forward) > CurrentDistance); i++)
		{
		}
		if (i == 0)
		{
			this.m__E003.transform.position = positionHistory[i];
		}
		else if (i == positionHistory.Count)
		{
			this.m__E003.transform.position = positionHistory[positionHistory.Count - 1];
		}
		else
		{
			Vector3 lhs = positionHistory[i - 1];
			Vector3 lhs2 = positionHistory[i];
			float num = Vector3.Dot(lhs2, base.transform.forward) - Vector3.Dot(lhs, base.transform.forward);
			float num2 = (CurrentDistance - Vector3.Dot(lhs, base.transform.forward)) / num * (Vector3.Dot(lhs2, base.transform.up) - Vector3.Dot(lhs, base.transform.up));
			this.m__E003.transform.position = new Vector3(base.transform.position.x, lhs.y + num2, CurrentDistance);
		}
		float num3 = Vector3.Distance(this.m__E003.transform.position, base.transform.position) * MarkerScaleStep;
		this.m__E003.transform.localScale = num3 * Vector3.one;
	}

	private void _E005()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			_E006(Mathf.Min(CurrentDistance + DistanceStep, MaxDistance));
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			_E006(Mathf.Max(0f, CurrentDistance - DistanceStep));
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			SimulateShot();
			_E004();
		}
	}

	private void _E006(float value)
	{
		CurrentDistance = value;
		_E004();
	}

	private void _E007()
	{
		this.m__E002.text = _ED3E._E000(40705) + (Vector3.Dot(this.m__E003.transform.position, base.transform.forward) - base.transform.position.z) + _ED3E._E000(40756) + (Vector3.Dot(this.m__E003.transform.position, base.transform.up) - base.transform.position.y) + _ED3E._E000(40744) + this.m__E003.transform.localScale.x;
	}

	public void OnDrawGizmos()
	{
		if (_E38D.DisabledForNow)
		{
			return;
		}
		Gizmos.color = Color.magenta;
		if (this.m__E001 != null)
		{
			List<Vector3> positionHistory = this.m__E001.PositionHistory;
			for (int i = 1; i < positionHistory.Count; i++)
			{
				Gizmos.DrawLine(positionHistory[i - 1], positionHistory[i]);
			}
		}
	}
}
