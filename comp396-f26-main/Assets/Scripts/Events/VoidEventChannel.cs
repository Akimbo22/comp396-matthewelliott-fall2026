using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Scriptable Objects/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public UnityAction OnEventRaised; // This event is subscribed by observers or listeners
    public void RaiseEvent() // This is called by anyone that needs to dispatch this event
    {
        OnEventRaised?.Invoke();
    }
}

public class PlayerEventChannel : GenericEventChannel<Player>
{

}

public class Player : MonoBehaviour
{
    public string _name;
    public int level;
    public float currHealth;
    public float maxHealth;
}