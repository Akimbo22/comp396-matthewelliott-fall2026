using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "BoolEvent", story: "[Event] received", category: "Conditions", id: "31809b331b5bc8642b111c1c8eebfebd")]
public partial class BoolEventCondition : Condition
{
    [SerializeReference] public BlackboardVariable<BoolEventChannel> Event;
    private bool status = false;

    public override bool IsTrue()
    {
        Debug.Log($"BoolEventCondition.IsTrue() -> {status}");
        return status;
    }

    public override void OnStart()
    {
        Debug.Log("BoolEventCondition.OnStart()");
        Event.Value.OnEventRaised += value => status = value;
    }

    public override void OnEnd()
    {
        Debug.Log("BoolEventCondition.OnEnd()");
        Event.Value.OnEventRaised -= value => status = value;
    }
}
