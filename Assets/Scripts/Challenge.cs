﻿using System.Collections;
using System.Collections.Generic;
using pkm.EventManager;
using UnityEngine;

public enum ChallengeType {
	Kill,
	Chain
}

public abstract class Challenge : ScriptableObject {
	public ChallengeType type;
	public string name = "Default name";
	public int goal;
	public int currentValue = 0;

	public abstract void Initialize();

	protected void UpdateChallenge(dynamic obj) {
		currentValue += obj.valueToAdd;
		Debug.Log("Challenge update : " + currentValue);
		EventManager.TriggerEvent("OnChallengeUpdate", new { challenge = this });
		if (currentValue >= goal) {
			OnComplete();
		}
	}

	private void OnComplete() {
		EventManager.TriggerEvent("OnChallengeComplete", new{challenge = this});
	}

	public abstract void Clean();
}

public class KillChallenge : Challenge {
	public override void Initialize() {
		EventManager.StartListening("OnEnemyKill", UpdateChallenge);
	}

	public override void Clean() {
		EventManager.StopListening("OnEnemyKill", UpdateChallenge);
	}
}

public class ChainChallenge : Challenge {
	public override void Initialize() {
		EventManager.StartListening("OnChainEnd", UpdateChallenge);
	}

	public override void Clean() {
		EventManager.StopListening("OnChainEnd", UpdateChallenge);
	}
}