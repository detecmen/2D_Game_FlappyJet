using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpaceshipBoss : MonoBehaviour
{
    public GameObject prefab;
    public GameObject bulletPrefab;
    public GameObject playerPosition;

    public float spawnInterval = 10f;
    public float spawnPositionY = 10f;
    public float spawnPositionX = 10f;

    public float speedMove = 2f;
    private bool bossSpawn = false;
    public float bulletSpeed = 5f;
    public float fireDelay = 1f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Coroutine coroutine;

    // Start is called before the first frame update
    private void OnEnable()
    {

        GameManager.OnGameStarted += StarSpawning;
        TapController.OnPlayerDied += Cleanup;
    }
    public void ResetGame()
    {
        Cleanup();
        bossSpawn = false;
        StarSpawning();
    }

    private void OnDisable()
    {
        TapController.OnPlayerDied -= Cleanup; // Unsubscribe to prevent memory leaks
    }

    private void StarSpawning()
    {
        if (spawnedObjects.Count == 0 && !bossSpawn)
        {
            coroutine = StartCoroutine(SpawnObjects());
        }
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


    IEnumerator SpawnObjects()
    {
        while (!bossSpawn)
        {
            if (GameManager.Instance.Score > 4)
            {
                Vector3 spawnPosition = new Vector3(spawnPositionX, spawnPositionY, 0f);
                GameObject spawnObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
                Debug.Log("Spawn Position: " + spawnObject); // Log the spawn position
                AudioManager.Instance.PlayUFOSound();
                StartCoroutine(Moveleft(spawnObject));
                spawnedObjects.Add(spawnObject);
                StartCoroutine(ShootAtPlayer(spawnObject.transform.position, playerPosition.transform.position));
                yield return new WaitForSeconds(spawnInterval);

            }
            yield return null;
        }
    }

    IEnumerator Moveleft(GameObject spawnObject)
    {
        while (spawnObject != null && spawnObject.transform.position.x > -10f)
        {
            spawnObject.transform.position += Vector3.left * speedMove * Time.deltaTime;
            yield return null;
        }
        if (spawnObject != null)
        {
            Destroy(spawnObject);
            spawnedObjects.Remove(spawnObject);
        }
    }


    IEnumerator ShootAtPlayer(Vector3 positionSpawn, Vector3 positionHit)
    {

        yield return new WaitForSeconds(2f);
        Vector3 bulletSpawnPosition = positionSpawn + new Vector3(-3f, 0f, 0f);
        Debug.Log($"Bullet Spawn Position: {bulletSpawnPosition}");
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);

        Vector3 direction = (positionHit - bulletSpawnPosition).normalized;
        StartCoroutine(MoveBullet(bullet, direction));
        yield return new WaitForSeconds(Random.Range(3f, 5f));

    }

    IEnumerator MoveBullet(GameObject bullet, Vector3 direction)
    {
        while (bullet != null)
        {

            bullet.transform.position += direction * Time.deltaTime * bulletSpeed;
            if (bullet.transform.position.x < -10f)
            {
                Destroy(bullet);
                break;
            }
            yield return null;
        }
    }

    private void Cleanup()
    {


        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear(); // Clear the list after destroying


        bossSpawn = false;
    }

}
