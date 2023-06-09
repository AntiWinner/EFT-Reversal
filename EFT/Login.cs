using System;

namespace EFT;

[Serializable]
public sealed class Login
{
	public string email;

	public string password;

	public bool toggle;
}
