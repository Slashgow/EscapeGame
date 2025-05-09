using PurrNet;
using Unity.Splines.Examples;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform spawnPoint;

    public void SpawnItem()
    {
        var itemInstance = Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        itemInstance.GetComponent<Rigidbody>().isKinematic = true;
    }
}
