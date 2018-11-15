using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class KillChallenge : Challenge
{
    public override void Initialize()
    {
        EventManager.StartListening("OnEnemyKill", UpdateChallenge);
    }

    public override void Clean()
    {
        EventManager.StopListening("OnEnemyKill", UpdateChallenge);
    }
}