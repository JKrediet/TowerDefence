using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //waves
    public Transform meleeEnemyPrefab, rangedEnemyPrefab, tankEnemyPrefab;
    public List<Transform> spawnsPoints;
    public enum SpawnState {spawning, waiting, counting}
    private bool startCounting;
    public GameObject startButton;

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int meleeEnemy, rangedEnemy, tankEnemy;
        public float spawnDelay;
    }

    public Wave[] waves;
    private int nextWave = 0;
    private float totalEnemies;

    public float timeBetweenWaves = 5f;
    private float waveCountDown;

    private SpawnState state = SpawnState.counting;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }
    private void Update()
    {
        if (waveCountDown <= 0)
        {
            startCounting = false;
            waveCountDown = timeBetweenWaves;
            if (state != SpawnState.spawning)
            {
                totalEnemies = waves[nextWave].rangedEnemy + waves[nextWave].meleeEnemy + waves[nextWave].tankEnemy;
                StartCoroutine(SpawnWave(waves[nextWave]));
                nextWave++;
            }
        }
        else
        {
            if (startCounting)
            {
                waveCountDown -= Time.deltaTime;
            }
        }
    }
    public void StartWave()
    {
        startCounting = true;
        startButton.SetActive(false);
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.spawning;
        for (int i = 0; i < _wave.meleeEnemy; i++)
        {
            SpawnEnemy(meleeEnemyPrefab);
            yield return new WaitForSeconds(_wave.spawnDelay);
        }
        yield return new WaitForSeconds(_wave.spawnDelay);
        for (int i = 0; i < _wave.rangedEnemy; i++)
        {
            SpawnEnemy(rangedEnemyPrefab);
            yield return new WaitForSeconds(_wave.spawnDelay);
        }
        yield return new WaitForSeconds(_wave.spawnDelay);
        for (int i = 0; i < _wave.tankEnemy; i++)
        {
            SpawnEnemy(tankEnemyPrefab);
            yield return new WaitForSeconds(_wave.spawnDelay);
        }
        state = SpawnState.waiting;
    }

    private void SpawnEnemy(Transform _enemy)
    {
        int randomNumber = Random.Range(0, spawnsPoints.Count);
        Instantiate(_enemy, spawnsPoints[randomNumber].transform.position, spawnsPoints[randomNumber].transform.rotation);
    }
    public void EnemyDied()
    {
        totalEnemies--;
        if(totalEnemies == 0)
        {
            startButton.SetActive(true);
            FindObjectOfType<Shop>().DeleteItems();
            FindObjectOfType<Shop>().GiveStatsToItem();
        }
    }
}