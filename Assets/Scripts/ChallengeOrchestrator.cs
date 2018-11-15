using System.Collections;
using System.Collections.Generic;
using pkm.EventManager;
using UnityEngine;

public class ChallengeOrchestrator : MonoBehaviour {

	public Challenge debugChall;

	public List<Challenge> currentChallenges = new List<Challenge>();
	
	void Start () {
		EventManager.StartListening("OnChallengeComplete", OnChallengeComplete);
		EventManager.StartListening("OnChallengeUpdate", OnChallengeUpdate);
        if(debugChall != null)
        {
            AddChallenge(debugChall);
        }
	}

	private void OnChallengeUpdate(dynamic obj) {
		Challenge chall = currentChallenges.Find(e => e == obj.challenge);
		Debug.Log("Challenge update : " + chall.currentValue + " / " + chall.goal);
	}

	private void OnChallengeComplete(dynamic obj) {
		Challenge chall = currentChallenges.Find(e => e == obj.challenge);
		Debug.Log("Challenge complete : " + chall.name);
		chall.Clean();
		currentChallenges.Remove(chall);
	}

	public void AddChallenge(Challenge challenge) {
		Challenge chall = Instantiate(challenge);
		chall.Initialize();
		currentChallenges.Add(chall);
	}
}
