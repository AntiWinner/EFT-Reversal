using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MuzzleFlash : MonoBehaviour
{
	public int Count = 2048;

	public int TileSheetRows = 1;

	public int TileSheetColumns = 1;

	private Vector2[][] _E000;

	private Vector3[] _E001;

	private Vector2[] _E002;

	private Vector2[] _E003;

	private int[] _E004;

	private Mesh _E005;

	private int _E006;

	private bool _E007;

	private int _E008;

	private _E8EE._E000 _E009;

	private void Awake()
	{
		int num = Count << 2;
		_E001 = new Vector3[num];
		_E002 = new Vector2[num];
		_E003 = new Vector2[num];
		_E004 = new int[Count * 6];
		int i = 0;
		int num2 = 0;
		for (; i < Count; i++)
		{
			int num3 = i << 2;
			_E004[num2++] = num3;
			_E004[num2++] = num3 + 1;
			_E004[num2++] = num3 + 2;
			_E004[num2++] = num3 + 2;
			_E004[num2++] = num3 + 3;
			_E004[num2++] = num3;
		}
		int j = 0;
		int num4 = 0;
		for (; j < Count; j++)
		{
			_E002[num4++] = new Vector2(0f, 0f);
			_E002[num4++] = new Vector2(0f, 1f);
			_E002[num4++] = new Vector2(1f, 1f);
			_E002[num4++] = new Vector2(1f, 0f);
		}
		_E006 = TileSheetRows * TileSheetColumns;
		_E007 = _E006 == 1;
		if (!_E007)
		{
			_E009 = new _E8EE._E000(_E006);
			_E000 = new Vector2[_E006][];
			float num5 = 1f / (float)TileSheetRows;
			float num6 = 1f / (float)TileSheetColumns;
			int k = 0;
			int num7 = 0;
			int num8 = 0;
			for (; k < _E006; k++)
			{
				float x = num5 * (float)num7;
				float x2 = num5 * (float)(num7 + 1);
				float y = num6 * (float)num8;
				float y2 = num6 * (float)(num8 + 1);
				_E000[k] = new Vector2[4];
				_E000[k][0] = new Vector2(x, y);
				_E000[k][1] = new Vector2(x, y2);
				_E000[k][2] = new Vector2(x2, y2);
				_E000[k][3] = new Vector2(x2, y);
				num7++;
				if (num7 >= TileSheetRows)
				{
					num8++;
					num7 = 0;
				}
			}
		}
		_E005 = new Mesh
		{
			vertices = _E001,
			uv = _E002,
			uv2 = _E003,
			triangles = _E004,
			name = _ED3E._E000(88512)
		};
		_E005.bounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));
		GetComponent<MeshFilter>().mesh = _E005;
	}

	public void AddFlash(float radius, float durationTime, float addTime, bool randomRotation)
	{
		Vector3 vector;
		Vector3 vector2;
		if (randomRotation)
		{
			vector = Random.insideUnitCircle * radius;
			vector2 = new Vector2(vector.y, 0f - vector.x);
		}
		else
		{
			vector = new Vector2(radius, radius);
			vector2 = new Vector2(radius, 0f - radius);
		}
		int num = _E008 << 2;
		int num2 = num++;
		int num3 = num++;
		int num4 = num++;
		_E008 = (_E008 + 1) % Count;
		_E001[num2] = -vector;
		_E001[num3] = -vector2;
		_E001[num4] = vector;
		_E001[num] = vector2;
		if (_E007)
		{
			_E002[num2] = new Vector2(0f, 0f);
			_E002[num3] = new Vector2(0f, 1f);
			_E002[num4] = new Vector2(1f, 1f);
			_E002[num] = new Vector2(1f, 0f);
		}
		else
		{
			Vector2[] array = _E000[_E009.Get()];
			_E002[num2] = array[0];
			_E002[num3] = array[1];
			_E002[num4] = array[2];
			_E002[num] = array[3];
		}
		_E003[num2] = (_E003[num3] = (_E003[num4] = (_E003[num] = new Vector2(Time.time + durationTime + addTime, durationTime))));
		base.enabled = true;
	}

	private void LateUpdate()
	{
		_E005.vertices = _E001;
		_E005.uv = _E002;
		_E005.uv2 = _E003;
		base.enabled = false;
	}
}
