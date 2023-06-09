using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VoxelAmbientTest : MonoBehaviour
{
	private class _E000
	{
		private int m__E000;

		private int m__E001;

		private int _E002;

		private int _E003;

		public _E000(int pixelsX, int pixelsY, int pixelsZ)
		{
			this.m__E000 = pixelsX;
			this.m__E001 = pixelsY;
			_E002 = pixelsZ;
			_E003 = this.m__E000 * this.m__E001;
		}

		public bool[] GetObstacles(Transform gridPosition)
		{
			bool[] array = new bool[this.m__E000 * this.m__E001 * _E002];
			Vector3 b = new Vector3(1f / (float)this.m__E000, 1f / (float)this.m__E001, 1f / (float)_E002);
			Vector3 lossyScale = gridPosition.lossyScale;
			Vector3 halfExtents = new Vector3(lossyScale.x / (float)this.m__E000, lossyScale.y / (float)this.m__E001, lossyScale.z / (float)_E002) * 0.5f;
			Quaternion rotation = gridPosition.rotation;
			int i = 0;
			int num = 0;
			for (; i < _E002; i++)
			{
				for (int j = 0; j < this.m__E001; j++)
				{
					int num2 = 0;
					while (num2 < this.m__E000)
					{
						Vector3 position = Vector3.Scale(new Vector3((float)num2 + 0.5f, (float)j + 0.5f, (float)i + 0.5f), b);
						Vector3 center = gridPosition.TransformPoint(position);
						array[num] = Physics.CheckBox(center, halfExtents, rotation);
						num2++;
						num++;
					}
				}
			}
			return array;
		}

		public LinkedList<int> GetSunLight(bool[] obstacles)
		{
			int x = this.m__E000 - 1;
			int num = this.m__E001 - 1;
			int z = _E002 - 1;
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < this.m__E000; i++)
			{
				for (int j = 0; j < _E002; j++)
				{
					for (int num2 = num; num2 >= 0; num2--)
					{
						int num3 = _E000(i, num2, j);
						if (obstacles[num3])
						{
							break;
						}
						hashSet.Add(num3);
					}
				}
			}
			for (int k = 0; k < this.m__E000; k++)
			{
				for (int l = 0; l < this.m__E001; l++)
				{
					int num4 = _E000(k, l, 0);
					int num5 = _E000(k, l, z);
					if (!obstacles[num4])
					{
						hashSet.Add(num4);
					}
					if (!obstacles[num5])
					{
						hashSet.Add(num5);
					}
				}
			}
			for (int m = 0; m < this.m__E000; m++)
			{
				for (int n = 0; n < _E002; n++)
				{
					int num6 = _E000(m, 0, n);
					int num7 = _E000(m, num, n);
					if (!obstacles[num6])
					{
						hashSet.Add(num6);
					}
					if (!obstacles[num7])
					{
						hashSet.Add(num7);
					}
				}
			}
			for (int num8 = 0; num8 < this.m__E001; num8++)
			{
				for (int num9 = 0; num9 < _E002; num9++)
				{
					int num10 = _E000(0, num8, num9);
					int num11 = _E000(x, num8, num9);
					if (!obstacles[num10])
					{
						hashSet.Add(num10);
					}
					if (!obstacles[num11])
					{
						hashSet.Add(num11);
					}
				}
			}
			return new LinkedList<int>(hashSet);
		}

		public float[] GetLight(bool[] obstacles, LinkedList<int> lights, int[][] axes, float[] intensities)
		{
			int num = this.m__E000 - 1;
			int num2 = this.m__E001 - 1;
			int num3 = _E002 - 1;
			float[] array = new float[obstacles.Length];
			bool[] array2 = new bool[obstacles.Length];
			foreach (int light in lights)
			{
				array[light] = 1f;
				array2[light] = true;
			}
			List<int> list = new List<int>(26);
			for (LinkedListNode<int> first = lights.First; first != null; first = lights.First)
			{
				int value = first.Value;
				lights.Remove(first);
				_E001(value, out var x, out var y, out var z);
				int num4 = ((x == 0) ? (-1) : ((x == num) ? 1 : 666));
				int num5 = ((y == 0) ? (-1) : ((y == num2) ? 1 : 666));
				int num6 = ((z == 0) ? (-1) : ((z == num3) ? 1 : 666));
				float num7 = 0f;
				list.Clear();
				for (int i = 0; i < 26; i++)
				{
					int[] array3 = axes[i];
					if (array3[0] == num4 || array3[1] == num5 || array3[2] == num6)
					{
						continue;
					}
					int num8 = _E000(x + array3[0], y + array3[1], z + array3[2]);
					if (!obstacles[num8])
					{
						if (!array2[num8])
						{
							array2[num8] = true;
							list.Add(num8);
						}
						else
						{
							num7 = Math.Max(num7, array[num8] * (1f - intensities[i]));
						}
					}
				}
				if (num7 > 0.0001f)
				{
					foreach (int item in list)
					{
						lights.AddLast(item);
					}
					array[value] = num7;
				}
			}
			return array;
		}

		public Color[] GetDirected(float[] array, int[][] axes, Vector3[] vectors, float NormalContrast)
		{
			Color[] array2 = new Color[this.m__E000 * this.m__E001 * _E002];
			int num = this.m__E000 - 1;
			int num2 = this.m__E001 - 1;
			int num3 = _E002 - 1;
			for (int i = 0; i < array.Length; i++)
			{
				_E001(i, out var x, out var y, out var z);
				int num4 = ((x == 0) ? (-1) : ((x == num) ? 1 : 666));
				int num5 = ((y == 0) ? (-1) : ((y == num2) ? 1 : 666));
				int num6 = ((z == 0) ? (-1) : ((z == num3) ? 1 : 666));
				float num7 = array[i];
				int num8 = 0;
				Vector3 vector = default(Vector3);
				for (int j = 0; j < 26; j++)
				{
					int[] array3 = axes[j];
					if (array3[0] != num4 && array3[1] != num5 && array3[2] != num6)
					{
						int num9 = _E000(x + array3[0], y + array3[1], z + array3[2]);
						float num10 = array[num9] - num7;
						num10 = ((!(num10 > 0f)) ? (0f - Mathf.Sqrt(0f - num10)) : Mathf.Sqrt(num10));
						vector += vectors[j] * num10;
						num8++;
					}
				}
				vector /= (float)num8;
				vector *= NormalContrast;
				array2[i] = new Color(vector.x * 0.5f + 0.5f, vector.y * 0.5f + 0.5f, vector.z * 0.5f + 0.5f, num7);
			}
			return array2;
		}

		public Color[] BlurImage(Color[] pixels, int size)
		{
			if (size < 1)
			{
				return pixels;
			}
			Color[] array = new Color[this.m__E000 * this.m__E001 * _E002];
			int i = 0;
			int num = 0;
			for (; i < _E002; i++)
			{
				for (int j = 0; j < this.m__E001; j++)
				{
					int num2 = 0;
					while (num2 < this.m__E000)
					{
						Color color = pixels[num];
						int num3 = 1;
						for (int k = 1; k < size; k++)
						{
							int num4 = num2 + k;
							if (num4 >= this.m__E000)
							{
								break;
							}
							color += pixels[_E000(num4, j, i)];
							num3++;
						}
						for (int l = 1; l < size; l++)
						{
							int num5 = num2 - l;
							if (num5 < 0)
							{
								break;
							}
							color += pixels[_E000(num5, j, i)];
							num3++;
						}
						array[num] = color / num3;
						num2++;
						num++;
					}
				}
			}
			int m = 0;
			int num6 = 0;
			for (; m < _E002; m++)
			{
				for (int n = 0; n < this.m__E001; n++)
				{
					int num7 = 0;
					while (num7 < this.m__E000)
					{
						Color color2 = array[num6];
						int num8 = 1;
						for (int num9 = 1; num9 < size; num9++)
						{
							int num10 = n + num9;
							if (num10 >= this.m__E001)
							{
								break;
							}
							color2 += array[_E000(num7, num10, m)];
							num8++;
						}
						for (int num11 = 1; num11 < size; num11++)
						{
							int num12 = n - num11;
							if (num12 < 0)
							{
								break;
							}
							color2 += array[_E000(num7, num12, m)];
							num8++;
						}
						pixels[num6] = color2 / num8;
						num7++;
						num6++;
					}
				}
			}
			int num13 = 0;
			int num14 = 0;
			for (; num13 < _E002; num13++)
			{
				for (int num15 = 0; num15 < this.m__E001; num15++)
				{
					int num16 = 0;
					while (num16 < this.m__E000)
					{
						Color color3 = pixels[num14];
						int num17 = 1;
						for (int num18 = 1; num18 < size; num18++)
						{
							int num19 = num13 + num18;
							if (num19 >= _E002)
							{
								break;
							}
							color3 += pixels[_E000(num16, num15, num19)];
							num17++;
						}
						for (int num20 = 1; num20 < size; num20++)
						{
							int num21 = num13 - num20;
							if (num21 < 0)
							{
								break;
							}
							color3 += pixels[_E000(num16, num15, num21)];
							num17++;
						}
						array[num14] = color3 / num17;
						num16++;
						num14++;
					}
				}
			}
			return array;
		}

		public Color[] Downsample(Color[] pixels, int samples, out _E000 im)
		{
			im = new _E000(this.m__E000 >> samples, this.m__E001 >> samples, _E002 >> samples);
			if (samples < 1)
			{
				return pixels;
			}
			Color[] array = new Color[im.m__E000 * im.m__E001 * im._E002];
			int i = 0;
			int num = 0;
			for (; i < _E002; i++)
			{
				for (int j = 0; j < this.m__E001; j++)
				{
					int num2 = 0;
					while (num2 < this.m__E000)
					{
						int num3 = im._E000(num2 >> samples, j >> samples, i >> samples);
						Color color = array[num3];
						color += pixels[num];
						array[num3] = color;
						num2++;
						num++;
					}
				}
			}
			float num4 = 1f / Mathf.Pow(1 << samples, 3f);
			for (int k = 0; k < array.Length; k++)
			{
				array[k] *= num4;
			}
			return array;
		}

		private int _E000(int x, int y, int z)
		{
			return x + this.m__E000 * y + _E003 * z;
		}

		private void _E001(int id, out int x, out int y, out int z)
		{
			z = Math.DivRem(id, _E003, out var result);
			y = Math.DivRem(result, this.m__E000, out result);
			x = result;
		}

		public Texture3D GetTex(Color[] pixels)
		{
			Texture3D texture3D = new Texture3D(this.m__E000, this.m__E001, _E002, TextureFormat.ARGB32, mipChain: false);
			texture3D.SetPixels(pixels);
			texture3D.Apply(updateMipmaps: false);
			return texture3D;
		}
	}

	public int PixelsX;

	public int PixelsY;

	public int PixelsZ;

	[Space]
	public int Upsample;

	[Space]
	public float LightRange = 1f;

	public float NormalsContrast = 1f;

	public float WallLightness;

	[Space]
	public Transform GridPosition;

	public Material Material;

	public Texture3D Tex;

	public Vector4 DebugVec;

	public bool DebugDrawGrid;

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(19728));

	private void OnEnable()
	{
		_E000 obj = new _E000(PixelsX << Upsample, PixelsY << Upsample, PixelsZ << Upsample);
		bool[] obstacles = obj.GetObstacles(GridPosition);
		LinkedList<int> sunLight = obj.GetSunLight(obstacles);
		_E001(LightRange, out var axes, out var intensities, out var vectors);
		float[] light = obj.GetLight(obstacles, sunLight, axes, intensities);
		Color[] directed = obj.GetDirected(light, axes, vectors, NormalsContrast);
		for (int i = 0; i < obstacles.Length; i++)
		{
			if (obstacles[i])
			{
				directed[i].a = WallLightness;
			}
		}
		directed = obj.Downsample(directed, Upsample, out var im);
		Texture3D tex = im.GetTex(directed);
		if (Material != null)
		{
			Material.SetTexture(VoxelAmbientTest.m__E000, tex);
		}
		Tex = tex;
	}

	private void OnValidate()
	{
	}

	private void OnDrawGizmosSelected()
	{
		if (DebugDrawGrid)
		{
			Gizmos.matrix = GridPosition.localToWorldMatrix;
			Vector3 vector = new Vector3(1f / (float)PixelsX, 1f / (float)PixelsY, 1f / (float)PixelsZ);
			Vector3 vector2 = vector * 0.5f;
			for (int i = 0; i < PixelsX; i += 2)
			{
				Gizmos.DrawWireCube(new Vector3((float)i * vector.x + vector2.x, 0.5f, 0.5f), new Vector3(vector.x, 1f, 1f));
			}
			for (int j = 0; j < PixelsY; j += 2)
			{
				Gizmos.DrawWireCube(new Vector3(0.5f, (float)j * vector.y + vector2.y, 0.5f), new Vector3(1f, vector.y, 1f));
			}
			for (int k = 0; k < PixelsZ; k += 2)
			{
				Gizmos.DrawWireCube(new Vector3(0.5f, 0.5f, (float)k * vector.z + vector2.z), new Vector3(1f, 1f, vector.z));
			}
		}
	}

	private bool[] _E000()
	{
		if (GridPosition == null)
		{
			return null;
		}
		bool[] array = new bool[PixelsX * PixelsY * PixelsZ];
		Vector3 b = new Vector3(1f / (float)PixelsX, 1f / (float)PixelsY, 1f / (float)PixelsZ);
		Vector3 lossyScale = GridPosition.lossyScale;
		Vector3 halfExtents = new Vector3(lossyScale.x / (float)PixelsX, lossyScale.y / (float)PixelsY, lossyScale.z / (float)PixelsZ) * 0.5f;
		Quaternion rotation = GridPosition.rotation;
		int i = 0;
		int num = 0;
		for (; i < PixelsZ; i++)
		{
			for (int j = 0; j < PixelsY; j++)
			{
				int num2 = 0;
				while (num2 < PixelsX)
				{
					Vector3 position = Vector3.Scale(new Vector3((float)num2 + 0.5f, (float)j + 0.5f, (float)i + 0.5f), b);
					Vector3 center = GridPosition.TransformPoint(position);
					array[num] = Physics.CheckBox(center, halfExtents, rotation);
					num2++;
					num++;
				}
			}
		}
		return array;
	}

	private void _E001(float lightRange, out int[][] axes, out float[] intensities, out Vector3[] vectors)
	{
		axes = new int[26][];
		intensities = new float[26];
		vectors = new Vector3[26];
		if (GridPosition == null)
		{
			return;
		}
		Vector3 lossyScale = GridPosition.lossyScale;
		int i = -1;
		int num = 0;
		for (; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					if (i != 0 || j != 0 || k != 0)
					{
						axes[num] = new int[3] { i, j, k };
						Vector3 vector = Vector3.Scale(new Vector3(i, j, k), lossyScale);
						intensities[num] = vector.magnitude / lightRange;
						float magnitude = vector.magnitude;
						vectors[num] = vector / (magnitude * magnitude);
						num++;
					}
				}
			}
		}
	}
}
