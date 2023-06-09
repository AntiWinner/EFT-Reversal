using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using UnityEngine;

namespace Audio.SpatialSystem;

[Serializable]
[RequireComponent(typeof(BoxCollider))]
public class SpatialAudioPortal : MonoBehaviour
{
	[Serializable]
	public enum PortalType
	{
		Opening,
		Static,
		Wall,
		Window
	}

	[Serializable]
	public enum PortalState
	{
		Open,
		Closed
	}

	[Tooltip("The level of portal transmission, 1 - full transmission like open portal, 0 - min transmission, like wall")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _transmission;

	[Tooltip("Optionally give the portal a unique name for more informative debug messages.")]
	public string portalName;

	[Tooltip("this function is used to try to adjust the size of the portal to the surrounding geometry")]
	public bool FitToGeometry;

	[Tooltip("experimental function to try to automatically set rotations for the portal")]
	public bool AutoRotate;

	public PortalType portalType;

	public PortalState state;

	public BoxCollider portalCollider;

	[Range(0f, 1f)]
	public float wallOcclusion = 1f;

	[Range(0f, 1f)]
	public float traversalMaxCost;

	public bool ToOutdoor;

	public AnimationCurve openEnvelope = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve closeEnvelope = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	[Range(0f, 10f)]
	public float openFadeTime = 0.1f;

	[Range(0f, 10f)]
	public float closeFadeTime = 0.1f;

	public string DoorID;

	[SerializeField]
	private int _iD;

	private readonly List<SpatialAudioRoom> _connectedRooms = new List<SpatialAudioRoom>();

	private bool _inProgressOpen;

	private bool _inProgressClose;

	private Action _unsubscribeOnDestroy;

	public int ID => _iD;

	public float PortalClosureLevel { get; private set; }

	private void Awake()
	{
		_unsubscribeOnDestroy = _EBAF.Instance.SubscribeOnEvent<_EBB7>(_E002);
		if ((portalType == PortalType.Opening || portalType == PortalType.Window) && state == PortalState.Closed)
		{
			_E008(allowFade: false);
		}
		else
		{
			_E009(allowFade: false);
		}
		_E005();
		_E006();
		_E000().HandleExceptions();
	}

	private async Task _E000()
	{
		await _E8D3.WaitGameStarted();
		_E001();
	}

	private void _E001()
	{
		if (portalType == PortalType.Opening)
		{
			World world = World._E019;
			if (world != null)
			{
				WorldInteractiveObject worldInteractiveObject = world.FindDoor(DoorID);
				if (worldInteractiveObject != null && _E004(worldInteractiveObject.transform.position))
				{
					state = ((worldInteractiveObject.DoorState != EDoorState.Open) ? PortalState.Closed : PortalState.Open);
				}
			}
		}
		if (portalType == PortalType.Window && Singleton<GameWorld>.Instance.Windows.TryGetByKey(DoorID.GetHashCode(), out var value))
		{
			if (_E004(value.transform.position))
			{
				state = ((!value.IsDamaged) ? PortalState.Closed : PortalState.Open);
			}
			WindowBreaker.OnWindowHitAction += _E003;
		}
		_E007(state, allowFade: false);
	}

	private void _E002(_EBB7 doorInteractionEvent)
	{
		Door door = doorInteractionEvent.Door;
		if (portalType == PortalType.Opening && (object)door != null && DoorID.Equals(door.Id) && _E004(door.transform.position) && door.Operatable)
		{
			if (state == PortalState.Closed && door.DoorState == EDoorState.Open)
			{
				_E007(PortalState.Open, allowFade: false);
			}
			else if (state != PortalState.Closed)
			{
				_E007(PortalState.Closed, allowFade: true);
			}
		}
	}

	private void _E003(WindowBreaker breaker, _EC23 info, WindowBreakingConfig.Crack crack, float damage)
	{
		if (portalType == PortalType.Window && (object)breaker != null && DoorID.Equals(breaker.Id) && _E004(breaker.transform.position) && breaker.IsDamaged)
		{
			_E009(allowFade: false);
		}
	}

	private bool _E004(Vector3 interactiveObjectPosition)
	{
		return Vector3.Distance(interactiveObjectPosition, base.transform.position) < 1f;
	}

	private void _E005()
	{
		portalName = (string.IsNullOrEmpty(portalName) ? base.gameObject.name : portalName);
	}

	private void _E006()
	{
		MeshRenderer component = GetComponent<MeshRenderer>();
		if ((bool)component)
		{
			component.enabled = false;
		}
	}

	private void _E007(PortalState portalState, bool allowFade)
	{
		if (portalState == PortalState.Open)
		{
			_E009(allowFade);
		}
		else
		{
			_E008(allowFade);
		}
	}

	private void _E008(bool allowFade)
	{
		if (portalType != PortalType.Wall)
		{
			float duration = (allowFade ? closeFadeTime : 0f);
			StartCoroutine(_E00B(PortalClosureLevel, duration));
		}
	}

	private void _E009(bool allowFade)
	{
		if (portalType != PortalType.Wall)
		{
			float duration = (allowFade ? openFadeTime : 0f);
			StartCoroutine(_E00A(PortalClosureLevel, duration));
		}
	}

	public void SetConnectedRoom(SpatialAudioRoom room)
	{
		if ((object)room != null)
		{
			_ = _connectedRooms.Count;
			_ = 2;
			_connectedRooms.Add(room);
		}
	}

	public List<SpatialAudioRoom> GetConnectedRooms()
	{
		return _connectedRooms;
	}

	private IEnumerator _E00A(float start, float duration)
	{
		duration = PortalClosureLevel * duration;
		float num = 0f;
		_inProgressOpen = true;
		_inProgressClose = false;
		while (num < duration && !_inProgressClose)
		{
			num += Time.deltaTime;
			float time = Mathf.Clamp01(num / duration);
			float value = openEnvelope.Evaluate(time);
			value = Mathf.Clamp01(value);
			PortalClosureLevel = start - start * value;
			yield return null;
		}
		if (!_inProgressClose)
		{
			PortalClosureLevel = 0f;
		}
		_inProgressOpen = false;
		state = PortalState.Open;
	}

	private IEnumerator _E00B(float start, float duration)
	{
		duration = (1f - PortalClosureLevel) * duration;
		float num = 0f;
		_inProgressClose = true;
		_inProgressOpen = false;
		while (num < duration && !_inProgressOpen)
		{
			num += Time.deltaTime;
			float time = Mathf.Clamp01(num / duration);
			float value = closeEnvelope.Evaluate(time);
			value = Mathf.Clamp01(value);
			float num2 = 1f - start;
			PortalClosureLevel = start + num2 * value;
			yield return null;
		}
		if (!_inProgressOpen)
		{
			PortalClosureLevel = Mathf.Lerp(1f, 0.1f, _transmission);
		}
		_inProgressClose = false;
		state = PortalState.Closed;
	}

	private void OnDestroy()
	{
		_E00C();
	}

	private void _E00C()
	{
		_unsubscribeOnDestroy?.Invoke();
		_unsubscribeOnDestroy = null;
		WindowBreaker.OnWindowHitAction -= _E003;
	}
}
