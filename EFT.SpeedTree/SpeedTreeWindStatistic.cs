using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.SpeedTree;

public class SpeedTreeWindStatistic : MonoBehaviour
{
	[CompilerGenerated]
	private TreeWind.BaseTreeData m__E000;

	[CompilerGenerated]
	private TreeWind.BaseTreeData m__E001;

	[CompilerGenerated]
	private TreeWind.FactorTreeData m__E002;

	[CompilerGenerated]
	private TreeWind.FactorTreeData m__E003;

	private List<Vector4> m__E004 = new List<Vector4>();

	private List<Vector4> m__E005 = new List<Vector4>();

	private List<Vector4> _E006 = new List<Vector4>();

	private List<Vector4> _E007 = new List<Vector4>();

	private List<Vector4> _E008 = new List<Vector4>();

	private List<Vector4> _E009 = new List<Vector4>();

	private List<Vector4> _E00A = new List<Vector4>();

	private List<Vector4> _E00B = new List<Vector4>();

	private List<Vector4> _E00C = new List<Vector4>();

	private List<Vector4> _E00D = new List<Vector4>();

	private List<Vector4> _E00E = new List<Vector4>();

	private List<Vector4> _E00F = new List<Vector4>();

	private List<Vector4> _E010 = new List<Vector4>();

	private List<Vector4> _E011 = new List<Vector4>();

	private List<Vector4> _E012 = new List<Vector4>();

	private List<float> _E013 = new List<float>();

	private MaterialPropertyBlock _E014;

	private Renderer _E015;

	public TreeWind.BaseTreeData BaseMinWindData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public TreeWind.BaseTreeData BaseMaxWindData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public TreeWind.FactorTreeData MinWindData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public TreeWind.FactorTreeData MaxWindData
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	public void RecordTick()
	{
		if (_E015 == null)
		{
			_E015 = GetComponent<Renderer>();
			_E014 = new MaterialPropertyBlock();
		}
		_E015.GetPropertyBlock(_E014);
		_E002();
	}

	public void SaveAsMinWindData()
	{
		BaseMinWindData = _E000();
		MinWindData = _E001();
	}

	public void SaveAsMaxWindData()
	{
		BaseMaxWindData = _E000();
		MaxWindData = _E001();
	}

	private TreeWind.BaseTreeData _E000()
	{
		_E004();
		TreeWind.BaseTreeData baseTreeData = default(TreeWind.BaseTreeData);
		baseTreeData._ST_WindGlobal = this.m__E005.First();
		baseTreeData._ST_WindBranch = _E006.First();
		baseTreeData._ST_WindBranchTwitch = _E007.First();
		baseTreeData._ST_WindBranchWhip = _E008.First();
		baseTreeData._ST_WindBranchAnchor = _E009.First();
		baseTreeData._ST_WindBranchAdherences = _E00A.First();
		baseTreeData._ST_WindTurbulences = _E00B.First();
		baseTreeData._ST_WindLeaf1Ripple = _E00C.First();
		baseTreeData._ST_WindLeaf1Tumble = _E00D.First();
		baseTreeData._ST_WindLeaf1Twitch = _E00E.First();
		baseTreeData._ST_WindLeaf2Ripple = _E00F.First();
		baseTreeData._ST_WindLeaf2Tumble = _E010.First();
		baseTreeData._ST_WindLeaf2Twitch = _E011.First();
		baseTreeData._ST_WindFrondRipple = _E012.First();
		TreeWind.BaseTreeData result = baseTreeData;
		if (_E015 is BillboardRenderer)
		{
			result._ST_WindGlobal.x = 1f;
			result._ST_WindGlobal.y = 1f;
		}
		return result;
	}

	private TreeWind.FactorTreeData _E001()
	{
		_E004();
		TreeWind.FactorTreeData result = default(TreeWind.FactorTreeData);
		float num = _E013.Last() - _E013.First();
		Vector4 sT_WindGlobal_B = (this.m__E005.Last() - this.m__E005.First()) / num;
		result._ST_WindGlobal_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E006.Last() - _E006.First()) / num;
		result._ST_WindBranch_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E00C.Last() - _E00C.First()) / num;
		result._ST_WindLeaf1Ripple_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E00D.Last() - _E00D.First()) / num;
		result._ST_WindLeaf1Tumble_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E00E.Last() - _E00E.First()) / num;
		result._ST_WindLeaf1Twitch_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E00F.Last() - _E00F.First()) / num;
		result._ST_WindLeaf2Ripple_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E010.Last() - _E010.First()) / num;
		result._ST_WindLeaf2Tumble_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E011.Last() - _E011.First()) / num;
		result._ST_WindLeaf2Twitch_B = sT_WindGlobal_B;
		sT_WindGlobal_B = (_E012.Last() - _E012.First()) / num;
		result._ST_WindFrondRipple_B = sT_WindGlobal_B;
		return result;
	}

	private void _E002()
	{
		this.m__E004.Add(_E014.GetVector(_ED3E._E000(175287)));
		this.m__E005.Add(_E014.GetVector(_ED3E._E000(175270)));
		_E006.Add(_E014.GetVector(_ED3E._E000(175317)));
		_E007.Add(_E014.GetVector(_ED3E._E000(175300)));
		_E008.Add(_E014.GetVector(_ED3E._E000(175345)));
		_E009.Add(_E014.GetVector(_ED3E._E000(175388)));
		_E00A.Add(_E014.GetVector(_ED3E._E000(175369)));
		_E00B.Add(_E014.GetVector(_ED3E._E000(175410)));
		_E00C.Add(_E014.GetVector(_ED3E._E000(175454)));
		_E00D.Add(_E014.GetVector(_ED3E._E000(175434)));
		_E00E.Add(_E014.GetVector(_ED3E._E000(175478)));
		_E00F.Add(_E014.GetVector(_ED3E._E000(175458)));
		_E010.Add(_E014.GetVector(_ED3E._E000(175502)));
		_E011.Add(_E014.GetVector(_ED3E._E000(175546)));
		_E012.Add(_E014.GetVector(_ED3E._E000(175526)));
		_E013.Add(Time.time);
	}

	public void ExportToFile(string path)
	{
		StreamWriter streamWriter = new StreamWriter(path, append: true);
		streamWriter.Write(Environment.NewLine + Environment.NewLine + _ED3E._E000(175570) + base.gameObject.name);
		_E003(streamWriter, this.m__E004, _ED3E._E000(175287));
		_E003(streamWriter, this.m__E005, _ED3E._E000(175270));
		_E003(streamWriter, _E006, _ED3E._E000(175317));
		_E003(streamWriter, _E007, _ED3E._E000(175300));
		_E003(streamWriter, _E008, _ED3E._E000(175345));
		_E003(streamWriter, _E009, _ED3E._E000(175388));
		_E003(streamWriter, _E00A, _ED3E._E000(175369));
		_E003(streamWriter, _E00B, _ED3E._E000(175410));
		_E003(streamWriter, _E00C, _ED3E._E000(175454));
		_E003(streamWriter, _E00D, _ED3E._E000(175434));
		_E003(streamWriter, _E00E, _ED3E._E000(175478));
		_E003(streamWriter, _E00F, _ED3E._E000(175458));
		_E003(streamWriter, _E010, _ED3E._E000(175502));
		_E003(streamWriter, _E011, _ED3E._E000(175546));
		_E003(streamWriter, _E012, _ED3E._E000(175526));
		streamWriter.WriteLine(Environment.NewLine + base.gameObject.name + _ED3E._E000(175561));
		float num = _E013.Last() - _E013.First();
		Vector4 vector = (this.m__E004.Last() - this.m__E004.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175555) + vector);
		vector = (this.m__E005.Last() - this.m__E005.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175603) + vector);
		vector = (_E006.Last() - _E006.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175587) + vector);
		vector = (_E007.Last() - _E007.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175635) + vector);
		vector = (_E008.Last() - _E008.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175673) + vector);
		vector = (_E00A.Last() - _E00A.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175653) + vector);
		vector = (_E00B.Last() - _E00B.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175695) + vector);
		vector = (_E00C.Last() - _E00C.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175732) + vector);
		vector = (_E00D.Last() - _E00D.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175713) + vector);
		vector = (_E00E.Last() - _E00E.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175758) + vector);
		vector = (_E00F.Last() - _E00F.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175803) + vector);
		vector = (_E010.Last() - _E010.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175776) + vector);
		vector = (_E011.Last() - _E011.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175821) + vector);
		vector = (_E012.Last() - _E012.First()) / num;
		streamWriter.WriteLine(_ED3E._E000(175866) + vector);
		streamWriter.Close();
	}

	private void _E003(StreamWriter writer, List<Vector4> data, string varName)
	{
		writer.WriteLine(Environment.NewLine + varName + _ED3E._E000(30697));
		writer.Write(_ED3E._E000(175847));
		float a = 0f;
		for (int i = 0; i < _E013.Count; i++)
		{
			float x = data[i].x;
			if (!Mathf.Approximately(a, x))
			{
				a = x;
				writer.Write(string.Format(_ED3E._E000(175842), x));
			}
		}
		writer.Write(Environment.NewLine + _ED3E._E000(175896));
		a = 0f;
		for (int j = 0; j < _E013.Count; j++)
		{
			float y = data[j].y;
			if (!Mathf.Approximately(a, y))
			{
				a = y;
				writer.Write(string.Format(_ED3E._E000(175842), y));
			}
		}
		writer.Write(Environment.NewLine + _ED3E._E000(175899));
		a = 0f;
		for (int k = 0; k < _E013.Count; k++)
		{
			float z = data[k].z;
			if (!Mathf.Approximately(a, z))
			{
				a = z;
				writer.Write(string.Format(_ED3E._E000(175842), z));
			}
		}
		writer.Write(Environment.NewLine + _ED3E._E000(175894));
		a = 0f;
		for (int l = 0; l < _E013.Count; l++)
		{
			float w = data[l].w;
			if (!Mathf.Approximately(a, w))
			{
				a = w;
				writer.Write(string.Format(_ED3E._E000(175842), w));
			}
		}
	}

	private void _E004()
	{
		for (int i = 0; i < _E013.Count; i++)
		{
			this.m__E004[i] = _E005(this.m__E004[i]);
			this.m__E005[i] = _E005(this.m__E005[i]);
			_E006[i] = _E005(_E006[i]);
			_E007[i] = _E005(_E007[i]);
			_E008[i] = _E005(_E008[i]);
			_E009[i] = _E005(_E009[i]);
			_E00A[i] = _E005(_E00A[i]);
			_E00B[i] = _E005(_E00B[i]);
			_E00C[i] = _E005(_E00C[i]);
			_E00D[i] = _E005(_E00D[i]);
			_E00E[i] = _E005(_E00E[i]);
			_E00F[i] = _E005(_E00F[i]);
			_E010[i] = _E005(_E010[i]);
			_E011[i] = _E005(_E011[i]);
			_E012[i] = _E005(_E012[i]);
		}
	}

	private Vector4 _E005(Vector4 vec)
	{
		float num = 1E-05f;
		if (float.IsNaN(vec.x))
		{
			vec = new Vector4(num, vec.y, vec.z, vec.w);
			Debug.LogWarning(_ED3E._E000(175889));
		}
		if (float.IsNaN(vec.y))
		{
			vec = new Vector4(vec.x, num, vec.z, vec.w);
			Debug.LogWarning(_ED3E._E000(175889));
		}
		if (float.IsNaN(vec.z))
		{
			vec = new Vector4(vec.x, vec.y, num, vec.w);
			Debug.LogWarning(_ED3E._E000(175889));
		}
		if (float.IsNaN(vec.w))
		{
			vec = new Vector4(vec.x, vec.y, vec.z, num);
			Debug.LogWarning(_ED3E._E000(175889));
		}
		return vec;
	}

	public void ClearStatisticData()
	{
		this.m__E005.Clear();
		_E006.Clear();
		_E007.Clear();
		_E008.Clear();
		_E009.Clear();
		_E00A.Clear();
		_E00B.Clear();
		_E00C.Clear();
		_E00D.Clear();
		_E00E.Clear();
		_E00F.Clear();
		_E010.Clear();
		_E011.Clear();
		_E012.Clear();
		_E013.Clear();
	}
}
