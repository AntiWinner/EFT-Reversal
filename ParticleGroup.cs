using UnityEngine;

public class ParticleGroup : MonoBehaviour
{
	private Vector3[] m__E000;

	private Vector3[] m__E001;

	private float[] _E002;

	private MeshRenderer _E003;

	private MeshFilter _E004;

	private void Awake()
	{
		_E000();
	}

	private void OnDrawGizmos()
	{
		_E000();
	}

	private void _E000()
	{
		if (_E003 == null)
		{
			_E003 = GetComponent<MeshRenderer>() ?? base.gameObject.AddComponent<MeshRenderer>();
		}
		if (_E004 == null)
		{
			_E004 = GetComponent<MeshFilter>() ?? base.gameObject.AddComponent<MeshFilter>();
		}
		if (this.m__E000 == null || this.m__E000.Length != base.transform.childCount)
		{
			this.m__E000 = new Vector3[base.transform.childCount];
			this.m__E001 = new Vector3[base.transform.childCount];
			_E002 = new float[base.transform.childCount];
		}
		int num = 0;
		bool flag = false;
		foreach (Transform item in base.transform)
		{
			if (this.m__E000[num] != item.localPosition || this.m__E001[num] != item.localScale || _E002[num] != item.eulerAngles.x)
			{
				this.m__E000[num] = item.localPosition;
				this.m__E001[num] = item.localScale;
				_E002[num] = item.eulerAngles.x;
				flag = true;
			}
			num++;
		}
		if (flag)
		{
			_E004.mesh = _E001();
		}
	}

	private Mesh _E001()
	{
		int num = this.m__E000.Length;
		int num2 = num << 2;
		Vector3[] array = new Vector3[num2];
		Vector2[] array2 = new Vector2[num2];
		Vector2[] array3 = new Vector2[num2];
		int[] array4 = new int[num * 6];
		int i = 0;
		int num3 = 0;
		for (; i < num; i++)
		{
			int num4 = i << 2;
			array4[num3++] = num4;
			array4[num3++] = num4 + 1;
			array4[num3++] = num4 + 2;
			array4[num3++] = num4 + 2;
			array4[num3++] = num4 + 3;
			array4[num3++] = num4;
		}
		int j = 0;
		int num5 = 0;
		for (; j < num; j++)
		{
			int num6 = num5++;
			int num7 = num5++;
			int num8 = num5++;
			int num9 = num5++;
			array2[num6] = new Vector2(0f, 1f);
			array2[num7] = new Vector2(0f, 0f);
			array2[num8] = new Vector2(1f, 0f);
			array2[num9] = new Vector2(1f, 1f);
			Vector2 vector = this.m__E001[j];
			float f = _E002[j];
			float num10 = Mathf.Cos(f);
			float num11 = Mathf.Sin(f);
			float num12 = vector.x * num10;
			float num13 = vector.y * (0f - num11);
			float num14 = vector.x * num11;
			float num15 = vector.y * num10;
			array3[num6] = new Vector2(0f - num12 - num13, 0f - num14 - num15);
			array3[num7] = new Vector2(0f - num12 + num13, 0f - num14 + num15);
			array3[num8] = new Vector2(num12 + num13, num14 + num15);
			array3[num9] = new Vector2(num12 - num13, num14 - num15);
			array[num6] = (array[num7] = (array[num8] = (array[num9] = this.m__E000[j])));
		}
		return new Mesh
		{
			vertices = array,
			uv = array2,
			uv2 = array3,
			triangles = array4,
			name = _ED3E._E000(96514)
		};
	}
}
