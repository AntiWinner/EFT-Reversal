using UnityEngine;

public class DynamicStencilShadow : StencilShadow
{
	public override Bounds Bounds
	{
		get
		{
			return base.Renderer.bounds;
		}
		protected set
		{
		}
	}
}
