using System;
using UnityEngine;

public class FarmerFactory : MonoBehaviour, IThemeFactory
{
    [SerializeField] private GameObject _farmerPrefab;
    //private float randXSpawn;
    //private float randZSpawn;

    //private void Start()
    //{
    //    randXSpawn = (float)UnityEngine.Random.Range(0, 20);
    //    randZSpawn = (float)UnityEngine.Random.Range(0, 20);
    //}
    public IEntity CreateEntity()
    {
        Instantiate(_farmerPrefab, gameObject.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        return null;
    }

    public IStateMachine CreateStateMachine()
    {
        Debug.Log("Farmer Factory");
        return null;
    }

    public ITask CreateTask()
    {
        Debug.Log("Farmer Factory");
        return null;
    }
}
