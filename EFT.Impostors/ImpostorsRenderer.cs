using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace EFT.Impostors;

[_E2E2(20002)]
[ExecuteInEditMode]
public sealed class ImpostorsRenderer : MonoBehaviourSingleton<ImpostorsRenderer>
{
	private Action m__E000;

	[HideInInspector]
	[SerializeField]
	private AmplifyImpostorsArray _impostorsArray;

	[SerializeField]
	[HideInInspector]
	private EImpostorsShadowMode _shadowMode;

	[SerializeField]
	private int MaxImpostorsCount;

	[SerializeField]
	private bool _drawGizmos;

	private readonly Dictionary<uint, _E8E3> m__E001 = new Dictionary<uint, _E8E3>();

	private int m__E002;

	private bool m__E003;

	private bool m__E004;

	private _E8E2 m__E005;

	public AmplifyImpostorsArray ImpostorsArray
	{
		get
		{
			return _impostorsArray;
		}
		set
		{
			if (_impostorsArray != null)
			{
				_impostorsArray.RegisterEvent -= _E000;
				_impostorsArray.UnregisterEvent -= _E001;
			}
			_E00F();
			_impostorsArray = value;
			if (!(_impostorsArray == null) && base.enabled)
			{
				_impostorsArray.RegisterEvent += _E000;
				_impostorsArray.UnregisterEvent += _E001;
				Refresh();
			}
		}
	}

	public EImpostorsShadowMode ShadowMode
	{
		get
		{
			return _shadowMode;
		}
		set
		{
			_shadowMode = value;
			_E002();
		}
	}

	public int ImpostorsToRenderCount => this.m__E005?.ImpostorsToRenderCount ?? 0;

	private void _E000()
	{
		Refresh();
	}

	private void _E001()
	{
		_E00F();
	}

	public override void Awake()
	{
		base.Awake();
	}

	private void OnEnable()
	{
		if (!(_impostorsArray == null))
		{
			_E003();
			_E00B();
		}
	}

	private void OnDisable()
	{
		if (!(_impostorsArray == null))
		{
			_E004();
			_E00F();
			Interlocked.Exchange(ref this.m__E005, null)?.Dispose();
		}
	}

	private void LateUpdate()
	{
		if (this.m__E003)
		{
			this.m__E003 = false;
			_E002();
		}
	}

	private void _E002()
	{
		if (!(_impostorsArray == null) && this.m__E001.Count > 0)
		{
			List<_E8E3> groups = this.m__E001.Values.OrderBy((_E8E3 group) => group._E002).ToList();
			_E7DE._E000<_E7EB, _E7E9> obj = Singleton<_E7DE>.Instance?.Graphics;
			int firstMipLevel = ((obj != null) ? (2 - obj.Settings.TextureQuality.Value) : 0);
			this.m__E005.Update(_impostorsArray, groups, firstMipLevel, _shadowMode);
		}
	}

	private void _E003()
	{
		if (this.m__E005 == null)
		{
			this.m__E005 = new _E8E2();
		}
		_impostorsArray.RegisterEvent += _E000;
		_impostorsArray.UnregisterEvent += _E001;
		SceneManager.activeSceneChanged += _E006;
		SceneManager.sceneLoaded += _E005;
		AmplifyImpostorsArrayElement.AddedEvent += _E009;
		AmplifyImpostorsArrayElement.RemovedEvent += _E00A;
		AmplifyImpostorsArrayElement.EnableChangedEvent += _E007;
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E00D));
		this.m__E000 = (Singleton<_E7DE>.Instance?.Graphics)?.Settings.TextureQuality.Subscribe(_E008);
	}

	private void _E004()
	{
		_impostorsArray.RegisterEvent -= _E000;
		_impostorsArray.UnregisterEvent -= _E001;
		SceneManager.activeSceneChanged -= _E006;
		SceneManager.sceneLoaded -= _E005;
		AmplifyImpostorsArrayElement.AddedEvent -= _E009;
		AmplifyImpostorsArrayElement.RemovedEvent -= _E00A;
		AmplifyImpostorsArrayElement.EnableChangedEvent -= _E007;
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E00D));
		Interlocked.Exchange(ref this.m__E000, null)?.Invoke();
	}

	private void _E005(Scene scene, LoadSceneMode sceneMode)
	{
		if (this.m__E004)
		{
			this.m__E004 = false;
			_E002();
		}
	}

	private void _E006(Scene scene, Scene next)
	{
		if (!(next != base.gameObject.scene))
		{
			Refresh();
		}
	}

	private void _E007(AmplifyImpostorsArrayElement impostor, bool enable)
	{
		this.m__E005?.EnablesChanged();
	}

	private void _E008(int textureQuality)
	{
		_E002();
	}

	private void _E009(AmplifyImpostorsArrayElement impostor)
	{
		if (MaxImpostorsCount > 0 && this.m__E002 >= MaxImpostorsCount)
		{
			Debug.LogError(string.Format(_ED3E._E000(175012), this.m__E002, MaxImpostorsCount));
			return;
		}
		if (impostor.TryGetComponent<LODGroup>(out var component) || impostor.transform.parent.TryGetComponent<LODGroup>(out component))
		{
			component.RecalculateBounds();
		}
		uint typeId = impostor.TypeId;
		if (!this.m__E001.TryGetValue(typeId, out var value))
		{
			value = new _E8E3(typeId);
			this.m__E001.Add(typeId, value);
		}
		if (!value._E003.Contains(impostor))
		{
			value._E003.Add(impostor);
			this.m__E002++;
			if (impostor.gameObject.scene.isLoaded)
			{
				this.m__E003 = true;
			}
			else
			{
				this.m__E004 = true;
			}
		}
	}

	private void _E00A(AmplifyImpostorsArrayElement impostor)
	{
		uint typeId = impostor.TypeId;
		if (this.m__E001.TryGetValue(typeId, out var value) && value._E003.Remove(impostor))
		{
			this.m__E002--;
			if (value._E003.Count == 0)
			{
				this.m__E001.Remove(typeId);
			}
			this.m__E003 = true;
		}
	}

	private void _E00B()
	{
		List<AmplifyImpostorsArrayElement> list = _E3AA.FindObjectsOfTypeAll<AmplifyImpostorsArrayElement>();
		for (int i = 0; i < list.Count; i++)
		{
			AmplifyImpostorsArrayElement impostor = list[i];
			_E009(impostor);
		}
		this.m__E003 = false;
		_E002();
	}

	internal void _E00C(Camera currentCamera, CommandBuffer renderObjectsCmdBuf, uint impostorsPerFrame, out int impostorsRest)
	{
		impostorsRest = 0;
		if (this.m__E001.Count != 0 && !(currentCamera == null) && (!Application.isPlaying || _E00E(currentCamera)))
		{
			this.m__E005._E006(currentCamera, renderObjectsCmdBuf, impostorsPerFrame, out impostorsRest);
		}
	}

	private void _E00D(Camera currentCamera)
	{
		if (this.m__E001.Count != 0 && !(currentCamera == null) && (!Application.isPlaying || _E00E(currentCamera)))
		{
			this.m__E005.OnPreCull(currentCamera);
		}
	}

	private static bool _E00E(Component camera)
	{
		bool flag = camera.CompareTag(_ED3E._E000(42129));
		return camera.CompareTag(_ED3E._E000(42407)) || flag;
	}

	private void _E00F()
	{
		this.m__E001.Clear();
		this.m__E005?._E000();
		this.m__E002 = 0;
		this.m__E003 = false;
	}

	public void Refresh()
	{
		_E004();
		_E00F();
		_E003();
		_E00B();
		this.m__E003 = true;
		Singleton<ImpostorsRenderer>.Instance = this;
	}
}
