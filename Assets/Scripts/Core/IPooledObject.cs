using UnityEngine;

public interface IPooledObject
{
        void OnObjectSpawn(IObjectPooler newPooler);
}

public interface IObjectPooler
{
        void ReturnToPool(GameObject gameObject);
}