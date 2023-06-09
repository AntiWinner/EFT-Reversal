using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace EFT.UI;

public sealed class EnvironmentUIRoot : MonoBehaviour
{
	[Serializable]
	private struct EnvironmentEventObject
	{
		public EEventType EventType;

		public GameObject[] GameObjects;
	}

	private const float m__E000 = 1f;

	public CanvasGroup Shading;

	[SerializeField]
	private Transform CameraContainer;

	[SerializeField]
	private Light[] MainScreenLights;

	[SerializeField]
	private GameObject[] MainScreenObjects;

	[SerializeField]
	private ScreenPositionAnchor[] ScreenAnchors;

	[SerializeField]
	private EnvironmentEventObject[] EventObjects;

	private VRamUsageWrapper m__E001;

	public VRamUsageWrapper VRamWrapper => this.m__E001;

	public void Init(Camera alignmentCamera, IReadOnlyCollection<EEventType> events, bool isMain)
	{
		_E000(alignmentCamera);
		_E001(events);
		SetMain(isMain);
		SetCameraActive(value: true);
	}

	private void _E000(Camera newCam)
	{
		if (!ScreenAnchors.IsNullOrEmpty())
		{
			ScreenPositionAnchor[] screenAnchors = ScreenAnchors;
			for (int i = 0; i < screenAnchors.Length; i++)
			{
				screenAnchors[i].SetCamera(newCam);
			}
		}
	}

	public void SetCameraActive(bool value)
	{
		CameraContainer.gameObject.SetActive(value);
		if (this.m__E001 == null)
		{
			Camera componentInChildren = CameraContainer.gameObject.GetComponentInChildren<Camera>();
			this.m__E001 = new VRamUsageWrapper();
			this.m__E001.Init(componentInChildren);
		}
	}

	public void SetMain(bool isMain)
	{
		if (isMain)
		{
			_E002();
		}
		else
		{
			RandomRotate();
		}
		if (!MainScreenLights.IsNullOrEmpty())
		{
			Light[] mainScreenLights = MainScreenLights;
			for (int i = 0; i < mainScreenLights.Length; i++)
			{
				mainScreenLights[i].enabled = isMain;
			}
		}
		if (!MainScreenObjects.IsNullOrEmpty())
		{
			GameObject[] mainScreenObjects = MainScreenObjects;
			for (int i = 0; i < mainScreenObjects.Length; i++)
			{
				mainScreenObjects[i].SetActive(isMain);
			}
		}
	}

	private void _E001(IReadOnlyCollection<EEventType> events)
	{
		EnvironmentEventObject[] eventObjects = EventObjects;
		for (int i = 0; i < eventObjects.Length; i++)
		{
			EnvironmentEventObject environmentEventObject = eventObjects[i];
			GameObject[] gameObjects = environmentEventObject.GameObjects;
			for (int j = 0; j < gameObjects.Length; j++)
			{
				gameObjects[j].SetActive(events.Contains(environmentEventObject.EventType));
			}
		}
	}

	public void RandomRotate()
	{
		bool num = UnityEngine.Random.value > 0.5f;
		float min = (num ? 70f : (-110f));
		float max = (num ? 110f : (-70f));
		_E003(UnityEngine.Random.Range(min, max));
	}

	private void _E002()
	{
		if (CameraContainer.gameObject.activeInHierarchy)
		{
			CameraContainer.DOKill();
			CameraContainer.DORotateQuaternion(Quaternion.identity, 1f);
		}
		else
		{
			ResetRotation();
		}
	}

	private void _E003(float angle)
	{
		Vector3 vector = Vector3.up * angle;
		if (CameraContainer.gameObject.activeInHierarchy)
		{
			CameraContainer.DOKill();
			CameraContainer.DORotate(vector, 1f, RotateMode.LocalAxisAdd);
		}
		else
		{
			CameraContainer.Rotate(vector, Space.Self);
		}
	}

	public void ResetRotation()
	{
		CameraContainer.localRotation = Quaternion.identity;
	}
}
