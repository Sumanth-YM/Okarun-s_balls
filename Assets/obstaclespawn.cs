using UnityEngine;
using System.Collections;

public class obstaclespawn : MonoBehaviour
{
    public GameObject obstaclePrefab1, obstaclePrefab2;
    public float gapSize = 3f;

    private float camHeight;

    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        StartCoroutine(SpawnTopObstacle());
    }

    IEnumerator SpawnTopObstacle()
    {
        while (true)
        {
            float gapHalf = gapSize ;
            float minY = gapHalf;
            float maxY = camHeight-1f;

            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(10f, y, 0f);

            GameObject selectedPrefab = (Random.value < 0.5f) ? obstaclePrefab1 : obstaclePrefab2;
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

            float waitTime = Random.Range(2f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
