using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cinemachine;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutArea : MonoBehaviour, _E812, _E633
{
	[SerializeField]
	private HideoutAreaTrigger _trigger;

	[SerializeField]
	private Color _highlightColor;

	[SerializeField]
	private AreaTemplate _areaTemplate;

	[SerializeField]
	[Space]
	private HideoutAreaDefaultLevel _areaDefaultLevel;

	[SerializeField]
	private HideoutAreaLevel[] _areaLevels;

	[SerializeField]
	[Space]
	private CinemachineVirtualCamera _areaCamera;

	[SerializeField]
	private CinemachineVirtualCamera _specialActionCamera;

	[SerializeField]
	private Transform _areaIconPoint;

	private AreaData m__E000;

	private HideoutAreaLevel m__E001;

	private ELightingLevel m__E002;

	private readonly Dictionary<string, KeyValuePair<Item, GameObject>> m__E003 = new Dictionary<string, KeyValuePair<Item, GameObject>>();

	private _EC76 m__E004 = new _EC76();

	private VideocardInstaller m__E005;

	[CompilerGenerated]
	private AreaLevelAudio m__E006;

	public AreaData Data
	{
		get
		{
			return this.m__E000;
		}
		set
		{
			if (this.m__E000 != value)
			{
				this.m__E000 = value;
				_E000();
			}
		}
	}

	public HideoutArea Area => this;

	public AreaTemplate AreaTemplate => _areaTemplate;

	public HideoutAreaDefaultLevel AreaDefaultLevel => _areaDefaultLevel;

	public HideoutAreaLevel[] AreaLevels => _areaLevels;

	public Transform HighlightTransform
	{
		get
		{
			return this.m__E001.HighlightTransform;
		}
		set
		{
			this.m__E001.HighlightTransform = value;
		}
	}

	public Color HighlightColor => _highlightColor;

	public CinemachineVirtualCamera AreaCamera => _areaCamera;

	public CinemachineVirtualCamera SpecialActionCamera => _specialActionCamera;

	public Transform AreaIconPoint => _areaIconPoint;

	public AreaLevelAudio AudioAmbiance
	{
		[CompilerGenerated]
		get
		{
			return this.m__E006;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E006 = value;
		}
	}

	private async void _E000()
	{
		this.m__E004.Dispose();
		Data.InitArea(this);
		_areaDefaultLevel?.Enable();
		for (int i = 0; i < _areaLevels.Length; i++)
		{
			HideoutAreaLevel hideoutAreaLevel = _areaLevels[i];
			hideoutAreaLevel.Disable();
			if (!Data.Template.Stages.ContainsKey(i))
			{
				Debug.LogErrorFormat(_ED3E._E000(171129), i, Data.Template.Name);
			}
			else
			{
				hideoutAreaLevel.Init(Data.Template.Stages[i]);
			}
		}
		if (!Data.Enabled)
		{
			return;
		}
		if (_trigger != null)
		{
			_trigger.Init(this);
			_trigger.gameObject.SetActive(value: true);
		}
		this.m__E004.AddDisposable(Data.LevelUpdated.Bind(_E004));
		this.m__E004.AddDisposable(Data.LevelUpdated.Subscribe(_E006));
		this.m__E004.AddDisposable(Data.LightStatusChanged.Subscribe(_E003));
		this.m__E004.AddDisposable(Data.StatusUpdated.Bind(_E002));
		this.m__E004.AddDisposable(Data.VisibilityChanged.Bind(_E001, Data.IsVisible));
		this.m__E004.AddDisposable(Data.OnSelected.Subscribe(delegate(bool isSelected)
		{
			if (isSelected && this.m__E001 != null)
			{
				AudioAmbiance.Select();
			}
		}));
		if (Application.isPlaying && AreaTemplate.Type == EAreaType.BitcoinFarm)
		{
			this.m__E005 = await _E008();
		}
	}

	private void _E001(bool visible)
	{
		_areaLevels[this.m__E000.CurrentLevel].gameObject.SetActive(visible);
		if ((bool)_areaDefaultLevel)
		{
			_areaDefaultLevel.gameObject.SetActive(!visible);
		}
	}

	private void Update()
	{
		_areaTemplate.AreaBehaviour.BehaviourUpdate();
	}

	private void _E002()
	{
		Stage nextStage = Data.NextStage;
		if (nextStage != null && nextStage.ConstructionTime.Data >= float.Epsilon)
		{
			AudioAmbiance.AreaStatus = Data.Status;
		}
	}

	private void _E003(ELightStatus lightStatus)
	{
		AudioAmbiance.LightStatus = lightStatus;
	}

	private async void _E004()
	{
		if (this.m__E001 != null)
		{
			this.m__E001.Disable();
		}
		_areaDefaultLevel?.Disable();
		this.m__E001 = _areaLevels[Data.CurrentLevel];
		this.m__E001.Enable(Data.Status == EAreaStatus.NotSet);
		_E005(this.m__E001.AudioAmbiance);
		_E003(Data.LightStatus);
		_E002();
		SetLightingLevel(this.m__E002);
		if (AreaTemplate.Type == EAreaType.BitcoinFarm)
		{
			VideocardInstaller videocardInstaller = this.m__E005;
			this.m__E005 = await _E008();
			if (videocardInstaller != null && this.m__E005 != null)
			{
				this.m__E005.GetFromOther(videocardInstaller);
			}
		}
		List<GameObject> list = new List<GameObject>();
		List<Task> list2 = new List<Task>();
		foreach (var (_, keyValuePair3) in this.m__E003)
		{
			list.Add(keyValuePair3.Value);
			list2.Add(_E007(keyValuePair3.Value, keyValuePair3.Key));
		}
		this.m__E001.SetItemsOnPlace(list);
		await Task.WhenAll(list2);
	}

	private void _E005(AreaLevelAudio source)
	{
		if (!(AudioAmbiance == source))
		{
			if (AudioAmbiance != null)
			{
				AudioAmbiance.Pause();
			}
			source.Init(this.m__E001.Sounds, Data.LightStatus, Data.Status);
			AudioAmbiance = source;
			AudioAmbiance.Resume();
		}
	}

	private void _E006()
	{
		AudioAmbiance.LevelChanged();
	}

	public void SetLightingLevel(ELightingLevel level)
	{
		this.m__E002 = level;
		if (this.m__E001 != null)
		{
			this.m__E001.SetLightLevel(this.m__E002);
		}
	}

	public void EnableIlluminationLevels(IReadOnlyCollection<ELightingLevel> availableLevels)
	{
		HideoutAreaLevel[] areaLevels = _areaLevels;
		for (int i = 0; i < areaLevels.Length; i++)
		{
			areaLevels[i].SetAvailableLightingLevels(availableLevels);
		}
	}

	public async Task SpawnProducedItems(Item[] items)
	{
		_E760 instance = Singleton<_E760>.Instance;
		List<Task> list = new List<Task>();
		List<GameObject> list2 = new List<GameObject>();
		foreach (Item item in items)
		{
			if (item != null && !this.m__E003.ContainsKey(item.TemplateId))
			{
				GameObject gameObject = await instance.CreateCleanLootPrefabAsync(item);
				list.Add(_E007(gameObject, item));
				this.m__E003[item.TemplateId] = new KeyValuePair<Item, GameObject>(item, gameObject);
				list2.Add(gameObject);
			}
		}
		if (!this.m__E001.CanSetProducedItemsOnPlace)
		{
			foreach (GameObject item2 in list2)
			{
				item2.SetActive(value: false);
			}
			return;
		}
		this.m__E001.SetItemsOnPlace(list2);
		await Task.WhenAll(list);
	}

	private async Task _E007(GameObject spawnedObject, Item item)
	{
		spawnedObject.layer = LayerMask.NameToLayer(_ED3E._E000(60734));
		spawnedObject.SetActive(value: true);
		Collider[] componentsInChildren = spawnedObject.GetComponentsInChildren<Collider>(includeInactive: true);
		List<GameObject> list = new List<GameObject>();
		Collider[] array = componentsInChildren;
		foreach (Collider obj in array)
		{
			obj.enabled = true;
			GameObject gameObject = obj.gameObject;
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(value: true);
				list.Add(gameObject);
			}
		}
		Rigidbody orAddComponent = spawnedObject.GetOrAddComponent<Rigidbody>();
		orAddComponent.mass = item.GetSingleItemTotalWeight();
		orAddComponent.isKinematic = false;
		_E320._E002.SupportRigidbody(orAddComponent);
		Bounds totalBounds = componentsInChildren.GetTotalBounds();
		Debug.LogWarning(totalBounds);
		BoxCollider orAddComponent2 = spawnedObject.GetOrAddComponent<BoxCollider>();
		orAddComponent2.enabled = true;
		orAddComponent2.center = totalBounds.center;
		orAddComponent2.size = Vector3.Max(totalBounds.size, Vector3.Min(totalBounds.size * 1.5f, Vector3.one * 0.05f));
		array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		foreach (GameObject item2 in list)
		{
			item2.SetActive(value: false);
		}
		await Task.Delay(500);
		orAddComponent.isKinematic = true;
		orAddComponent2.enabled = false;
		_E320._E002.UnsupportRigidbody(orAddComponent);
	}

	private async Task<VideocardInstaller> _E008()
	{
		VideocardInstaller videocardInstaller = ((this.m__E001 != null) ? this.m__E001.GetComponent<VideocardInstaller>() : GetComponentInChildren<VideocardInstaller>(includeInactive: false));
		if (VideocardInstaller.IsInited || videocardInstaller == null)
		{
			return videocardInstaller;
		}
		if (!(Singleton<_E815>.Instance.ProductionController.GetProducer(Data) is _E824 obj) || string.IsNullOrEmpty(obj.SlotItemTemplate))
		{
			return videocardInstaller;
		}
		await VideocardInstaller.Init(obj.SlotItemTemplate);
		videocardInstaller.AttachCard(obj.InstalledSuppliesCount).HandleExceptions();
		return videocardInstaller;
	}

	public void RemoveProducedItems(IEnumerable<Item> items)
	{
		foreach (Item item in items)
		{
			if (item != null && this.m__E003.TryGetValue(item.TemplateId, out var value))
			{
				Object.Destroy(value.Value);
				this.m__E003.Remove(item.TemplateId);
			}
		}
		this.m__E001.SetItemsOnPlace(this.m__E003.Values.Select((KeyValuePair<Item, GameObject> go) => go.Value));
	}

	public void AttachVideocard(int count)
	{
		if (this.m__E005 != null)
		{
			this.m__E005.AttachCard(count).HandleExceptions();
		}
	}

	public void DetachVideocard()
	{
		if (this.m__E005 != null)
		{
			this.m__E005.DetachCard();
		}
	}

	private void OnDestroy()
	{
		this.m__E004.Dispose();
		if (AreaTemplate.Type == EAreaType.BitcoinFarm)
		{
			VideocardInstaller.ReleaseBundles();
		}
	}

	[CompilerGenerated]
	private void _E009(bool isSelected)
	{
		if (isSelected && this.m__E001 != null)
		{
			AudioAmbiance.Select();
		}
	}
}
