using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class Canon : MonoBehaviour
{
    [SerializeField] private GameObject _canonBallPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _forceMultiplier;

    private void Update()
    {
        // TODO: ObjectPool for Instantiation and reusability of objects AND cooldown for shooting.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var canonBall = Instantiate(_canonBallPrefab, _spawnPoint.position, Quaternion.identity);
            canonBall.GetOrAddComponent<CanonBall>();
            canonBall.GetComponent<Rigidbody>().AddForce(_spawnPoint.forward * _forceMultiplier, ForceMode.Impulse);
            Destroy(canonBall, 3f);
        }
    }

    
}
