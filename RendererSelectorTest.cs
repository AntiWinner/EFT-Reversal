using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class RendererSelectorTest : MonoBehaviour
{
	private Collider m__E000;

	private Plane[] m__E001 = new Plane[6];

	[SerializeField]
	private bool _updateRendersEveryFrame;

	[SerializeField]
	private bool _updatePlanesEveryFrame = true;

	[SerializeField]
	private bool _updateSelectionEveryFrame;

	private ICollection<MeshRenderer> m__E002;

	[SerializeField]
	private List<MeshRenderer> _selectedRenderers = new List<MeshRenderer>();

	private void Awake()
	{
		this.m__E000 = GetComponent<Collider>();
	}

	private void Update()
	{
		if (!(this.m__E000 == null))
		{
			if (_updateRendersEveryFrame)
			{
				_E000();
			}
			if (_updatePlanesEveryFrame)
			{
				_E001();
			}
			if (_updateSelectionEveryFrame)
			{
				_E002();
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		foreach (MeshRenderer selectedRenderer in _selectedRenderers)
		{
			Gizmos.DrawCube(selectedRenderer.bounds.center, selectedRenderer.bounds.size);
		}
	}

	private void _E000()
	{
		this.m__E002 = _E38D.GetAllComponentsOfType<MeshRenderer>(includeInactive: false);
	}

	private void _E001()
	{
		Bounds colliderBoundsWithoutRotation = _E38D.GetColliderBoundsWithoutRotation(this.m__E000);
		Quaternion rotation = this.m__E000.transform.rotation;
		_E379.ConvertBoundsToPlanesNonAlloc(colliderBoundsWithoutRotation, rotation, normalsIn: true, this.m__E001);
	}

	private void _E002()
	{
		_selectedRenderers.Clear();
		foreach (MeshRenderer item in this.m__E002)
		{
			if (GeometryUtility.TestPlanesAABB(this.m__E001, item.bounds))
			{
				_selectedRenderers.Add(item);
			}
		}
	}
}
