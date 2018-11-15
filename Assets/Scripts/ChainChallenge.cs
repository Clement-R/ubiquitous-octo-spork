using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class ChainChallenge : Challenge
{
    public override void Initialize()
    {
        EventManager.StartListening("OnChainEnd", UpdateChallenge);
    }

    public override void Clean()
    {
        EventManager.StopListening("OnChainEnd", UpdateChallenge);
    }
}