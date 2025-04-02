using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
	/// <summary>
	/// gmgajg
	/// </summary>
	/// <param name="hitInfo"></param>
	public HitResponse Hit(HitInfo hitInfo);
}
