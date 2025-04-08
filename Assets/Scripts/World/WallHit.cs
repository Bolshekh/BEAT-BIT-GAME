using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHit : MonoBehaviour, IHitable
{
	public HitResponse Hit(HitInfo hitInfo)
	{
        return HitResponse.Hit;
	}
}
