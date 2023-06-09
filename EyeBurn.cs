using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EyeBurn : MonoBehaviour
{
	private class _E000
	{
		public Vector3 ScreenPosition;

		public Material Material;

		public float Rotation;

		public Vector3 Scale;

		public float StartTime;

		public float MaxTime;

		public _E000(Vector3 screenPosition, Material material, float rotation, Vector3 scale, float startTime, float maxTime)
		{
			ScreenPosition = screenPosition;
			Material = material;
			Rotation = rotation;
			Scale = scale;
			StartTime = startTime;
			MaxTime = maxTime;
		}
	}

	public Material EyeBurnScreenEffect;

	[SerializeField]
	private Material[] _eyeBurnEffectMaterial;

	[SerializeField]
	private Material[] _eyeBurnTrailMaterial;

	[SerializeField]
	private Vector2 _scaleMinMax;

	[SerializeField]
	private float _eyeBurnMaxTime = 100f;

	[SerializeField]
	private float TrailSpotEvery = 0.4f;

	[SerializeField]
	private float TrailSpotShift = 0.15f;

	[SerializeField]
	private AnimationCurve trailDecreaseCurve;

	[SerializeField]
	private int MaxTrailLength = 8;

	[SerializeField]
	private AnimationCurve _scaleCoefCurve;

	[CompilerGenerated]
	private bool m__E000;

	public bool IsPaused;

	private const string m__E001 = "_Rotation";

	private const string m__E002 = "_Coef";

	private RenderTexture m__E003;

	private Coroutine m__E004;

	private List<_E000> m__E005 = new List<_E000>();

	private int _E006;

	private Vector3 _E007;

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(43541));

	public bool EyesBurn
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	private void Awake()
	{
		this.m__E005 = new List<_E000>();
	}

	private void OnEnable()
	{
		Awake();
	}

	private void OnDisable()
	{
		this.m__E005.Clear();
		ClearEyeBurnRT();
	}

	private void Update()
	{
		if (this.m__E005.Count != 0 && !IsPaused)
		{
			RenderTexture active = RenderTexture.active;
			Graphics.SetRenderTarget(_E000());
			GL.Clear(clearDepth: true, clearColor: true, Color.clear);
			EyeBurnScreenEffect.SetTexture(_E008, _E000());
			for (int i = 0; i < this.m__E005.Count; i++)
			{
				float num = Mathf.Clamp01(1f - (Time.timeSinceLevelLoad - this.m__E005[i].StartTime) / this.m__E005[i].MaxTime);
				float scaleCoef = _scaleCoefCurve.Evaluate(num);
				Draw(this.m__E005[i].ScreenPosition, this.m__E005[i].Scale, this.m__E005[i].Rotation, this.m__E005[i].Material, num, scaleCoef);
			}
			RenderTexture.active = active;
		}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, EyeBurnScreenEffect);
	}

	public void Burn(Vector3 position, float stregnth, Vector2 deltaRotation)
	{
		float num = Mathf.Lerp(_scaleMinMax.x, _scaleMinMax.y, stregnth);
		Vector3 scale = new Vector3(num, num / (float)Screen.height * (float)Screen.width);
		float eyeBurnTime = _eyeBurnMaxTime * stregnth;
		Vector3 screenPosition = _E8A8.Instance.Camera.WorldToViewportPoint(position);
		if (_E004(screenPosition))
		{
			_E002(screenPosition, scale, eyeBurnTime);
			_E003(new Vector2(deltaRotation.x, 0f - deltaRotation.y), scale, eyeBurnTime);
			if (this.m__E004 != null)
			{
				StopCoroutine(this.m__E004);
			}
			this.m__E004 = StartCoroutine(_E001(_eyeBurnMaxTime));
		}
	}

	public void Draw(Vector3 position, Vector3 scale, float rotation, Material material, float coef, float scaleCoef)
	{
		material.SetPass(0);
		material.SetFloat(_ED3E._E000(43507), rotation);
		material.SetFloat(_ED3E._E000(43493), coef);
		float x = position.x - scale.x * scaleCoef;
		float x2 = position.x + scale.x * scaleCoef;
		float y = 0f - position.y - scale.y * scaleCoef;
		float y2 = 0f - position.y + scale.y * scaleCoef;
		GL.Begin(7);
		material.SetPass(0);
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(x, y, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(x2, y, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(x2, y2, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(x, y2, 0f);
		GL.End();
	}

	private RenderTexture _E000()
	{
		if (this.m__E003 != null)
		{
			return this.m__E003;
		}
		this.m__E003 = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
		{
			name = _ED3E._E000(43491),
			autoGenerateMips = false,
			useMipMap = false
		};
		return this.m__E003;
	}

	private IEnumerator _E001(float t)
	{
		float num = 0f;
		while (num <= 1f)
		{
			if (!IsPaused)
			{
				num += Time.deltaTime / t;
			}
			yield return null;
		}
		base.enabled = false;
	}

	private void _E002(Vector3 screenPosition, Vector3 scale, float eyeBurnTime)
	{
		Material material = _eyeBurnEffectMaterial[++_E006 % _eyeBurnEffectMaterial.Length];
		int num = Random.Range(0, 360);
		Vector3 vector = _E005(screenPosition);
		_E000 item = new _E000(vector, material, num, scale, Time.timeSinceLevelLoad, eyeBurnTime);
		_E007 = vector;
		this.m__E005.Add(item);
	}

	private void _E003(Vector3 direction, Vector3 scale, float eyeBurnTime)
	{
		Vector3 normalized = direction.normalized;
		int num = Mathf.Min(MaxTrailLength, (int)(direction.magnitude / TrailSpotEvery));
		if (num >= 1)
		{
			Vector3 screenPosition = _E007 + normalized * TrailSpotShift * scale.x;
			for (int i = 0; i < num; i++)
			{
				Material material = _eyeBurnTrailMaterial[Random.Range(0, _eyeBurnTrailMaterial.Length)];
				int num2 = Random.Range(0, 360);
				Vector3 scale2 = scale * trailDecreaseCurve.Evaluate(i);
				_E000 item = new _E000(screenPosition, material, num2, scale2, Time.timeSinceLevelLoad, eyeBurnTime);
				this.m__E005.Add(item);
				screenPosition += normalized * TrailSpotShift * scale2.x;
			}
		}
	}

	private bool _E004(Vector3 screenPosition)
	{
		if (screenPosition.x < 1f && screenPosition.x > 0f && screenPosition.y < 1f && screenPosition.y > 0f)
		{
			return screenPosition.z > 0f;
		}
		return false;
	}

	private Vector3 _E005(Vector3 screenPosition)
	{
		return new Vector3(screenPosition.x * 2f - 1f, screenPosition.y * 2f - 1f, screenPosition.z);
	}

	public void ClearEyeBurnRT()
	{
		RenderTexture active = RenderTexture.active;
		Graphics.SetRenderTarget(_E000());
		GL.Clear(clearDepth: true, clearColor: true, Color.clear);
		RenderTexture.active = active;
	}
}
