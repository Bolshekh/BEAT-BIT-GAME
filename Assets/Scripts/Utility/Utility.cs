using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class IsExternalInit { }
}

public class EntityHealedEventArgs : EventArgs
{
	public float PointsHealed;
	/// <summary>
	/// If entity was in fact healed. If entity had max health he will recieve heal (true), but will not be getting any healing (false)
	/// </summary>
	public bool IsEntityHealed;
	public float HealthBefore { get; set; }
	public float HealthAfter { get; set; }
}

public class EntityHitEventArgs : EventArgs
{
	public EntityHitEventArgs(HitInfo HitInfo)
	{
		this.HitInfo = HitInfo;
	}

	public HitInfo HitInfo { get; init; }
	/// <summary>
	/// If entity hit event chould be calcelled due to something
	/// </summary>
	public bool IsCancelled { get; set; }
	public float HealthBefore { get; set; }
	public float HealthAfter { get; set; }
	public bool OverrideResponse { get; set; }
	public HitResponse OverridenResponse { get; set; }
}

public class WeaponHitEventArgs : EventArgs
{
	public IHitable Hit { get; init; }
	public Collider2D Collision { get; init; }
}
public class GameObjectEventArgs : EventArgs
{
	public GameObject Entity { get; init; }
}

public class HitInfo
{
	/// <summary>
	/// Gameobject that hits victim
	/// </summary>
	public GameObject Hitter { get; init; }
	public float Damage { get; init; }
	public Vector3 Knockback { get; set; }
}

[System.Serializable]
public class Cooldown
{
	[SerializeField] float cooldownTime;
	public float CooldownTime
	{
		set
		{
			cooldownTime = value;
		}
		get
		{
			return cooldownTime;
		}
	}
	float nextFireTime;

	public bool IsCoolingDown => Time.time < nextFireTime;
	public void StartCooldown() => nextFireTime = Time.time + cooldownTime;
}

[Flags]
public enum HitResponse
{
	Hit = 1,
	Ignore = 2,
	PassThrough = 4
}

public struct HelperAnimation
{
	public HelperAnimation(string StateName, float TransitionDuration = 0f, bool LockAnimation = false, bool InterruptableAnimation = false, bool Interruptor = false)
	{
		this.StateName = StateName;
		this.TransitionDuration = TransitionDuration = 0f;
		this.LockAnimation = LockAnimation = false;
		this.InterruptableAnimation = InterruptableAnimation = false;
		this.Interruptor = Interruptor = false;
	}
	public string StateName { get; set; }
	public float TransitionDuration { get; set; }
	public bool LockAnimation { get; set; }
	public bool InterruptableAnimation { get; set; }
	public bool Interruptor { get; set; }
}