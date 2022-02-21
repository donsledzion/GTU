using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserbySpawner : MonoBehaviour
{
    [SerializeField] GameObject passerbyPrefab;
    [SerializeField] Transform[] targetsPool;

    [SerializeField] string[] spawnSurfaceTags;

    [SerializeField] List<GameObject> spawnedInstances = new List<GameObject>();
    [SerializeField] float offscreenTolerance = 1.5f;

    [SerializeField] Vector3 cameraView;
    [SerializeField] Vector3 spawnBaseRange;
    private Vector3 spawnBounds;
    private BoxCollider spawnCollider;


    [SerializeField] int targetsCountMin, targetsCountMax;
    [SerializeField] float speedMin, speedMax;
    GameObject passerbyInstance;
    [SerializeField] GameObject player;
    [SerializeField] FollowPlayer cameraFollower;
    // Start is called before the first frame update
    void Start()
    {
        spawnCollider = gameObject.GetComponent<BoxCollider>();
        InvokeRepeating("SpawnPasserby", 1f, 5f);
        InvokeRepeating("DisposeRunaways", 10f, 1f);
    }

    private void Update()
    {
        spawnBounds = spawnBaseRange + new Vector3(cameraFollower.cameraOffset.y - 15f, 0, cameraFollower.cameraOffset.y - 15f);
        spawnCollider.center = player.transform.position;
        spawnCollider.size = new Vector3(spawnBounds.x,5f,spawnBounds.z);
    }

    public void SpawnPasserby()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(player.transform.position.x - spawnBounds.x / 2, player.transform.position.x + spawnBounds.x / 2),
            Random.Range(player.transform.position.y - spawnBounds.y / 2, player.transform.position.y + spawnBounds.y / 2),
            Random.Range(player.transform.position.z - spawnBounds.z / 2, player.transform.position.z + spawnBounds.z / 2)
            );
        Quaternion spawnRot = Quaternion.Euler(0,Random.Range(0,360),0);

        passerbyInstance = Instantiate(passerbyPrefab, spawnPos, spawnRot);
        AddTargets();
        spawnedInstances.Add(passerbyInstance);
    }

    void AddTargets()
    {
        Passerby passerby = passerbyInstance.GetComponent<Passerby>();
        passerby.targetSet = new Transform[Random.Range(targetsCountMin, targetsCountMax)];
        passerby.speedFactor = Random.Range(speedMin, speedMax);
        for (int i = 0; i < passerby.targetSet.Length; i++)
        {
            passerby.targetSet[i] = targetsPool[Random.Range(0, targetsPool.Length - 1)];
        }
    }

    bool IsOffScreen(GameObject spawnedInstance)
    {
        Vector3 instancePos = spawnedInstance.transform.position;
        float leftBound = player.transform.position.x - spawnBounds.x * offscreenTolerance;
        float rightBound = player.transform.position.x + spawnBounds.x * offscreenTolerance;

        float bottomBound = player.transform.position.z - spawnBounds.z * offscreenTolerance;
        float topBound = player.transform.position.z + spawnBounds.z * offscreenTolerance;

        if (instancePos.x < leftBound || instancePos.x > rightBound) return true;
        if (instancePos.z < bottomBound || instancePos.z > topBound) return true;

        return false;
    }

    void DisposeRunaways()
    {
        if(spawnedInstances.Count>0)
        {
            foreach(GameObject instance in spawnedInstances)
            {
                if (IsOffScreen(instance))
                {
                    spawnedInstances.Remove(instance);
                    Destroy(instance);
                    DisposeRunaways();
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(player.transform.position,spawnCollider.size);
    }
}
