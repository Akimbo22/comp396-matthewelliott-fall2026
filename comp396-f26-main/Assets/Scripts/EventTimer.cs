using UnityEngine;

public class EventTimer : MonoBehaviour
{
    //[SerializeField] private VoidEventChannel _channel;
    [SerializeField] private VoidEventChannel _channelPos;
    [SerializeField] private Transform[] _harvestLocations;
    [SerializeField] private BoolEventChannel _boolUIEvent;
    private float timer = 0;
    private bool toggleUI = false;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 5f)
        {
            _channelPos.RaiseEvent();
            toggleUI = !toggleUI;
            _boolUIEvent.RaiseEvent(toggleUI);
            timer = 0;
        }
    }
}
