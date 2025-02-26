﻿using System.IO;
using UnityEngine;

public class Flower : StandEnemy
{
	[SerializeField] Shooter shooter;
	[SerializeField] EnemyAimer aimer;
	[SerializeField] private Animator anim;

	float lastShootTime;

	protected new void Awake()
	{
		base.Awake();
		anim = GetComponent<Animator>();

		if (shooter == null)
			shooter = GetComponentInChildren<Shooter>();
		if (aimer == null)
			aimer = GetComponentInChildren<EnemyAimer>();
	}

	protected void Update()
	{
		if (aimer.Target != null)
		{
			aimer.FollowTarget();
			if (Time.time - lastShootTime >= (1 / attackSpeed))
			{
				Invoke("AttackAnim", 0.8f);
				lastShootTime = Time.time;
				shooter.Shoot(new DamageReport(damage, this));
			}
		}
	}
	void AttackAnim(){
		anim.SetTrigger("attack");
	}
	protected new void FixedUpdate()
	{
		base.FixedUpdate();
		if (aimer.Target == null)
			aimer.Aim();
		else if (!aimer.IsVisible())
			aimer.ResetTarget();
		
	}
	protected override void Death(Entity killer)
	{
		Debug.Log("------------Monster Dead-------------");
		shooter.Dispose();
		// 확률에따라 스킬구슬, 장비아이템, 꽝 3가지 중에서 랜덤 실행

		base.Death(killer);
		DropItem(getSkill); // 스킬아이템 드랍
	}
}
