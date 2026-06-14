using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourceNode prefab;
    [SerializeField] private float respawnTime = 300f;

    private ResourceNode node;

    private void Start()
    {
        node = Instantiate(
            prefab,
            transform.position,
            Quaternion.identity);

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        while (true)
        {
            if (!node.gameObject.activeSelf)
            {
                yield return new WaitForSeconds(respawnTime);

                node.transform.position = transform.position;
                node.ResetNode();
                node.gameObject.SetActive(true);
            }

            yield return null;
        }
    }
}