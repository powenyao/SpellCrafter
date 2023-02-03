using UnityEngine;

public interface IPooledObject
{
        void OnObjectSpawn(IObjectPooler pooler);
}

public interface IObjectPooler
{
        void ReturnToPool(GameObject gameObject);
}