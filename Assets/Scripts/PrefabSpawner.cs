using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PrefabSpawner : MonoBehaviour {
    [FormerlySerializedAs("prefab")] public GameObject Prefab;
    [FormerlySerializedAs("spawnerDelay")] public float SpawnerDelay;

    private void Start()
    {
        StartCoroutine(D());
    }

    private IEnumerator D()
    {
        yield return new WaitForSeconds(SpawnerDelay);
        Instantiate(Prefab, transform.position, Quaternion.identity);
        StartCoroutine(D());
    }
}
