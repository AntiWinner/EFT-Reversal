using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerDetailManager : GPUInstancerTerrainManager
{
	public int detailLayer;

	public bool runInThreads = true;

	public bool doRefresh = true;

	private static ComputeShader m__E002;

	private ComputeBuffer m__E003;

	private bool m__E004;

	private ComputeBuffer m__E005;

	private int[] m__E006 = new int[1];

	public float[,] threadHeightMapData;

	public List<int[,]> threadDetailMapData;

	public int threadHeightResolution;

	private float m__E007;

	public override void Awake()
	{
		if (GPUInstancerDetailManager.m__E002 == null)
		{
			GPUInstancerDetailManager.m__E002 = (ComputeShader)Resources.Load(_E4BF.GRASS_INSTANTIATION_RESOURCE_PATH);
		}
		base.terrain.freeUnusedRenderingResources = false;
		base.Awake();
		_E8A8.Instance.OnCameraChanged -= _E000;
		_E8A8.Instance.OnCameraChanged += _E000;
		_E000();
	}

	private void _E000()
	{
		_E006(_E001());
	}

	private IEnumerator _E001()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		_E4BD.UpdateTerrainNormalMapDetailInstance(this);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		this.m__E003?.Release();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_E8A8.Instance.OnCameraChanged -= _E000;
	}

	public override void ClearInstancingData()
	{
		base.ClearInstancingData();
		if (base.terrain != null && base.terrain.detailObjectDistance <= 0f)
		{
			base.terrain.detailObjectDistance = terrainSettings.maxDetailDistanceLegacy;
		}
		this.m__E005?.Release();
		this.m__E005 = null;
	}

	public override void GeneratePrototypes(bool forceNew = false)
	{
		base.GeneratePrototypes(forceNew);
		if (terrainSettings != null && base.terrain != null && base.terrain.terrainData != null)
		{
			_E4C8.SetDetailInstancePrototypes(this, prototypeList, base.terrain.terrainData.detailPrototypes, 2, terrainSettings, forceNew, base.terrain);
		}
	}

	public override void InitializeRuntimeDataAndBuffers(bool forceNew = true)
	{
		base.InitializeRuntimeDataAndBuffers(forceNew);
		if ((!forceNew && isInitialized) || terrainSettings == null || base.terrain == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(terrainSettings.warningText))
		{
			Debug.LogError(_ED3E._E000(115316));
			return;
		}
		replacingInstances = false;
		initalizingInstances = true;
		base.terrain.detailObjectDistance = 0f;
		InitializeSpatialPartitioning();
		List<GPUInstancerPrototype> list = prototypeList;
		if (list != null && list.Count > 0)
		{
			_E4C8.AddDetailInstanceRuntimeDataToList(base.terrain, runtimeDataList, prototypeList, terrainSettings, detailLayer);
		}
		isInitialized = true;
	}

	public override void UpdateSpatialPartitioningCells(GPUInstancerCameraData renderingCameraData)
	{
		base.UpdateSpatialPartitioningCells(renderingCameraData);
		if (!(terrainSettings == null) && spData != null && !initalizingInstances && !replacingInstances && spData.IsActiveCellUpdateRequired(renderingCameraData.mainCamera.transform.position))
		{
			replacingInstances = true;
			_E006(_E4BF.DETAIL_STORE_INSTANCE_DATA ? _E007() : _E008());
		}
	}

	public override void DeletePrototype(GPUInstancerPrototype prototype, bool removeSO = true)
	{
		if (terrainSettings != null && base.terrain != null && base.terrain.terrainData != null)
		{
			int num = prototypeList.IndexOf(prototype);
			DetailPrototype[] detailPrototypes = base.terrain.terrainData.detailPrototypes;
			List<DetailPrototype> list = new List<DetailPrototype>();
			List<int[,]> list2 = new List<int[,]>();
			for (int i = 0; i < detailPrototypes.Length; i++)
			{
				if (i != num)
				{
					list.Add(detailPrototypes[i]);
					list2.Add(base.terrain.terrainData.GetDetailLayer(0, 0, base.terrain.terrainData.detailResolution, base.terrain.terrainData.detailResolution, i));
				}
				base.terrain.terrainData.SetDetailLayer(0, 0, i, new int[base.terrain.terrainData.detailResolution, base.terrain.terrainData.detailResolution]);
			}
			base.terrain.terrainData.detailPrototypes = list.ToArray();
			for (int j = 0; j < list2.Count; j++)
			{
				base.terrain.terrainData.SetDetailLayer(0, 0, j, list2[j]);
			}
			base.terrain.terrainData.RefreshPrototypes();
			if (removeSO)
			{
				base.DeletePrototype(prototype, removeSO);
			}
			GeneratePrototypes();
			if (!removeSO)
			{
				base.DeletePrototype(prototype, removeSO);
			}
		}
		else
		{
			base.DeletePrototype(prototype, removeSO);
		}
	}

	public override void RemoveInstancesInsideBounds(Bounds bounds, float offset, List<GPUInstancerPrototype> prototypeFilter = null)
	{
		base.RemoveInstancesInsideBounds(bounds, offset, prototypeFilter);
		if (spData == null || initalizingInstances)
		{
			return;
		}
		int num = base.terrain.terrainData.detailResolution / spData.cellRowAndCollumnCountPerTerrain;
		float num2 = base.terrain.terrainData.size.x / (float)spData.cellRowAndCollumnCountPerTerrain / (float)num;
		float num3 = base.terrain.terrainData.size.z / (float)spData.cellRowAndCollumnCountPerTerrain / (float)num;
		int num4 = Mathf.CeilToInt((bounds.extents.x * 2f + offset) / num2);
		int num5 = Mathf.CeilToInt((bounds.extents.z * 2f + offset) / num3);
		foreach (_E4C0 cell in spData.GetCellList())
		{
			if (!cell.cellInnerBounds.Intersects(bounds))
			{
				continue;
			}
			if (cell.isActive && cell.detailInstanceBuffers != null)
			{
				foreach (int key in cell.detailInstanceBuffers.Keys)
				{
					if (prototypeFilter == null || prototypeFilter.Contains(prototypeList[key]))
					{
						_E4C8.RemoveInstancesInsideBounds(cell.detailInstanceBuffers[key], bounds.center, bounds.extents, offset);
					}
				}
			}
			int num6 = Mathf.FloorToInt((bounds.center.x - bounds.extents.x - cell.instanceStartPosition.x - offset) / num2);
			int num7 = Mathf.FloorToInt((bounds.center.z - bounds.extents.z - cell.instanceStartPosition.z - offset) / num3);
			for (int i = 0; i < cell.detailMapData.Count; i++)
			{
				if (prototypeFilter != null && !prototypeFilter.Contains(prototypeList[i]))
				{
					continue;
				}
				for (int j = num7; j < num5 + num7; j++)
				{
					if (j < 0 || j >= num)
					{
						continue;
					}
					for (int k = num6; k < num4 + num6; k++)
					{
						if (k >= 0 && k < num)
						{
							cell.detailMapData[i][k + j * num] = 0;
						}
					}
				}
			}
		}
	}

	public override void RemoveInstancesInsideCollider(Collider collider, float offset, List<GPUInstancerPrototype> prototypeFilter = null)
	{
		base.RemoveInstancesInsideCollider(collider, offset, prototypeFilter);
		if (spData == null || initalizingInstances)
		{
			return;
		}
		Bounds bounds = collider.bounds;
		int num = base.terrain.terrainData.detailResolution / spData.cellRowAndCollumnCountPerTerrain;
		float num2 = base.terrain.terrainData.size.x / (float)spData.cellRowAndCollumnCountPerTerrain / (float)num;
		float num3 = base.terrain.terrainData.size.z / (float)spData.cellRowAndCollumnCountPerTerrain / (float)num;
		int num4 = Mathf.CeilToInt((bounds.extents.x * 2f + offset) / num2);
		int num5 = Mathf.CeilToInt((bounds.extents.z * 2f + offset) / num3);
		Vector3 zero = Vector3.zero;
		_ = Vector3.zero;
		foreach (_E4C0 cell in spData.GetCellList())
		{
			if (!cell.cellInnerBounds.Intersects(bounds))
			{
				continue;
			}
			if (cell.isActive && cell.detailInstanceBuffers != null)
			{
				foreach (int key in cell.detailInstanceBuffers.Keys)
				{
					if (prototypeFilter == null || prototypeFilter.Contains(prototypeList[key]))
					{
						if (collider is BoxCollider)
						{
							_E4C8.RemoveInstancesInsideBoxCollider(cell.detailInstanceBuffers[key], (BoxCollider)collider, offset);
						}
						else if (collider is SphereCollider)
						{
							_E4C8.RemoveInstancesInsideSphereCollider(cell.detailInstanceBuffers[key], (SphereCollider)collider, offset);
						}
						else if (collider is CapsuleCollider)
						{
							_E4C8.RemoveInstancesInsideCapsuleCollider(cell.detailInstanceBuffers[key], (CapsuleCollider)collider, offset);
						}
						else
						{
							_E4C8.RemoveInstancesInsideBounds(cell.detailInstanceBuffers[key], collider.bounds.center, collider.bounds.extents, offset);
						}
					}
				}
			}
			int num6 = Mathf.FloorToInt((bounds.center.x - bounds.extents.x - cell.instanceStartPosition.x - offset) / num2);
			int num7 = Mathf.FloorToInt((bounds.center.z - bounds.extents.z - cell.instanceStartPosition.z - offset) / num3);
			for (int i = num7; i < num5 + num7; i++)
			{
				if (i < 0 || i >= num)
				{
					continue;
				}
				for (int j = num6; j < num4 + num6; j++)
				{
					if (j < 0 || j >= num)
					{
						continue;
					}
					zero.x = cell.instanceStartPosition.x + (float)j * num2;
					zero.z = cell.instanceStartPosition.z + (float)i * num3;
					zero.y = base.terrain.SampleHeight(zero);
					if (Vector3.Distance(collider.ClosestPoint(zero), zero) > num2 + offset)
					{
						continue;
					}
					for (int k = 0; k < cell.detailMapData.Count; k++)
					{
						if (prototypeFilter == null || prototypeFilter.Contains(prototypeList[k]))
						{
							cell.detailMapData[k][j + i * num] = 0;
						}
					}
				}
			}
		}
	}

	public override void SetGlobalPositionOffset(Vector3 offsetPosition)
	{
		base.SetGlobalPositionOffset(offsetPosition);
		if (spData == null)
		{
			return;
		}
		foreach (_E4C0 cell in spData.GetCellList())
		{
			if (cell == null)
			{
				continue;
			}
			cell.instanceStartPosition += offsetPosition;
			cell.cellBounds.center += offsetPosition;
			if (cell.detailInstanceBuffers != null)
			{
				foreach (ComputeBuffer value in cell.detailInstanceBuffers.Values)
				{
					if (value != null)
					{
						_E4BF.computeRuntimeModification.SetBuffer(_E4BF.computeBufferTransformOffsetId, _E4BF._E001.INSTANCE_DATA_BUFFER, value);
						_E4BF.computeRuntimeModification.SetInt(_E4BF._E001.BUFFER_PARAMETER_BUFFER_SIZE, value.count);
						_E4BF.computeRuntimeModification.SetVector(_E4BF._E006.BUFFER_PARAMETER_POSITION_OFFSET, offsetPosition);
						_E4BF.computeRuntimeModification.Dispatch(_E4BF.computeBufferTransformOffsetId, Mathf.CeilToInt((float)value.count / _E4BF.COMPUTE_SHADER_THREAD_COUNT), 1, 1);
					}
				}
			}
			if (!_E4BF.DETAIL_STORE_INSTANCE_DATA || cell.detailInstanceList == null)
			{
				continue;
			}
			foreach (Matrix4x4[] value2 in cell.detailInstanceList.Values)
			{
				for (int i = 0; i < value2.Length; i++)
				{
					value2[i].SetColumn(3, value2[i].GetColumn(3) + new Vector4(offsetPosition.x, offsetPosition.y, offsetPosition.z, 0f));
				}
			}
		}
	}

	private static int _E002(int value, int max, int failValue)
	{
		if (value >= max)
		{
			return failValue;
		}
		return value;
	}

	public static Matrix4x4[] GetInstanceDataForDetailPrototype(GPUInstancerDetailPrototype detailPrototype, int[] detailMap, float[] heightMapData, int detailMapSize, int heightMapSize, int detailResolution, int heightResolution, Vector3 startPosition, Vector3 terrainSize, int instanceCount)
	{
		Matrix4x4[] array = new Matrix4x4[instanceCount];
		if (instanceCount == 0)
		{
			return array;
		}
		System.Random prng = new System.Random();
		float num = ((float)heightResolution - 1f) / (float)detailResolution;
		int max = heightMapSize * heightMapSize;
		float num2 = terrainSize.x / (float)detailResolution;
		float num3 = terrainSize.z / (float)detailResolution;
		float num4 = (float)heightResolution / (terrainSize.x / terrainSize.y);
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		Vector3 zero2 = Vector3.zero;
		Vector3 vector = new Vector3(0f, 0f, 1f);
		Vector3 vector2 = new Vector3(1f, 0f, 0f);
		Vector3 zero3 = Vector3.zero;
		int num5 = 0;
		Vector3 pos = default(Vector3);
		for (int i = 0; i < detailMapSize; i++)
		{
			for (int j = 0; j < detailMapSize; j++)
			{
				for (int k = 0; k < detailMap[i * detailMapSize + j]; k++)
				{
					pos.x = (float)j + prng.Range(0f, 0.99f);
					pos.y = 0f;
					pos.z = (float)i + prng.Range(0f, 0.99f);
					float num6 = pos.x * num;
					float num7 = pos.z * num;
					int num8 = Mathf.FloorToInt(num6) + Mathf.FloorToInt(num7) * heightMapSize;
					float num9 = heightMapData[num8];
					float num10 = heightMapData[_E002(num8 + heightMapSize, max, num8)];
					float num11 = heightMapData[num8 + 1];
					float rightTopH = heightMapData[_E002(num8 + heightMapSize + 1, max, num8)];
					pos.x *= num2;
					pos.y = _E4C8.SampleTerrainHeight(num6 - Mathf.Floor(num6), num7 - Mathf.Floor(num7), num9, num10, num11, rightTopH) * terrainSize.y;
					pos.z *= num3;
					pos += startPosition;
					zero2.y = num9 * num4;
					vector.y = num10 * num4;
					zero2.y = num11 * num4;
					zero3 = Vector3.Cross(vector - vector2, vector2 - zero2).normalized;
					identity.SetFromToRotation(Vector3.up, zero3);
					identity *= Quaternion.AngleAxis(prng.Range(0f, 360f), Vector3.up);
					float num12 = prng.Range(0f, 1f);
					float num13 = detailPrototype.detailScale.x + (detailPrototype.detailScale.y - detailPrototype.detailScale.x) * num12;
					float y = detailPrototype.detailScale.z + (detailPrototype.detailScale.w - detailPrototype.detailScale.z) * num12;
					zero.x = num13;
					zero.y = y;
					zero.z = num13;
					array[num5].SetTRS(pos, identity, zero);
					num5++;
				}
			}
		}
		return array;
	}

	private Matrix4x4[] _E003(GPUInstancerDetailPrototype detailPrototype, int[] detailMap, float[] heightMapData, int heightMapSize, int heightResolution, Vector3 startPosition, Vector3 terrainSize, int instanceCount, ComputeShader grassInstantiationComputeShader, GPUInstancerTerrainSettings terrainSettings)
	{
		Matrix4x4[] array = new Matrix4x4[instanceCount];
		if (instanceCount == 0)
		{
			return array;
		}
		int num = detailPrototype.detailResolution / spData.cellRowAndCollumnCountPerTerrain;
		int grassInstantiationComputeKernelId = grassInstantiationComputeShader.FindKernel(_E4BF.GRASS_INSTANTIATION_KERNEL);
		ComputeBuffer computeBuffer = new ComputeBuffer(heightMapData.Length, _E4BF.STRIDE_SIZE_INT);
		computeBuffer.SetData(heightMapData);
		ComputeBuffer computeBuffer2 = new ComputeBuffer(instanceCount, _E4BF.STRIDE_SIZE_MATRIX4X4);
		this.m__E005.SetData(this.m__E006);
		ComputeBuffer computeBuffer3 = new ComputeBuffer(Mathf.CeilToInt(num * num), _E4BF.STRIDE_SIZE_INT);
		computeBuffer3.SetData(detailMap);
		_E005(grassInstantiationComputeShader, grassInstantiationComputeKernelId, computeBuffer2, computeBuffer3, computeBuffer, new Vector4(num, num, heightMapSize, heightMapSize), startPosition, terrainSize, detailPrototype.detailResolution, heightResolution, detailPrototype.detailScale, terrainSettings.GetHealthyDryNoiseTexture(detailPrototype), detailPrototype.noiseSpread, detailPrototype.GetInstanceID(), detailPrototype.detailDensity, detailPrototype.detailGrowDirection);
		computeBuffer3.Release();
		computeBuffer2.GetData(array);
		computeBuffer2.Release();
		computeBuffer.Release();
		return array;
	}

	private ComputeBuffer _E004(GPUInstancerDetailPrototype detailPrototype, int heightMapSize, int heightResolution, Vector3 startPosition, Vector3 terrainSize, int instanceCount, ComputeShader grassInstantiationComputeShader, GPUInstancerTerrainSettings terrainSettings, ComputeBuffer heightMapBuffer, ComputeBuffer detailMapBuffer)
	{
		if (instanceCount == 0)
		{
			return null;
		}
		int num = detailPrototype.detailResolution / spData.cellRowAndCollumnCountPerTerrain;
		int grassInstantiationComputeKernelId = grassInstantiationComputeShader.FindKernel(_E4BF.GRASS_INSTANTIATION_KERNEL);
		ComputeBuffer computeBuffer = new ComputeBuffer(instanceCount, _E4BF.STRIDE_SIZE_MATRIX4X4);
		this.m__E005.SetData(this.m__E006);
		_E005(grassInstantiationComputeShader, grassInstantiationComputeKernelId, computeBuffer, detailMapBuffer, heightMapBuffer, new Vector4(num, num, heightMapSize, heightMapSize), startPosition, terrainSize, detailPrototype.detailResolution, heightResolution, detailPrototype.detailScale, terrainSettings.GetHealthyDryNoiseTexture(detailPrototype), detailPrototype.noiseSpread, detailPrototype.GetInstanceID(), detailPrototype.detailDensity, detailPrototype.detailGrowDirection);
		return computeBuffer;
	}

	private void _E005(ComputeShader grassComputeShader, int grassInstantiationComputeKernelId, ComputeBuffer visibilityBuffer, ComputeBuffer detailMapBuffer, ComputeBuffer heightMapBuffer, Vector4 detailAndHeightMapSize, Vector3 startPosition, Vector3 terrainSize, float detailResolution, int heightResolution, Vector4 detailScale, Texture healthyDryNoiseTexture, float noiseSpread, int instanceID, float detailDensity, float detailTerrainNormal)
	{
		grassComputeShader.SetBuffer(grassInstantiationComputeKernelId, _E4BF._E001.INSTANCE_DATA_BUFFER, visibilityBuffer);
		grassComputeShader.SetBuffer(grassInstantiationComputeKernelId, _E4BF._E004.DETAIL_MAP_DATA_BUFFER, detailMapBuffer);
		grassComputeShader.SetBuffer(grassInstantiationComputeKernelId, _E4BF._E004.HEIGHT_MAP_DATA_BUFFER, heightMapBuffer);
		grassComputeShader.SetBuffer(grassInstantiationComputeKernelId, _E4BF._E004.COUNTER_BUFFER, this.m__E005);
		grassComputeShader.SetFloat(_E4BF._E004.TERRAIN_DETAIL_RESOLUTION_DATA, detailResolution);
		grassComputeShader.SetInt(_E4BF._E004.TERRAIN_HEIGHT_RESOLUTION_DATA, heightResolution);
		grassComputeShader.SetVector(_E4BF._E004.GRASS_START_POSITION_DATA, startPosition);
		grassComputeShader.SetVector(_E4BF._E004.TERRAIN_SIZE_DATA, terrainSize);
		grassComputeShader.SetVector(_E4BF._E004.DETAIL_SCALE_DATA, detailScale);
		grassComputeShader.SetVector(_E4BF._E004.DETAIL_AND_HEIGHT_MAP_SIZE_DATA, detailAndHeightMapSize);
		if (healthyDryNoiseTexture != null)
		{
			grassComputeShader.SetTexture(grassInstantiationComputeKernelId, _E4BF._E004.HEALTHY_DRY_NOISE_TEXTURE, healthyDryNoiseTexture);
			grassComputeShader.SetFloat(_E4BF._E004.NOISE_SPREAD, noiseSpread);
		}
		grassComputeShader.SetFloat(_E4BF._E004.DETAIL_UNIQUE_VALUE, (float)instanceID / 1000f);
		grassComputeShader.SetFloat(_E4BF._E004.DETAIL_DENSITY, detailDensity);
		grassComputeShader.SetFloat(_E4BF._E004.DETAIL_TERRAIN_NORMAL, detailTerrainNormal);
		grassComputeShader.Dispatch(grassInstantiationComputeKernelId, Mathf.CeilToInt(detailAndHeightMapSize.x / _E4BF.COMPUTE_SHADER_THREAD_COUNT_2D), 1, Mathf.CeilToInt(detailAndHeightMapSize.y / _E4BF.COMPUTE_SHADER_THREAD_COUNT_2D));
	}

	private void _E006(IEnumerator routine)
	{
		if (Application.isPlaying)
		{
			StartCoroutine(routine);
		}
		else
		{
			while (routine.MoveNext())
			{
			}
		}
	}

	public override void InitializeSpatialPartitioning()
	{
		base.InitializeSpatialPartitioning();
		_E4C8.ReleaseSPBuffers(spData);
		spData = new _E4C6<_E4BE>();
		_E4C8.CalculateSpatialPartitioningValuesFromTerrain(spData, base.terrain, terrainSettings.maxDetailDistance, (!terrainSettings.autoSPCellSize) ? terrainSettings.preferedSPCellSize : 0);
		_E009();
	}

	private IEnumerator _E007()
	{
		if (initalizingInstances)
		{
			yield break;
		}
		List<_E4C2> list = new List<_E4C2>(runtimeDataList);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			_E4C2 obj = list[i];
			if (spData.activeCellList != null && spData.activeCellList.Count > 0)
			{
				int num3 = 0;
				foreach (_E4C0 activeCell in spData.activeCellList)
				{
					if (activeCell != null && activeCell.detailInstanceList != null)
					{
						num3 += activeCell.detailInstanceList[i].Length;
					}
				}
				obj.bufferSize = num3;
				obj.instanceCount = num3;
				if (num3 == 0)
				{
					obj.transformationMatrixVisibilityBuffer?.Release();
					obj.transformationMatrixVisibilityBuffer = null;
					continue;
				}
				this.m__E003 = new ComputeBuffer(num3, _E4BF.STRIDE_SIZE_MATRIX4X4);
				int num4 = 0;
				for (int j = 0; j < spData.activeCellList.Count; j++)
				{
					_E4C0 obj3 = (_E4C0)spData.activeCellList[j];
					for (int k = 0; (float)k < Mathf.Ceil((float)obj3.detailInstanceList[i].Length / (float)_E4BF.BUFFER_COROUTINE_STEP_NUMBER); k++)
					{
						int num5 = k * _E4BF.BUFFER_COROUTINE_STEP_NUMBER;
						int num6 = _E4BF.BUFFER_COROUTINE_STEP_NUMBER;
						if (num5 + num6 > obj3.detailInstanceList[i].Length)
						{
							num6 = obj3.detailInstanceList[i].Length - num5;
						}
						this.m__E003.SetDataPartial(obj3.detailInstanceList[i], num5, num4, num6);
						num4 += num6;
						num += num6;
						if (num6 + num5 < obj3.detailInstanceList[i].Length - 1 && num - num2 > _E4BF.BUFFER_COROUTINE_STEP_NUMBER)
						{
							num2 = num;
							yield return null;
						}
					}
					if (initalizingInstances)
					{
						break;
					}
				}
				if (initalizingInstances)
				{
					break;
				}
				obj.transformationMatrixVisibilityBuffer?.Release();
				obj.transformationMatrixVisibilityBuffer = this.m__E003;
			}
			if (initalizingInstances)
			{
				break;
			}
			_E4C8.InitializeGPUBuffer(obj);
			num2 = num;
			yield return null;
		}
		if (initalizingInstances)
		{
			this.m__E003?.Release();
			_E4C8.ReleaseInstanceBuffers(list);
			_E4C8.ClearInstanceData(list);
		}
		this.m__E003 = null;
		replacingInstances = false;
		if (!initalizingInstances)
		{
			if (this.m__E004)
			{
				_E4C8.TriggerEvent(GPUInstancerEventType.DetailInitializationFinished);
			}
			this.m__E004 = false;
			isInitial = true;
		}
	}

	private IEnumerator _E008()
	{
		if (initalizingInstances)
		{
			yield break;
		}
		List<_E4C2> list = new List<_E4C2>(runtimeDataList);
		int heightMapSize = (base.terrain.terrainData.heightmapResolution - 1) / spData.cellRowAndCollumnCountPerTerrain + 1;
		int heightmapResolution = base.terrain.terrainData.heightmapResolution;
		Vector3 size = base.terrain.terrainData.size;
		ComputeBuffer computeBuffer = null;
		ComputeBuffer computeBuffer2 = null;
		_E4C0 obj = null;
		if (spData.activeCellList != null && spData.activeCellList.Count > 0)
		{
			for (int i = 0; i < spData.activeCellList.Count; i++)
			{
				_E4C0 obj2 = (_E4C0)spData.activeCellList[i];
				if (obj2?.totalDetailCounts == null)
				{
					continue;
				}
				if (obj2.detailInstanceBuffers == null)
				{
					obj2.detailInstanceBuffers = new Dictionary<int, ComputeBuffer>();
				}
				for (int j = 0; j < obj2.totalDetailCounts.Count; j++)
				{
					if (obj2.totalDetailCounts[j] <= 0 || (obj2.detailInstanceBuffers.ContainsKey(j) && obj2.detailInstanceBuffers[j] != null))
					{
						continue;
					}
					if (obj != obj2)
					{
						if (computeBuffer == null)
						{
							computeBuffer = new ComputeBuffer(obj2.heightMapData.Length, _E4BF.STRIDE_SIZE_INT);
						}
						computeBuffer.SetData(obj2.heightMapData);
						obj = obj2;
					}
					GPUInstancerDetailPrototype gPUInstancerDetailPrototype = (GPUInstancerDetailPrototype)prototypeList[j];
					int num = gPUInstancerDetailPrototype.detailResolution / spData.cellRowAndCollumnCountPerTerrain;
					computeBuffer2?.Release();
					computeBuffer2 = new ComputeBuffer(num * num, _E4BF.STRIDE_SIZE_INT);
					computeBuffer2.SetData(obj2.detailMapData[j]);
					obj2.detailInstanceBuffers[j] = _E004(gPUInstancerDetailPrototype, heightMapSize, heightmapResolution, obj2.instanceStartPosition, size, obj2.totalDetailCounts[j], GPUInstancerDetailManager.m__E002, terrainSettings, computeBuffer, computeBuffer2);
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			_E4C2 obj3 = list[k];
			if (spData.activeCellList != null && spData.activeCellList.Count > 0)
			{
				int num2 = 0;
				for (int l = 0; l < spData.activeCellList.Count; l++)
				{
					_E4C0 obj4 = (_E4C0)spData.activeCellList[l];
					if (obj4?.totalDetailCounts != null)
					{
						num2 += obj4.totalDetailCounts[k];
					}
				}
				obj3.bufferSize = num2;
				obj3.instanceCount = num2;
				if (num2 == 0)
				{
					obj3.transformationMatrixVisibilityBuffer?.Release();
					obj3.transformationMatrixVisibilityBuffer = null;
					continue;
				}
				this.m__E003 = new ComputeBuffer(num2, _E4BF.STRIDE_SIZE_MATRIX4X4);
				int num3 = 0;
				for (int m = 0; m < spData.activeCellList.Count; m++)
				{
					_E4C0 obj5 = (_E4C0)spData.activeCellList[m];
					if (obj5.detailInstanceBuffers.ContainsKey(k) && obj5.detailInstanceBuffers[k] != null)
					{
						this.m__E003.CopyComputeBuffer(num3, obj5.detailInstanceBuffers[k].count, obj5.detailInstanceBuffers[k]);
						num3 += obj5.detailInstanceBuffers[k].count;
					}
				}
				obj3.transformationMatrixVisibilityBuffer?.Release();
				obj3.transformationMatrixVisibilityBuffer = this.m__E003;
			}
			_E4C8.InitializeGPUBuffer(obj3);
		}
		computeBuffer?.Release();
		computeBuffer2?.Release();
		this.m__E003 = null;
		replacingInstances = false;
		if (this.m__E004)
		{
			_E4C8.TriggerEvent(GPUInstancerEventType.DetailInitializationFinished);
		}
		this.m__E004 = false;
	}

	private void _E009()
	{
		if (this.m__E005 == null)
		{
			this.m__E005 = new ComputeBuffer(1, _E4BF.STRIDE_SIZE_INT);
		}
		_E006(FillCellsDetailData(base.terrain));
	}

	public void FillCellsDetailDataCallBack()
	{
		ClearCompletedThreads();
		if (threadHeightMapData != null && (!runInThreads || activeThreads.Count <= 0))
		{
			threadHeightMapData = null;
			threadDetailMapData = null;
			if (_E4BF.DETAIL_STORE_INSTANCE_DATA)
			{
				int heightMapSize = (base.terrain.terrainData.heightmapResolution - 1) / spData.cellRowAndCollumnCountPerTerrain + 1;
				int heightmapResolution = base.terrain.terrainData.heightmapResolution;
				Vector3 size = base.terrain.terrainData.size;
				_E006(_E00B(spData, prototypeList, heightMapSize, heightmapResolution, size, terrainSettings, _E00A));
			}
			else
			{
				_E00A();
			}
		}
	}

	private void _E00A()
	{
		initalizingInstances = false;
		foreach (_E4BE activeCell in spData.activeCellList)
		{
			activeCell.isActive = false;
		}
		spData.activeCellList.Clear();
		this.m__E004 = true;
	}

	private IEnumerator _E00B(_E4C6<_E4BE> spData, List<GPUInstancerPrototype> prototypeList, int heightMapSize, int heightmapResolution, Vector3 terrainSize, GPUInstancerTerrainSettings terrainSettings, Action callback)
	{
		int num = 0;
		foreach (_E4C0 cell in spData.GetCellList())
		{
			if (cell.detailMapData == null)
			{
				continue;
			}
			cell.detailInstanceList = new Dictionary<int, Matrix4x4[]>();
			for (int i = 0; i < prototypeList.Count; i++)
			{
				num += cell.totalDetailCounts[i];
				cell.detailInstanceList[i] = _E003((GPUInstancerDetailPrototype)prototypeList[i], cell.detailMapData[i], cell.heightMapData, heightMapSize, heightmapResolution, cell.instanceStartPosition, terrainSize, cell.totalDetailCounts[i], GPUInstancerDetailManager.m__E002, terrainSettings);
				if (num >= _E4BF.BUFFER_COROUTINE_STEP_NUMBER)
				{
					num = 0;
					yield return null;
				}
			}
		}
		callback();
	}

	private int[,] _E00C(int layer, float density, int resizeCount)
	{
		int detailResolution = base.terrain.terrainData.detailResolution;
		int[,] array = base.terrain.terrainData.GetDetailLayer(0, 0, detailResolution, detailResolution, layer);
		if (density == 1f)
		{
			return array;
		}
		float density2 = Mathf.Pow(density, 1f / (float)resizeCount);
		for (int i = 0; i < resizeCount; i++)
		{
			array = _E00D(array, density2);
		}
		return array;
	}

	private int[,] _E00D(int[,] map, float density)
	{
		int length = map.GetLength(0);
		int num = Mathf.RoundToInt((float)length * density);
		int[,] array = new int[num, num];
		float[,] array2 = new float[num, num];
		this.m__E007 = 0f;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				float num2 = 1f * (float)i / (float)num;
				float num3 = 1f * (float)j / (float)num;
				float num4 = num2 * (float)length;
				float num5 = num3 * (float)length;
				float num6 = 1f * (float)(i + 1) / (float)num;
				float num7 = 1f * (float)(j + 1) / (float)num;
				float num8 = num6 * (float)length;
				float num9 = num7 * (float)length;
				int num10 = 8;
				float num11 = (num8 - num4) / (float)num10;
				float num12 = (num9 - num5) / (float)num10;
				for (int k = 0; k < num10; k++)
				{
					for (int l = 0; l < num10; l++)
					{
						int num13 = Mathf.Clamp(Mathf.FloorToInt(num4 + num11 * (float)k), 0, length - 1);
						int num14 = Mathf.Clamp(Mathf.FloorToInt(num5 + num12 * (float)l), 0, length - 1);
						this.m__E007 += map[num13, num14];
					}
				}
				array2[i, j] = this.m__E007 / (float)(num10 * num10);
				this.m__E007 = 0f;
				array[i, j] = Mathf.CeilToInt(array2[i, j]);
			}
		}
		return array;
	}

	public IEnumerator FillCellsDetailData(Terrain terrain)
	{
		threadHeightResolution = terrain.terrainData.heightmapResolution;
		if (threadHeightMapData == null)
		{
			threadHeightMapData = terrain.terrainData.GetHeights(0, 0, threadHeightResolution, threadHeightResolution);
		}
		if (threadDetailMapData == null)
		{
			threadDetailMapData = new List<int[,]>();
			for (int i = 0; i < prototypeList.Count; i++)
			{
				GPUInstancerDetailPrototype gPUInstancerDetailPrototype = prototypeList[i] as GPUInstancerDetailPrototype;
				if (!(gPUInstancerDetailPrototype == null))
				{
					int[,] array = _E00E(gPUInstancerDetailPrototype.cachedDensityMapForInstance);
					if (array == null || array.GetLength(0) == 0)
					{
						Debug.LogWarning(gPUInstancerDetailPrototype.name + _ED3E._E000(115441));
						float density = Mathf.Floor(Mathf.Sqrt(gPUInstancerDetailPrototype.detailDensity) * 16f) * 0.0625f;
						int resizeDensityCount = terrainSettings.resizeDensityCount;
						array = _E00C(i, density, resizeDensityCount);
					}
					threadDetailMapData.Add(array);
					gPUInstancerDetailPrototype.detailResolution = array.GetLength(0);
					if (runInThreads && i % 3 == 0)
					{
						yield return null;
					}
				}
			}
		}
		if (runInThreads)
		{
			ParameterizedThreadStart start = FillCellsDetailDataThread;
			Thread thread = new Thread(start);
			thread.IsBackground = true;
			Vector4 zero = Vector4.zero;
			zero.z = Mathf.CeilToInt((float)spData.cellRowAndCollumnCountPerTerrain / 2f);
			zero.w = spData.cellRowAndCollumnCountPerTerrain;
			threadStartQueue.Enqueue(new GPUInstancerManager._E000
			{
				thread = thread,
				parameter = zero
			});
			if (spData.cellRowAndCollumnCountPerTerrain > 1)
			{
				thread = new Thread(start);
				thread.IsBackground = true;
				zero.x = zero.z;
				zero.z = spData.cellRowAndCollumnCountPerTerrain;
				threadStartQueue.Enqueue(new GPUInstancerManager._E000
				{
					thread = thread,
					parameter = zero
				});
			}
		}
		else
		{
			Vector4 coord = new Vector4(0f, 0f, spData.cellRowAndCollumnCountPerTerrain, spData.cellRowAndCollumnCountPerTerrain);
			_E006(FillCellsDetailDataCoroutine(coord));
		}
	}

	public void FillCellsDetailDataThread(object parameter)
	{
		try
		{
			Vector4 obj = (Vector4)parameter;
			int num = (int)obj.x;
			int num2 = (int)obj.y;
			int num3 = (int)obj.z;
			int num4 = (int)obj.w;
			int num5 = (threadHeightResolution - 1) / spData.cellRowAndCollumnCountPerTerrain + 1;
			_E4BE cell = null;
			for (int i = num2; i < num4; i++)
			{
				for (int j = num; j < num3; j++)
				{
					int hash = _E4BE.CalculateHash(j, 0, i);
					spData.GetCell(hash, out cell);
					if (cell != null)
					{
						_E4C0 obj2 = (_E4C0)cell;
						obj2.heightMapData = threadHeightMapData.MirrorAndFlatten(obj2.coordX * (num5 - 1), obj2.coordZ * (num5 - 1), num5, num5);
						obj2.detailMapData = new List<int[]>();
						obj2.totalDetailCounts = new List<int>();
						for (int k = 0; k < threadDetailMapData.Count; k++)
						{
							int num6 = threadDetailMapData[k].GetLength(0) / spData.cellRowAndCollumnCountPerTerrain;
							int[] array = threadDetailMapData[k].MirrorAndFlatten(obj2.coordX * num6, obj2.coordZ * num6, num6, num6);
							obj2.detailMapData.Add(array);
							int item = array.Sum();
							obj2.totalDetailCounts.Add(item);
						}
						continue;
					}
					throw new Exception(_ED3E._E000(115398));
				}
			}
			threadQueue.Enqueue(FillCellsDetailDataCallBack);
		}
		catch (Exception ex)
		{
			threadException = ex;
			threadQueue.Enqueue(base.LogThreadException);
		}
	}

	public IEnumerator FillCellsDetailDataCoroutine(Vector4 coord)
	{
		int num = (int)coord.x;
		int num2 = (int)coord.y;
		int num3 = (int)coord.z;
		int num4 = (int)coord.w;
		int num5 = (threadHeightResolution - 1) / spData.cellRowAndCollumnCountPerTerrain + 1;
		_E4BE cell = null;
		for (int i = num2; i < num4; i++)
		{
			for (int j = num; j < num3; j++)
			{
				int hash = _E4BE.CalculateHash(j, 0, i);
				spData.GetCell(hash, out cell);
				if (cell != null)
				{
					_E4C0 obj = (_E4C0)cell;
					obj.heightMapData = threadHeightMapData.MirrorAndFlatten(obj.coordX * (num5 - 1), obj.coordZ * (num5 - 1), num5, num5);
					obj.detailMapData = new List<int[]>();
					obj.totalDetailCounts = new List<int>();
					for (int k = 0; k < threadDetailMapData.Count; k++)
					{
						int num6 = threadDetailMapData[k].GetLength(0) / spData.cellRowAndCollumnCountPerTerrain;
						int[] array = threadDetailMapData[k].MirrorAndFlatten(obj.coordX * num6, obj.coordZ * num6, num6, num6);
						obj.detailMapData.Add(array);
						int item = array.Sum();
						obj.totalDetailCounts.Add(item);
					}
					yield return null;
				}
				else
				{
					Debug.LogError(_ED3E._E000(115398));
				}
			}
		}
		FillCellsDetailDataCallBack();
	}

	public void SetDetailMapData(List<int[,]> detailMapData)
	{
		threadDetailMapData = detailMapData;
	}

	public int[,] GetDetailLayer(int layer)
	{
		if (!isInitialized || base.terrain == null || spData == null)
		{
			return null;
		}
		int detailResolution = base.terrain.terrainData.detailResolution;
		int num = detailResolution / spData.cellRowAndCollumnCountPerTerrain;
		int[,] array = new int[detailResolution, detailResolution];
		for (int i = 0; i < spData.cellRowAndCollumnCountPerTerrain; i++)
		{
			for (int j = 0; j < spData.cellRowAndCollumnCountPerTerrain; j++)
			{
				if (!spData.GetCell(_E4BE.CalculateHash(i, 0, j), out var cell))
				{
					continue;
				}
				_E4C0 obj = (_E4C0)cell;
				if (obj.detailMapData == null)
				{
					continue;
				}
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < num; l++)
					{
						array[l + j * num, k + i * num] = obj.detailMapData[layer][k + l * num];
					}
				}
			}
		}
		return array;
	}

	public List<int[,]> GetDetailMapData()
	{
		if (!isInitialized || base.terrain == null || spData == null)
		{
			return null;
		}
		List<int[,]> list = new List<int[,]>();
		for (int i = 0; i < prototypeList.Count; i++)
		{
			list.Add(GetDetailLayer(i));
		}
		return list;
	}

	private int[,] _E00E(int[] array)
	{
		if (array == null || array.Length == 0)
		{
			return new int[0, 0];
		}
		int num = Mathf.FloorToInt(Mathf.Sqrt(array.Length));
		int[,] array2 = new int[num, num];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array2[i, j] = array[i * num + j];
			}
		}
		return array2;
	}

	private int[] _E00F(int[,] array)
	{
		int length = array.GetLength(0);
		int[] array2 = new int[length * length];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length; j++)
			{
				array2[i * length + j] = array[i, j];
			}
		}
		return array2;
	}
}
