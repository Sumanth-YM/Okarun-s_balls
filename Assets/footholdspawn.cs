using UnityEngine;
using System.Collections;

public class footholdspawn : MonoBehaviour
{
    public GameObject obstaclePrefab1, obstaclePrefab2;
    public GameUIManager gameUIManager;
    public float gapSize = 3f;
    public float basespawnInterval = 3f; // Time between spawns
    private float camHeight;


    void Start()
    {
        camHeight = Camera.main.orthographicSize;

        if (gameUIManager == null)
            gameUIManager = FindFirstObjectByType<GameUIManager>();

        StartCoroutine(SpawnBottomObstacle());
    }
    IEnumerator SpawnBottomObstacle()
    {
        while (true)
        {
            float gapHalf = gapSize;
            float minY = -camHeight+0.5f;
            float maxY = -gapHalf;

            float y = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(-10f, y, 0f);

            GameObject selectedPrefab = (Random.value < 0.5f) ? obstaclePrefab1 : obstaclePrefab2;
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
            float adder=gameUIManager.score_duplicate / 10f;

            float maxWaitTime = 10f;
            float waitTime = Mathf.Min(Random.Range(basespawnInterval+adder, basespawnInterval+adder + 1f), maxWaitTime);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
