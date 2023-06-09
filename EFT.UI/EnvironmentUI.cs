using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bsg.GameSettings;
using Comfort.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFT.UI;

public sealed class EnvironmentUI : MonoBehaviourSingleton<EnvironmentUI>
{
	public enum EEnvironmentUIType
	{
		RandomEnvironmentUiType,
		FactoryEnvironmentUiType,
		WoodEnvironmentUiType,
		LaboratoryEnvironmentUiType
	}

	[Serializable]
	private struct EnvironmentData
	{
		public EEnvironmentUIType Type;

		public string SceneName;
	}

	private static readonly List<GameObject> m__E000 = new List<GameObject>(4);

	[SerializeField]
	private Camera _alignmentCamera;

	[SerializeField]
	private GameObject _commonContainer;

	[SerializeField]
	private EnvironmentData[] _environments;

	[SerializeField]
	private EnvironmentShading _environmentShading;

	private EnvironmentUIRoot m__E001;

	private EEnvironmentUIType m__E002;

	private bool _E003 = true;

	private readonly _E3A4 _E004 = new _E3A4();

	private bool _E005;

	[CompilerGenerated]
	private EEventType[] _E006 = Array.Empty<EEventType>();

	public EEventType[] Events
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

	public bool IsActive
	{
		get
		{
			if (this.m__E001 != null)
			{
				return this.m__E001.gameObject.activeSelf;
			}
			return false;
		}
	}

	public override void Awake()
	{
		base.Awake();
		GameSetting<EEnvironmentUIType> environmentUiType = Singleton<_E7DE>.Instance.Game.Settings.EnvironmentUiType;
		_E004.BindState(environmentUiType, delegate(EEnvironmentUIType x)
		{
			SetEnvironmentAsync(x).HandleExceptions();
		});
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_E004.Dispose();
	}

	public async Task RefreshEnvironmentAsync()
	{
		await SetEnvironmentAsync(Singleton<_E7DE>.Instance.Game.Settings.EnvironmentUiType);
		ShowCameraContainer(value: true);
		ShowEnvironment(value: true);
	}

	public async Task SetEnvironmentAsync(EEnvironmentUIType environmentUiType)
	{
		if (environmentUiType == EEnvironmentUIType.RandomEnvironmentUiType)
		{
			environmentUiType = _E001();
		}
		if (environmentUiType == this.m__E002)
		{
			return;
		}
		EnvironmentData? environmentData = null;
		EnvironmentData[] environments = _environments;
		for (int i = 0; i < environments.Length; i++)
		{
			EnvironmentData value = environments[i];
			if (value.Type == environmentUiType)
			{
				environmentData = value;
			}
		}
		environmentData = environmentData ?? _environments.First();
		this.m__E002 = environmentData.Value.Type;
		await _E000(environmentData.Value.SceneName);
	}

	private async Task _E000(string sceneName)
	{
		await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).Await();
		Scene sceneByName = SceneManager.GetSceneByName(sceneName);
		sceneByName.GetRootGameObjects(EnvironmentUI.m__E000);
		if (EnvironmentUI.m__E000.Count < 1)
		{
			Debug.LogError(_ED3E._E000(253806) + sceneName + _ED3E._E000(253797));
			SceneManager.UnloadSceneAsync(sceneByName);
			return;
		}
		EnvironmentUIRoot component = EnvironmentUI.m__E000.First().GetComponent<EnvironmentUIRoot>();
		EnvironmentUI.m__E000.Clear();
		if (component == null)
		{
			Debug.LogError(_ED3E._E000(253806) + sceneName + _ED3E._E000(253832));
			SceneManager.UnloadSceneAsync(sceneByName);
			return;
		}
		EnvironmentUIRoot environmentUIRoot = this.m__E001;
		this.m__E001 = component;
		this.m__E001.gameObject.name = sceneName;
		this.m__E001.transform.SetParent(base.transform);
		ShowEnvironment(_E003);
		this.m__E001.Init(_alignmentCamera, (IReadOnlyCollection<EEventType>)(object)Events, _E005);
		_environmentShading.SetDefaultShading(this.m__E001.Shading);
		if (environmentUIRoot != null)
		{
			UnityEngine.Object.Destroy(environmentUIRoot.gameObject);
		}
		SceneManager.UnloadSceneAsync(sceneByName);
	}

	public void ResetRotation()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.ResetRotation();
		}
	}

	public bool SetAsMain(bool isMain)
	{
		if (!IsActive || _E005 == isMain)
		{
			return false;
		}
		_E005 = isMain;
		this.m__E001.SetMain(_E005);
		return true;
	}

	public void ShowEnvironment(bool value)
	{
		_E003 = value;
		if (value)
		{
			_commonContainer.SmartEnableWithoutHierarchy();
		}
		else
		{
			_commonContainer.SmartDisableWithoutHierarchy();
		}
		if (!(this.m__E001 == null))
		{
			if (value)
			{
				this.m__E001.gameObject.SmartEnableWithoutHierarchy();
			}
			else
			{
				this.m__E001.gameObject.SmartDisableWithoutHierarchy();
			}
		}
	}

	public void ChangeShading(EShadingType shadingType)
	{
		_environmentShading.SetShading(shadingType);
	}

	public void ShowCameraContainer(bool value)
	{
		if (!(this.m__E001 == null))
		{
			this.m__E001.SetCameraActive(value);
			if (!_E7A3.InRaid && _E8A8.Instance.Camera != null)
			{
				_E8A8.Instance.Camera.gameObject.SetActive(!value);
			}
		}
	}

	public void EnableOverlay(bool enable)
	{
		_environmentShading.SetShadingVisibility(enable, 0.4f);
	}

	public void Rotate()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.RandomRotate();
		}
	}

	private EEnvironmentUIType _E001()
	{
		int num = 3;
		int count = _E3A5<EEnvironmentUIType>.Count;
		EEnvironmentUIType eEnvironmentUIType;
		do
		{
			eEnvironmentUIType = _E3A5<EEnvironmentUIType>.Values[UnityEngine.Random.Range(1, count)];
			num--;
		}
		while (eEnvironmentUIType == this.m__E002 && num > 0);
		return eEnvironmentUIType;
	}

	[CompilerGenerated]
	private void _E002(EEnvironmentUIType x)
	{
		SetEnvironmentAsync(x).HandleExceptions();
	}
}
