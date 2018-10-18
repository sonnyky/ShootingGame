using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : StateManager<Ship> {

    private Ship targetObject;

    public Destroyed(Ship myObject) : base(myObject)
    {
        targetObject = myObject;
    }

    public override void Tick()
    {
      

    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        targetObject.Disable();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    private void AnnouncePosition()
    {

    }
}
