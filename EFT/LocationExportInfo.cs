using System;
using UnityEngine;

namespace EFT;

public sealed class LocationExportInfo : MonoBehaviour
{
	[SerializeField]
	public long _unixDateTime;

	public DateTime DateTime
	{
		get
		{
			return _E5AD.LocalDateTimeFromUnixTime(_unixDateTime);
		}
		set
		{
			_unixDateTime = (long)value.ToUniversalTime().ToUnixTime();
		}
	}
}
