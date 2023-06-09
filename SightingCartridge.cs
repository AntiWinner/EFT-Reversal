using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SightingCartridge : MonoBehaviour
{
	public Transform WeaponHierarchy;

	private Transform m__E000;

	private Vector3 _E001;

	private LineRenderer _E002;

	public float GizmoSize = 0.1f;

	private GameObject _E003;

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(36528));

	private async Task Start()
	{
		IOperation<_EC35> operation = await _E5DC.CreateAssetsManager();
		if (!operation.Failed)
		{
			_E5DB.Manager = operation.Result;
			_E002 = GetComponent<LineRenderer>();
			_E002.startWidth = 0.01f;
			_E002.endWidth = 0.05f;
			_E003 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			_E003.transform.localScale = new Vector3(GizmoSize, GizmoSize, GizmoSize);
			_E003.GetComponent<Renderer>().material.SetColor(_E004, Color.red);
			Object.Destroy(_E003.GetComponent<Collider>());
		}
	}

	private void Update()
	{
		if (WeaponHierarchy != null)
		{
			_E000(WeaponHierarchy);
			WeaponHierarchy = null;
		}
		if (this.m__E000 != null)
		{
			_E002.SetPosition(0, this.m__E000.position);
			_E002.SetPosition(1, this.m__E000.position - this.m__E000.up * 600f);
			Vector3 direction = -this.m__E000.up;
			if (Physics.Raycast(this.m__E000.position, direction, out var hitInfo, 600f))
			{
				_E003.transform.position = hitInfo.point;
				_E003.transform.localScale = new Vector3(GizmoSize, GizmoSize, GizmoSize);
			}
		}
	}

	private void _E000(Transform hier)
	{
		this.m__E000 = _E38B.FindTransformRecursive(hier, _ED3E._E000(64493));
	}
}
