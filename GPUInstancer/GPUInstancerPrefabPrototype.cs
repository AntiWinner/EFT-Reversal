using System;
using UnityEngine;

namespace GPUInstancer;

[Serializable]
public class GPUInstancerPrefabPrototype : GPUInstancerPrototype
{
	[Serializable]
	public class RigidbodyData
	{
		public bool useGravity;

		public float angularDrag;

		public float mass;

		public RigidbodyConstraints constraints;

		public float drag;

		public bool isKinematic;

		public RigidbodyInterpolation interpolation;
	}

	public bool enableRuntimeModifications;

	public bool startWithRigidBody;

	public bool addRemoveInstancesAtRuntime;

	public int extraBufferSize;

	public bool addRuntimeHandlerScript;

	public bool hasRigidBody;

	public RigidbodyData rigidbodyData;

	public bool meshRenderersDisabled;

	public bool isTransformsSerialized;

	public TextAsset serializedTransformData;

	public int serializedTransformDataCount;
}
