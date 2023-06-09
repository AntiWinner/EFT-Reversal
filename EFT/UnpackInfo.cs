using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public sealed class UnpackInfo
{
	[JsonProperty("id")]
	public string Id;

	public UnpackInfo(string id)
	{
		Id = id;
	}
}
