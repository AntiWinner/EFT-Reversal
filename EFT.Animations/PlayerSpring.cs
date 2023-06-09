using System;
using UnityEngine;

namespace EFT.Animations;

public class PlayerSpring : MonoBehaviour
{
	[NonSerialized]
	[HideInInspector]
	public WeaponPrefab.AimPlane DefaultAimPlane;

	[NonSerialized]
	[HideInInspector]
	public WeaponPrefab.AimPlane[] CustomAimPlanes;

	[NonSerialized]
	[HideInInspector]
	public WeaponPrefab.AimPlane FarPlane;

	public Spring HandsPosition;

	public Spring HandsRotation;

	public RecoilSpring Recoil;

	public Spring CameraRotation;

	public Spring CameraPosition;

	public BetterSpring SwaySpring;

	public Transform TrackingTransform;

	public Transform WeaponRootAnim;

	public Transform CameraTransform;

	public Transform WeaponRoot;

	public Transform Weapon;

	public Transform Fireport;

	public Transform CameraAnimatedFP;

	public Transform CameraAnimatedTP;

	public Vector3 RotationCenter;

	public Vector3 RotationCenterWoStock;

	public Vector3 RecoilPivot;

	public Vector3 CameraOffset;

	public Transform RootJoint;

	private void Start()
	{
		SwaySpring.Cache();
	}
}
