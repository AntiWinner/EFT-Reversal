using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Audio.DebugTools;
using Comfort.Common;
using EFT;
using EFT.EnvironmentEffect;
using JetBrains.Annotations;
using UnityEngine;

namespace Audio.SpatialSystem;

public sealed class SpatialAudioSystem : MonoBehaviourSingleton<SpatialAudioSystem>
{
	private const float m__E000 = 4f;

	public bool UseNewPropagationSystem = true;

	public bool UseWindowsBreakSystem;

	[SerializeField]
	private AudioRoutesBakeData _routesBakeData;

	private readonly List<_E48E> m__E001 = new List<_E48E>();

	private readonly Dictionary<int, _E48E> m__E002 = new Dictionary<int, _E48E>();

	private readonly List<_E48E> m__E003 = new List<_E48E>();

	private readonly List<_E48E> m__E004 = new List<_E48E>();

	private readonly List<_E485> m__E005 = new List<_E485>();

	private readonly _E3F1<_E48E> m__E006 = new _E3F1<_E48E>(100, 5);

	private Transform m__E007;

	private bool m__E008;

	private _E47D m__E009;

	private _E47C m__E00A;

	private _E481 m__E00B;

	private float m__E00C;

	[CompilerGenerated]
	private SoundOcclusionDebugger m__E00D;

	private static float _E00E => EFTHardSettings.Instance.SoundOcclusionUpdateInterval;

	private bool _E00F
	{
		get
		{
			if (this.m__E00C > SpatialAudioSystem._E00E)
			{
				return this.m__E007 != null;
			}
			return false;
		}
	}

	public SpatialAudioRoom ListenerCurrentRoom => this.m__E009?.ListenerCurrentRoom;

	public SoundOcclusionDebugger DebugPanel
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00D;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00D = value;
		}
	}

	public float PropagationDepth
	{
		get
		{
			if (_routesBakeData == null)
			{
				return 4f;
			}
			return _routesBakeData.MaxPropagationDepth;
		}
	}

	public void Initialize()
	{
		_E000();
		this.m__E00B = _E001();
		this.m__E007 = Singleton<AudioListenerConsistencyManager>.Instance.transform;
		DebugPanel = base.transform.gameObject.AddComponent<SoundOcclusionDebugger>();
		if (UseNewPropagationSystem)
		{
			this.m__E009 = new _E47D(_routesBakeData);
			this.m__E00A = new _E47C();
		}
		this.m__E008 = true;
	}

	private static void _E000(bool newSystemEnabled = true)
	{
		MonoBehaviourSingleton<BetterAudio>.Instance.Master.SetFloat(_ED3E._E000(73456), newSystemEnabled ? 22000f : 900f);
	}

	private static _E481 _E001()
	{
		return new _E483(new _E480());
	}

	public void ProcessSourceOcclusion([NotNull] Player player, [NotNull] BetterSource source)
	{
		int id = player.Id;
		if (this.m__E002.TryGetValue(id, out var value))
		{
			value.AddSource(source);
			return;
		}
		value = new _E48E(player.PlayerBones.WeaponRoot.Original, EOcclusionTest.Continuous);
		value.AddSource(source);
		_E485 item = new _E485(value);
		this.m__E005.Add(item);
		this.m__E002.Add(id, value);
	}

	public void ProcessSourceOcclusion([NotNull] BetterSource source, EOcclusionTest test)
	{
		switch (test)
		{
		case EOcclusionTest.Fast:
			_E002(source);
			break;
		case EOcclusionTest.Continuous:
			_E003(source);
			break;
		}
	}

	private void _E002([NotNull] BetterSource source)
	{
		_E48E @object = this.m__E006.GetObject();
		@object.OcclusionTest = EOcclusionTest.Fast;
		@object.AddSource(source);
		_E00B(@object);
		this.m__E00B?.UpdateOcclusion(@object, this.m__E007, updateImmediately: true);
		@object.Clear();
		this.m__E006.PutObject(@object);
	}

	private void _E003([NotNull] BetterSource source)
	{
		_E48E obj = new _E48E(source.transform, EOcclusionTest.Continuous);
		obj.AddSource(source);
		_E485 obj2 = new _E485(obj);
		this.m__E005.Add(obj2);
		obj2.UpdateSourcesOcclusion(updateImmediately: true);
		this.m__E001.Add(obj);
	}

	public void AddPlayerCurrentRoom(SpatialAudioRoom room, Player player)
	{
		this.m__E009?.AddPlayerCurrentRoom(room, player);
	}

	public void RemovePlayerCurrentRoom(SpatialAudioRoom room, Player player)
	{
		this.m__E009?.RemovePlayerCurrentRoom(room, player);
	}

	public bool IsSourceInListenerRoom(_E48E sourceContainer)
	{
		return this.m__E004.Contains(sourceContainer);
	}

	public bool IsSourceInListenerRoom(Vector3 sourcePosition)
	{
		if ((object)ListenerCurrentRoom == null)
		{
			return false;
		}
		BoxCollider[] colliders = ListenerCurrentRoom.Colliders;
		for (int i = 0; i < colliders.Length; i++)
		{
			if (_E3ED.IsPositionInsideCollider(colliders[i], sourcePosition))
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		if (this.m__E008)
		{
			this.m__E00C += Time.deltaTime;
			if (_E00F)
			{
				_E004();
			}
		}
	}

	private void LateUpdate()
	{
		if (!this.m__E008)
		{
			return;
		}
		if ((object)this.m__E007 == null)
		{
			this.m__E007 = _E00D();
		}
		else if (_E00F)
		{
			_E005();
			this.m__E00C = 0f;
			_E008();
			if ((object)ListenerCurrentRoom == null && EnvironmentManager.Instance.Environment == EnvironmentType.Outdoor)
			{
				_E006();
				return;
			}
			_E00A();
			_E00C();
			_E006();
		}
	}

	private void _E004()
	{
		if (this.m__E005.Count == 0)
		{
			return;
		}
		_E007();
		foreach (_E485 item in this.m__E005)
		{
			item.ManualUpdate();
		}
	}

	private void _E005()
	{
		if (this.m__E005.Count == 0)
		{
			return;
		}
		_E007();
		foreach (_E485 item in this.m__E005)
		{
			item.ManualLateUpdate();
		}
	}

	private void _E006()
	{
		if ((object)this.m__E007 == null || this.m__E005.Count == 0)
		{
			return;
		}
		_E007();
		foreach (_E485 item in this.m__E005)
		{
			item.UpdateSourcesOcclusion();
		}
	}

	private void _E007()
	{
		for (int i = 0; i < this.m__E005.Count; i++)
		{
			_E485 obj = this.m__E005[i];
			if (!obj.IsValid)
			{
				obj.Clear();
				this.m__E005.Remove(obj);
			}
		}
		if (!(this.m__E007 != null))
		{
			_E00E();
		}
	}

	private void _E008()
	{
		for (int i = 0; i < this.m__E001.Count; i++)
		{
			_E48E obj = this.m__E001[i];
			if (!obj.IsValid)
			{
				_E009(obj);
			}
		}
	}

	private void _E009(_E48E sourceContainer)
	{
		sourceContainer.Clear();
		this.m__E006.PutObject(sourceContainer);
		this.m__E001.Remove(sourceContainer);
	}

	private void _E00A()
	{
		if (!UseNewPropagationSystem || (object)this.m__E007 == null)
		{
			return;
		}
		this.m__E003.Clear();
		foreach (KeyValuePair<int, _E48E> item in this.m__E002)
		{
			_E39D.Deconstruct(item, out var key, out var value);
			int playerId = key;
			_E48E obj = value;
			SpatialAudioRoom otherPlayerCurrentRoom = this.m__E009.GetOtherPlayerCurrentRoom(playerId);
			obj.CurrentAudioRoom = otherPlayerCurrentRoom;
			this.m__E003.Add(obj);
		}
		foreach (_E48E item2 in this.m__E001)
		{
			_E00B(item2);
		}
	}

	private void _E00B(_E48E sourceContainer)
	{
		bool flag = _E3ED.IsPositionIdentical(sourceContainer.CurrentPosition, sourceContainer.GetCachedPosition());
		if ((object)sourceContainer.CurrentAudioRoom == null)
		{
			if (!flag)
			{
				sourceContainer.UpdateCachedPosition();
			}
			SpatialAudioRoom spatialAudioRoom = this.m__E009?.FindInitialRoom(sourceContainer.CurrentPosition);
			if (spatialAudioRoom != null)
			{
				sourceContainer.CurrentAudioRoom = spatialAudioRoom;
				this.m__E003.Add(sourceContainer);
			}
		}
		else if (!flag)
		{
			sourceContainer.UpdateCachedPosition();
			SpatialAudioRoom spatialAudioRoom2 = this.m__E009?.FindActualCurrentRoom(sourceContainer.CurrentAudioRoom, sourceContainer.CurrentPosition);
			if (spatialAudioRoom2 != null)
			{
				sourceContainer.CurrentAudioRoom = spatialAudioRoom2;
				this.m__E003.Add(sourceContainer);
			}
			else
			{
				sourceContainer.CurrentAudioRoom = null;
			}
		}
		else
		{
			this.m__E003.Add(sourceContainer);
		}
	}

	private void _E00C()
	{
		if (!UseNewPropagationSystem)
		{
			return;
		}
		this.m__E004.Clear();
		foreach (_E48E item in this.m__E003)
		{
			if ((object)ListenerCurrentRoom == null)
			{
				break;
			}
			if ((object)item.CurrentAudioRoom != null && item.CurrentAudioRoom.ID == ListenerCurrentRoom.ID)
			{
				this.m__E004.Add(item);
			}
		}
	}

	private Transform _E00D()
	{
		if (Singleton<AudioListenerConsistencyManager>.Instantiated)
		{
			return Singleton<AudioListenerConsistencyManager>.Instance.transform;
		}
		return null;
	}

	public bool TryGetMatchingRoomPair(SpatialAudioRoom emitterRoom, out RoomPair roomPair)
	{
		if (this.m__E009 == null)
		{
			roomPair = null;
			return false;
		}
		return this.m__E009.TryGetMatchingRoomPair(emitterRoom, ListenerCurrentRoom, out roomPair);
	}

	public bool TryGetPortalById(int id, out SpatialAudioPortal portal)
	{
		return this.m__E00A.TryGetPortalById(id, out portal);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		Clear();
	}

	public void Clear()
	{
		if (this.m__E008)
		{
			_E00E();
			for (int i = 0; i < this.m__E001.Count; i++)
			{
				_E009(this.m__E001.ElementAt(i));
			}
			this.m__E001.Clear();
			this.m__E002.Clear();
			this.m__E009?.Clear();
			this.m__E009 = null;
			this.m__E00A = null;
			this.m__E00B = null;
			this.m__E00C = 0f;
			_E3F5.Clear();
		}
	}

	private void _E00E()
	{
		if (this.m__E005.Count != 0)
		{
			for (int i = 0; i < this.m__E005.Count; i++)
			{
				this.m__E005.ElementAt(i).Clear();
			}
			this.m__E005.Clear();
		}
	}
}
