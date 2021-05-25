using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Attribute Fields")]
    [SerializeField]
    private float _speed;
    private Vector3 _direction;
    private float _fireRate = 1f;
    private float _nextFire = -1f;
    private float _randomValue;
    private bool _newMovement = false;

    [Header("Position Info")]
    [SerializeField]
    private float _yBounds;
    private float _randomXPos;
    [SerializeField]
    private float _ySpawnPos;

    [Header("References")]
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _firePoint;
    private BoxCollider2D _col;
    private Player _player;
    private Laser[] _laser;

    [Header("Score?")]
    [SerializeField]
    private int _scoreRewarded;

    private Animator _anim;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _col = GetComponent<BoxCollider2D>();
        _laser = _laserPrefab.GetComponentsInChildren<Laser>();


        if (_laser == null)
        {
            Debug.LogError("Laser is NULL on: " + gameObject.name);
        }

        if (_col == null)
        {
            Debug.LogError("Box collider is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Clip is null");
        }
        else
        {
            _audioSource.clip = _explosion;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();


    }

    private void FixedUpdate()
    {
        FireLaser();
    }

  
    private void Movement()
    {

        _direction = Vector3.down * (_speed * Time.deltaTime);

        transform.Translate(_direction);
        if (transform.position.y <= _yBounds)
        {

            _randomXPos = Random.Range(-9f, 9f);
            transform.position = new Vector3(_randomXPos, _ySpawnPos, 0);
        }

        if (_randomValue > 0.65)
        {
            _newMovement = true;
            if (_newMovement)
            {
                transform.Translate(0.04f * Mathf.Cos(Time.time), (-1 * _speed * Time.deltaTime), 0);
            }
        }



        float offSet = 1f;
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(_firePoint.transform.position.x - offSet, _firePoint.transform.position.y), new Vector2(_firePoint.transform.position.x + offSet, _firePoint.transform.position.y));

        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("Laser"))
            {
                Vector3 target = new Vector3(transform.position.x - 1, transform.position.y);
                transform.position = Vector3.Lerp(transform.position, target, 1);
            }
        }


    }

    private void OnEnable()
    {
        _speed = 6.5f;
        _randomValue = Random.value;
    }

    private void OnDisable()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage();
            }
            OnDeath();
            //this.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Laser"))
        {

            Destroy(other.gameObject);
            OnDeath();
            //this.gameObject.SetActive(false);

        }
        else if (other.CompareTag("EnemyLaser"))
        {
            return;
        }


    }

    private void FireLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.transform.position, Vector2.down, 3f);

        Debug.DrawRay(_firePoint.transform.position, Vector2.down, Color.red);

        for (int i = 0; i < _laser.Length; i++)
        {
            _laser[i].SetEnemy();
        }

        if (hit.collider != null && hit.collider.CompareTag("Player") && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, _firePoint.transform.position, Quaternion.identity);
        }


    }






    private void OnDeath()
    {

        _col.enabled = false;
        _scoreRewarded += 100;
        _anim.SetTrigger("OnDeath");
        _speed = 0;
        _anim.Play("Enemy_destroyed_anim");
        CameraShake.Instance.Shake(1);
        _audioSource.Play();
        UIManager.Instance.UpdateScore(_scoreRewarded);
        enabled = false;
        Destroy(this.gameObject, 2.6f);

    }




}
