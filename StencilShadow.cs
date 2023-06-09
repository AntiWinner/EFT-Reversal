using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
[_E2E2(-100)]
[DisallowMultipleComponent]
public class StencilShadow : MonoBehaviour
{
	public Color Ambient = new Color(0f, 0f, 0f, 1f);

	public AmbientLight.CullingSettings Culling;

	[CompilerGenerated]
	private Renderer _E000;

	[CompilerGenerated]
	private Bounds _E001;

	public int DesiredDrawPriority = -1;

	[HideInInspector]
	[SerializeField]
	private int _drawPriority = -1;

	private Mesh _E002;

	public Renderer Renderer
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		private set
		{
			_E000 = value;
		}
	}

	public virtual Bounds Bounds
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		protected set
		{
			_E001 = value;
		}
	}

	public int ActualDrawPriority => _drawPriority;

	[ContextMenu("Awake")]
	public void Awake()
	{
		if (Culling != null)
		{
			Culling.Update();
		}
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			_E002 = component.sharedMesh;
		}
		Renderer = GetComponent<Renderer>();
		if (!(Renderer == null))
		{
			Bounds = Renderer.bounds;
			if (base.isActiveAndEnabled)
			{
				AmbientLight.AddStencilShadow(this);
			}
		}
	}

	private void OnEnable()
	{
		Awake();
	}

	private void OnDisable()
	{
		AmbientLight.RemoveStencilShadow(this);
	}

	private void OnValidate()
	{
		Awake();
	}

	private void OnDestroy()
	{
		AmbientLight.RemoveStencilShadow(this);
	}
}
