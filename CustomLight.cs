using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class CustomLight : MonoBehaviour, _E411
{
	private Renderer m__E000;

	private void Awake()
	{
		this.m__E000 = GetComponent<Renderer>();
		this.m__E000.enabled = false;
		LightManager.Add(this);
	}

	private Renderer _E000()
	{
		if (this.m__E000 != null)
		{
			return this.m__E000;
		}
		this.m__E000 = GetComponent<Renderer>();
		return this.m__E000;
	}

	private void OnEnable()
	{
		LightManager.Add(this);
	}

	private void OnDisable()
	{
		LightManager.Remove(this);
	}

	private void OnDestroy()
	{
		OnDisable();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, _E38D.True ? _ED3E._E000(90047) : _ED3E._E000(89998), allowScaling: true);
	}

	public void Draw(CommandBuffer buffer, Plane[] frustrumPlanes, Camera cam)
	{
		if (GeometryUtility.TestPlanesAABB(frustrumPlanes, this.m__E000.bounds))
		{
			buffer.DrawRenderer(this.m__E000, this.m__E000.sharedMaterial);
		}
	}
}
