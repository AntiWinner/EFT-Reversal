using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu]
public sealed class EFTBackendSettings : ScriptableObject
{
	public enum EBackendLabel
	{
		Production,
		Stage,
		Development,
		Custom,
		Unknown
	}

	[Serializable]
	public struct BackendUrl : IComparable<BackendUrl>
	{
		public string Url;

		public EBackendLabel Label;

		public BackendUrl(string url, EBackendLabel label)
		{
			Url = url;
			Label = label;
		}

		public override string ToString()
		{
			return string.Format(_ED3E._E000(53797), _ED3E._E000(53840), Url, _ED3E._E000(53836), Label);
		}

		public bool Equals(BackendUrl other)
		{
			if (string.Equals(Url, other.Url, StringComparison.Ordinal))
			{
				if (Label != EBackendLabel.Unknown && other.Label != EBackendLabel.Unknown)
				{
					return Label == other.Label;
				}
				return true;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BackendUrl other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((Url != null) ? Url.GetHashCode() : 0) * 397) ^ (int)Label;
		}

		public int CompareTo(BackendUrl other)
		{
			int num = Label.CompareTo(other.Label);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(Url, other.Url, StringComparison.Ordinal);
		}
	}

	private const string _E000 = "EFTBackendURLs";

	private const string _E001 = "EscapeFromTarkov/selectedBackendUrl";

	[SerializeField]
	public BackendUrl[] AvailableUrls = new BackendUrl[1]
	{
		new BackendUrl(_ED3E._E000(8325), EBackendLabel.Development)
	};

	[CompilerGenerated]
	private static readonly EFTBackendSettings _E002;

	[CompilerGenerated]
	private string _E003;

	private string _E004 => Application.dataPath + _ED3E._E000(53755);

	private string _E005 => Application.dataPath + _ED3E._E000(53779);

	public static EFTBackendSettings Instance
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public string SelectedBackendUrl
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	public void Reset()
	{
		AvailableUrls = new BackendUrl[1]
		{
			new BackendUrl(_ED3E._E000(8325), EBackendLabel.Development)
		};
		SelectedBackendUrl = AvailableUrls[0].Url;
	}
}
