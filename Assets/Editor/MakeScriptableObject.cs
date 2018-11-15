using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject {
	[MenuItem("Assets/Create/Challenges/Kill challenge")]
	public static void CreateKillChallenge() {
		KillChallenge asset = ScriptableObject.CreateInstance<KillChallenge>();
		asset.type = ChallengeType.Kill;
		asset.name = "Default kill challenge";
		AssetDatabase.CreateAsset(asset, "Assets/Scriptables/KillChallenge.asset");

		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}

	[MenuItem("Assets/Create/Challenges/Chain challenge")]
	public static void CreateChainChallenge() {
		ChainChallenge asset = ScriptableObject.CreateInstance<ChainChallenge>();
		asset.type = ChallengeType.Chain;
		asset.name = "Default chain challenge";
		AssetDatabase.CreateAsset(asset, "Assets/Scriptables/ChainChallenge.asset");
		
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = asset;
	}
}