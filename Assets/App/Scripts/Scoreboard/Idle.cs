using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateManager<Scoreboard>
{
    // State when the scoreboard is updating and playing animation. No new updates are allowed during this state

    private Scoreboard targetObject;

    public Idle(Scoreboard myObject) : base(myObject)
    {
        targetObject = myObject;
    }

    public override void Tick()
    {

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
