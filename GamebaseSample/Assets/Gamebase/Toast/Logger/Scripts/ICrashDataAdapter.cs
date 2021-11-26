using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
	public interface ICrashDataAdapter
	{
		Dictionary<string, string> GetUserFields();
	}
}