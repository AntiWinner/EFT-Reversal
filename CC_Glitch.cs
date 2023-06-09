using System;
using EFT.BlitDebug;
using UnityEngine;

[AddComponentMenu("Colorful/Glitch")]
[ExecuteInEditMode]
public class CC_Glitch : CC_Base
{
	public enum Mode
	{
		Interferences,
		Tearing,
		Complete
	}

	[Serializable]
	public class InterferenceSettings
	{
		public float speed = 10f;

		public float density = 8f;

		public float maxDisplacement = 2f;
	}

	[Serializable]
	public class TearingSettings
	{
		public float speed = 1f;

		[Range(0f, 1f)]
		public float intensity = 0.25f;

		[Range(0f, 0.5f)]
		public float maxDisplacement = 0.05f;

		public bool allowFlipping;

		public bool yuvColorBleeding = true;

		[Range(-2f, 2f)]
		public float yuvOffset = 0.5f;
	}

	public bool randomActivation;

	public Vector2 randomEvery = new Vector2(1f, 2f);

	public Vector2 randomDuration = new Vector2(1f, 2f);

	public Mode mode;

	public InterferenceSettings interferencesSettings;

	public TearingSettings tearingSettings;

	protected Camera m_Camera;

	protected bool m_Activated = true;

	protected float m_EveryTimer;

	protected float m_EveryTimerEnd;

	protected float m_DurationTimer;

	protected float m_DurationTimerEnd;

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(16734));

	public bool IsActive => m_Activated;

	protected virtual void OnEnable()
	{
		m_Camera = GetComponent<Camera>();
	}

	protected override void Start()
	{
		base.Start();
		m_DurationTimerEnd = UnityEngine.Random.Range(randomDuration.x, randomDuration.y);
	}

	protected virtual void Update()
	{
		if (m_Activated)
		{
			m_DurationTimer += Time.deltaTime;
			if (m_DurationTimer >= m_DurationTimerEnd)
			{
				m_DurationTimer = 0f;
				m_Activated = false;
				m_EveryTimerEnd = UnityEngine.Random.Range(randomEvery.x, randomEvery.y);
			}
		}
		else
		{
			m_EveryTimer += Time.deltaTime;
			if (m_EveryTimer >= m_EveryTimerEnd)
			{
				m_EveryTimer = 0f;
				m_Activated = true;
				m_DurationTimerEnd = UnityEngine.Random.Range(randomDuration.x, randomDuration.y);
			}
		}
	}

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!m_Activated)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (mode == Mode.Interferences)
		{
			DoInterferences(source, destination, interferencesSettings);
			return;
		}
		if (mode == Mode.Tearing)
		{
			DoTearing(source, destination, tearingSettings);
			return;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(m_Camera.pixelWidth, m_Camera.pixelHeight, 0, RenderTextureFormat.ARGB32);
		temporary.name = _ED3E._E000(16682);
		DoTearing(source, temporary, tearingSettings);
		DoInterferences(temporary, destination, interferencesSettings);
		temporary.Release();
	}

	protected virtual void DoInterferences(RenderTexture source, RenderTexture destination, InterferenceSettings settings)
	{
		base.material.SetVector(_E021, new Vector3(settings.speed, settings.density, settings.maxDisplacement));
		DebugGraphics.Blit(source, destination, base.material, 0);
	}

	protected virtual void DoTearing(RenderTexture source, RenderTexture destination, TearingSettings settings)
	{
		base.material.SetVector(_E021, new Vector4(settings.speed, settings.intensity, settings.maxDisplacement, settings.yuvOffset));
		int pass = 1;
		if (settings.allowFlipping && settings.yuvColorBleeding)
		{
			pass = 4;
		}
		else if (settings.allowFlipping)
		{
			pass = 2;
		}
		else if (settings.yuvColorBleeding)
		{
			pass = 3;
		}
		DebugGraphics.Blit(source, destination, base.material, pass);
	}
}
