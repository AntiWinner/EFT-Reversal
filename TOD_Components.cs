using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class TOD_Components : MonoBehaviour
{
	public GameObject Sun;

	public GameObject Moon;

	public GameObject Atmosphere;

	public GameObject Clear;

	public GameObject Clouds;

	public GameObject Space;

	public GameObject Light;

	public GameObject Projector;

	public GameObject Billboards;

	[CompilerGenerated]
	private Transform _E000;

	[CompilerGenerated]
	private Transform _E001;

	[CompilerGenerated]
	private Transform _E002;

	[CompilerGenerated]
	private Transform _E003;

	[CompilerGenerated]
	private Transform _E004;

	[CompilerGenerated]
	private Renderer _E005;

	[CompilerGenerated]
	private Renderer _E006;

	[CompilerGenerated]
	private Renderer _E007;

	[CompilerGenerated]
	private Renderer _E008;

	[CompilerGenerated]
	private Renderer _E009;

	[CompilerGenerated]
	private Renderer _E00A;

	[CompilerGenerated]
	private MeshFilter _E00B;

	[CompilerGenerated]
	private MeshFilter _E00C;

	[CompilerGenerated]
	private MeshFilter _E00D;

	[CompilerGenerated]
	private MeshFilter _E00E;

	[CompilerGenerated]
	private MeshFilter _E00F;

	[CompilerGenerated]
	private MeshFilter _E010;

	[CompilerGenerated]
	private Material _E011;

	[CompilerGenerated]
	private Material _E012;

	[CompilerGenerated]
	private Material _E013;

	[CompilerGenerated]
	private Material _E014;

	[CompilerGenerated]
	private Material _E015;

	[CompilerGenerated]
	private Material _E016;

	[CompilerGenerated]
	private Material _E017;

	[CompilerGenerated]
	private Light _E018;

	[CompilerGenerated]
	private Projector _E019;

	[CompilerGenerated]
	private TOD_Sky _E01A;

	[CompilerGenerated]
	private TOD_Animation _E01B;

	[CompilerGenerated]
	private TOD_Time _E01C;

	[CompilerGenerated]
	private TOD_Weather _E01D;

	[CompilerGenerated]
	private TOD_Camera _E01E;

	[CompilerGenerated]
	private TOD_Rays _E01F;

	[CompilerGenerated]
	private TOD_Scattering _E020;

	public Transform DomeTransform
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	public Transform SunTransform
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public Transform MoonTransform
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		set
		{
			_E002 = value;
		}
	}

	public Transform LightTransform
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public Transform SpaceTransform
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		set
		{
			_E004 = value;
		}
	}

	public Renderer SpaceRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		set
		{
			_E005 = value;
		}
	}

	public Renderer AtmosphereRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		set
		{
			_E006 = value;
		}
	}

	public Renderer ClearRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		set
		{
			_E007 = value;
		}
	}

	public Renderer CloudRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
		[CompilerGenerated]
		set
		{
			_E008 = value;
		}
	}

	public Renderer SunRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		set
		{
			_E009 = value;
		}
	}

	public Renderer MoonRenderer
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		set
		{
			_E00A = value;
		}
	}

	public MeshFilter SpaceMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E00B;
		}
		[CompilerGenerated]
		set
		{
			_E00B = value;
		}
	}

	public MeshFilter AtmosphereMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		set
		{
			_E00C = value;
		}
	}

	public MeshFilter ClearMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		set
		{
			_E00D = value;
		}
	}

	public MeshFilter CloudMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E00E;
		}
		[CompilerGenerated]
		set
		{
			_E00E = value;
		}
	}

	public MeshFilter SunMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		set
		{
			_E00F = value;
		}
	}

	public MeshFilter MoonMeshFilter
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
		[CompilerGenerated]
		set
		{
			_E010 = value;
		}
	}

	public Material SpaceMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E011;
		}
		[CompilerGenerated]
		set
		{
			_E011 = value;
		}
	}

	public Material AtmosphereMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		set
		{
			_E012 = value;
		}
	}

	public Material ClearMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E013;
		}
		[CompilerGenerated]
		set
		{
			_E013 = value;
		}
	}

	public Material CloudMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E014;
		}
		[CompilerGenerated]
		set
		{
			_E014 = value;
		}
	}

	public Material SunMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E015;
		}
		[CompilerGenerated]
		set
		{
			_E015 = value;
		}
	}

	public Material MoonMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E016;
		}
		[CompilerGenerated]
		set
		{
			_E016 = value;
		}
	}

	public Material ShadowMaterial
	{
		[CompilerGenerated]
		get
		{
			return _E017;
		}
		[CompilerGenerated]
		set
		{
			_E017 = value;
		}
	}

	public Light LightSource
	{
		[CompilerGenerated]
		get
		{
			return _E018;
		}
		[CompilerGenerated]
		set
		{
			_E018 = value;
		}
	}

	public Projector ShadowProjector
	{
		[CompilerGenerated]
		get
		{
			return _E019;
		}
		[CompilerGenerated]
		set
		{
			_E019 = value;
		}
	}

	public TOD_Sky Sky
	{
		[CompilerGenerated]
		get
		{
			return _E01A;
		}
		[CompilerGenerated]
		set
		{
			_E01A = value;
		}
	}

	public TOD_Animation Animation
	{
		[CompilerGenerated]
		get
		{
			return _E01B;
		}
		[CompilerGenerated]
		set
		{
			_E01B = value;
		}
	}

	public TOD_Time Time
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
		[CompilerGenerated]
		set
		{
			_E01C = value;
		}
	}

	public TOD_Weather Weather
	{
		[CompilerGenerated]
		get
		{
			return _E01D;
		}
		[CompilerGenerated]
		set
		{
			_E01D = value;
		}
	}

	public TOD_Camera Camera
	{
		[CompilerGenerated]
		get
		{
			return _E01E;
		}
		[CompilerGenerated]
		set
		{
			_E01E = value;
		}
	}

	public TOD_Rays Rays
	{
		[CompilerGenerated]
		get
		{
			return _E01F;
		}
		[CompilerGenerated]
		set
		{
			_E01F = value;
		}
	}

	public TOD_Scattering Scattering
	{
		[CompilerGenerated]
		get
		{
			return _E020;
		}
		[CompilerGenerated]
		set
		{
			_E020 = value;
		}
	}

	public void Initialize()
	{
		DomeTransform = GetComponent<Transform>();
		Sky = GetComponent<TOD_Sky>();
		Animation = GetComponent<TOD_Animation>();
		Time = GetComponent<TOD_Time>();
		Weather = GetComponent<TOD_Weather>();
		if ((bool)Space)
		{
			SpaceTransform = Space.GetComponent<Transform>();
			SpaceRenderer = Space.GetComponent<Renderer>();
			SpaceMaterial = SpaceRenderer.sharedMaterial;
			SpaceMeshFilter = Space.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError(_ED3E._E000(17214));
		}
		if ((bool)Atmosphere)
		{
			AtmosphereRenderer = Atmosphere.GetComponent<Renderer>();
			AtmosphereMaterial = AtmosphereRenderer.sharedMaterial;
			AtmosphereMeshFilter = Atmosphere.GetComponent<MeshFilter>();
			if (AtmosphereMeshFilter != null && AtmosphereMeshFilter.sharedMesh != null)
			{
				AtmosphereMeshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100000f);
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(17191));
		}
		if ((bool)Clear)
		{
			ClearRenderer = Clear.GetComponent<Renderer>();
			ClearMaterial = ClearRenderer.sharedMaterial;
			ClearMeshFilter = Clear.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError(_ED3E._E000(17221));
		}
		if ((bool)Clouds)
		{
			CloudRenderer = Clouds.GetComponent<Renderer>();
			CloudMaterial = CloudRenderer.sharedMaterial;
			CloudMeshFilter = Clouds.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError(_ED3E._E000(17262));
		}
		if ((bool)Projector)
		{
			ShadowProjector = Projector.GetComponent<Projector>();
			ShadowMaterial = ShadowProjector.material;
		}
		else
		{
			Debug.LogError(_ED3E._E000(17296));
		}
		if ((bool)Light)
		{
			LightTransform = Light.GetComponent<Transform>();
			LightSource = Light.GetComponent<Light>();
		}
		else
		{
			Debug.LogError(_ED3E._E000(17333));
		}
		if ((bool)Sun)
		{
			SunTransform = Sun.GetComponent<Transform>();
			SunRenderer = Sun.GetComponent<Renderer>();
			SunMaterial = SunRenderer.sharedMaterial;
			SunMeshFilter = Sun.GetComponent<MeshFilter>();
		}
		else
		{
			Debug.LogError(_ED3E._E000(17374));
		}
		if ((bool)Moon)
		{
			MoonTransform = Moon.GetComponent<Transform>();
			MoonRenderer = Moon.GetComponent<Renderer>();
			MoonMaterial = MoonRenderer.sharedMaterial;
			MoonMeshFilter = Moon.GetComponent<MeshFilter>();
			if (Application.isPlaying)
			{
				MoonMeshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(17349));
		}
		if (!Billboards)
		{
			Debug.LogError(_ED3E._E000(17389));
		}
	}
}
