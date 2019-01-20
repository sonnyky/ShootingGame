using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : StateManager<Ship>
{

    private Ship targetObject;

    public Moving(Ship myObject) : base(myObject)
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

    private void AnnouncePosition()
    {

    }
}
