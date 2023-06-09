using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Colorful/Vintage")]
public class CC_Vintage : CC_LookupFilter
{
	public enum Filter
	{
		None,
		K506,
		Zabid,
		Cognac,
		Edwards,
		Cheese,
		LateGoose,
		Bread,
		Montreal,
		Feather,
		Jason,
		Fahrenheit,
		Owl,
		Chillwave,
		Albert,
		Bayswater,
		Atlanta,
		Felicity,
		Stefano,
		Boost,
		Emilia,
		Doze,
		Clifden,
		Blender,
		Tokyo,
		Walk,
		Olive,
		Hotshot
	}

	public Filter filter;

	protected Filter m_CurrentFilter;

	protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (filter != m_CurrentFilter)
		{
			m_CurrentFilter = filter;
			if (filter == Filter.None)
			{
				lookupTexture = null;
			}
			else
			{
				lookupTexture = _E3A2.Load<Texture2D>(_ED3E._E000(17027) + filter);
			}
		}
		base.OnRenderImage(source, destination);
	}
}
