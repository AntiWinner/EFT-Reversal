using System;
using Newtonsoft.Json;

namespace JsonType;

[Serializable]
public struct KeepAliveResponse
{
	[JsonProperty("utc_time")]
	public double UtcTime;
}
