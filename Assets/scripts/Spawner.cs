using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour

{
    
    public GameObject Prefab;
    public float spawnInterval = 1f;
    public float spawnRangeY = 3f;
    public float spawnPositionX = 10f;
    private List<GameObject> spawnList = new List<GameObject>();
    public float speedMove = 10f;
    private Coroutine coroutine;
    // Start is called before the first frame update

    public bool isColumnStyle = false;
    public bool Level1 = false;
    private float gameTime = 0f;
    private float phaseStartTime = 0f;
   
    private void OnEnable()
    {
        GameManager.OnGameStarted += StarSpawning;
        TapController.OnPlayerDied += StopSpawningoject;
    }

    private void StarSpawning()
    {
        coroutine = StartCoroutine(Spawn());
    }
    private void StopSpawningoject()
    {
        if (coroutine != null)
        {
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Track time while Level1 is true
        if (Level1)
        {
            if (Level1)
            {
                gameTime += Time.deltaTime;

                
                if (gameTime > 15f && spawnInterval == 1f)
                {
                    spawnInterval = 0.5f; 
                    phaseStartTime = gameTime; 
                }

                
                if (gameTime > 30f && gameTime - phaseStartTime > 15f)
                {
                    spawnInterval = 0.5f; 
                    StartCoroutine(ToggleIsCollumn());
                    phaseStartTime = gameTime; 
                }
            }
            else
            {
                spawnInterval = 1f; 
                isColumnStyle = false;
            }
        }
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            if (isColumnStyle)
            {
                SpawnColumn();
            }
            else
            {
                SpawnCoin();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCoin()
    {
        Vector3 spawnPosition = new Vector3(spawnPositionX, Random.Range(-spawnRangeY, spawnRangeY), 0);
        GameObject prefabRef = Instantiate(Prefab, spawnPosition, Quaternion.identity);
        spawnList.Add(prefabRef);
        StartCoroutine(MoveCoin(prefabRef));

    }
    private void SpawnColumn()
    {
        int count = Random.Range(2, 6);
        float columnBaseHeight = Random.Range(-spawnRangeY, spawnRangeY);
        for (int i = 0; i < count; i++)
        {
            float yOffset = i * (Prefab.transform.localScale.y + 0.5f);
            Vector3 spawnPosition = new Vector3(spawnPositionX, columnBaseHeight + yOffset, 0);
            GameObject prefabRef = Instantiate(Prefab, spawnPosition, Quaternion.identity);
            spawnList.Add(prefabRef);
            StartCoroutine(MoveCoin(prefabRef));
        }
    }

    private IEnumerator MoveCoin(GameObject prefab)
    {
        while (prefab != null)
        {
            prefab.transform.position += Vector3.left * speedMove * Time.deltaTime;
            if (prefab != null && prefab.transform.position.x < -30f)
            {
                Destroy(prefab);
                spawnList.Remove(prefab);
                break;

            }
            yield return null;

        }
    }
    IEnumerator ToggleIsCollumn()
    {
        isColumnStyle = true;
        yield return new WaitForSeconds(10f);
        isColumnStyle = false;

    }

}
