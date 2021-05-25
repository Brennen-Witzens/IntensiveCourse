using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Transactions;

public class SpawnManager : MonoSingleton<SpawnManager>
{

    public override void Init()
    {
        base.Init();
    }

    [Header("Wave System")]
    [SerializeField]
    private List<Wave> _wave;
    //[SerializeField]
    //private List<GameObject> _enemyPool;

    [Header("Power Ups")]
    [SerializeField]
    private List<GameObject> _powerUpList;


    [Header("Containers")]
    [SerializeField]
    private Transform _enemyContainer;




    [Header("Spawn Point")]
    private Vector3 _spawnPoint;
    private float _ranX;
    private float _yPos = 8f;

    [Header("Wave Checking")]
    [SerializeField]
    private int _currentWave;
    [SerializeField]
    private int _finalWave;
    [SerializeField]
    private int _waveMultiplier;

    [Header("Checks")]
    private float _spawnDelay = 0.8f;
    private bool _stopSpawning;

    [Header("Wait for Seconds")]
    private WaitForSeconds _breatheRoom;
    private WaitForSeconds _spawnDelayYield;






    private void Start()
    {

        _breatheRoom = new WaitForSeconds(_spawnDelay);
        _spawnDelayYield = new WaitForSeconds(_spawnDelay);

        _finalWave = _wave.Count;

        _stopSpawning = GameManager.Instance.IsGameOver;

        

    }


    public void StartSpawning()
    {
        StartCoroutine(EnemyPoolSpawn());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator EnemyPoolSpawn()
    {

        yield return _breatheRoom;

        while (_stopSpawning == false && GameManager.Instance.StartedGame)
        {


            var currentWave = _wave[_currentWave].enemies;
            var enemyCount = _enemyContainer.childCount;

            if (enemyCount < 1)
            {
                foreach (var enemy in currentWave)
                {
                    _ranX = Random.Range(-9f, 9f);
                    _spawnPoint = new Vector3(_ranX, _yPos, 0);
                    Instantiate(enemy, _spawnPoint, Quaternion.identity, _enemyContainer);
                    yield return _spawnDelayYield;
                }
                _currentWave++;
            }


            yield return _spawnDelayYield;


            if (_currentWave == _finalWave)
            {
                break;
            }


        }

    }


    private IEnumerator SpawnPowerUpRoutine()
    {

        while (_stopSpawning == false && GameManager.Instance.StartedGame)
        {

            _ranX = Random.Range(-9f, 9f);
            _spawnPoint = new Vector3(_ranX, _yPos, 0);
            Instantiate(_powerUpList[Random.Range(0, _powerUpList.Count)], _spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));



            yield return new WaitForSeconds(0.2f);

        }




    }







    public void OnPlayerDeath()
    {
        _stopSpawning = GameManager.Instance.IsGameOver;
    }




}
