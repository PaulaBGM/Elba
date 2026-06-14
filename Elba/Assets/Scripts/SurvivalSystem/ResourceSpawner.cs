using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [SerializeField]
    private float respawnTime = 300f;

    private GameObject currentNode;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        currentNode = Instantiate(prefab,transform.position,Quaternion.identity);

        StartCoroutine(CheckDestroyed());
    }

    private IEnumerator CheckDestroyed()
    {
        yield return new WaitUntil(() => currentNode == null);

        yield return new WaitForSeconds(respawnTime);

        Spawn();
    }
}