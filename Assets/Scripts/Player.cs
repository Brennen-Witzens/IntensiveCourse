using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Fields")]
    [SerializeField]
    private float _speed;
    private float _initalSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = -1f;

    [Header("Bounding Fields")]
    [SerializeField]
    private float _xBounds;
    [SerializeField]
    private float _yBounds;

    [Header("References")]
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _homingLaserPrefab; 
    [SerializeField]
    private Transform _firePoint;
    [SerializeField]
    private GameObject _shieldObject;
    [SerializeField]
    private GameObject _thrusterObject;
    [SerializeField]
    private GameObject[] _engines;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _explosionSource;
    private SpriteRenderer _shieldRenderer;


    [Header("Info Fields")]
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool _tripeShotCheck;
    [SerializeField]
    private bool _speedBoostCheck;
    [SerializeField]
    private bool _shieldCheck;
    private WaitForSeconds _cooldownTimer;
    private int _score;
    private int _currentAmmo;
    [SerializeField]
    private int _maxAmmo;
    private bool _isReloading = false;
    [SerializeField]
    private bool _thrustersActive;
    [SerializeField]
    private bool _thrusterCooldownActive;
    [SerializeField]
    private float _currentThrusterFuel;
    [SerializeField]
    private float _maxFuel;
    [SerializeField]
    private int _shieldCharges = 3;
    [SerializeField]
    private bool _homingActive;


    private void OnEnable()
    {
        PowerUp.onCollection += PowerUpCollection;
    }

    private void OnDisable()
    {
        PowerUp.onCollection -= PowerUpCollection;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (_shieldObject == null)
        {
            Debug.LogError("Shield Object is NULL on: " + gameObject.name);
        }

        _shieldRenderer = _shieldObject.GetComponent<SpriteRenderer>();

        if(_shieldRenderer == null)
        {
            Debug.LogError("Shield Renderer is NULL" + gameObject.name);
        }

        _shieldObject.SetActive(false);
        _initalSpeed = _speed;
        transform.position = Vector3.zero;
        _cooldownTimer = new WaitForSeconds(5.0f);
        _currentAmmo = _maxAmmo;
        _currentThrusterFuel = _maxFuel;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo, _isReloading);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }

        if (Input.GetKey(KeyCode.LeftShift) && !_thrusterCooldownActive)
        {

            StartThrusters();

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thrustersActive = false;
            _speed = _initalSpeed;

            if (_currentThrusterFuel == 0)
            {
                StartCoroutine(ThrustersCoolDown());
            }


        }

    }

    private void Movement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");


        Vector3 movement = new Vector3(_horizontalInput, _verticalInput, 0) * (_speed * Time.deltaTime);
        transform.Translate(movement);


        if (transform.position.x >= _xBounds)
        {
            transform.position = new Vector3(-_xBounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -_xBounds)
        {
            transform.position = new Vector3(_xBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.y <= _yBounds)
        {
            transform.position = new Vector3(transform.position.x, _yBounds, transform.position.z);
        }



    }


    private void FireLaser()
    {
        if (_currentAmmo > 0)
        {
            if (_tripeShotCheck)
            {
                _nextFire = Time.time + _fireRate;
                Instantiate(_tripleShotPrefab, _firePoint.position, Quaternion.identity);

            }
            else if (_homingActive)
            {
                _nextFire = Time.time + _fireRate;
                Instantiate(_homingLaserPrefab, _firePoint.position, Quaternion.identity);
            }
            else
            {
                _nextFire = Time.time + _fireRate;
                Instantiate(_laserPrefab, _firePoint.position, Quaternion.identity);

            }
            _currentAmmo--;
            UIManager.Instance.UpdateAmmoCount(_currentAmmo, _isReloading);
            _audioSource.Play();
        }
        else
        {
            StartCoroutine(Reload());
        }


    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo, _isReloading);
        yield return new WaitForSeconds(3f);
        _currentAmmo = _maxAmmo;
        _isReloading = false;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo, _isReloading);
    }




    public void Damage()
    {

        if (_shieldCheck)
        {
            switch (_shieldCharges) 
            {
                case 3:
                    _shieldCharges--;
                    _shieldRenderer.color = Color.green;
                    break;
                case 2:
                    _shieldCharges--;
                    _shieldRenderer.color = Color.red;
                    break;
                case 1:
                    _shieldCharges--;
                    _shieldObject.SetActive(false);
                    _shieldCheck = false;
                    break;
            }

            return;
        }

        _lives--;
        _score -= 50;
        UIManager.Instance.UpdateScore(_score);
        UIManager.Instance.UpdateLives(_lives);


        if (_lives == 2)
        {
            _engines[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _engines[1].SetActive(true);
        }

        _audioSource.Play();
        if (_lives < 1)
        {
            GameManager.Instance.GameOver();
            SpawnManager.Instance.OnPlayerDeath();
            UIManager.Instance.GameOver();
            Destroy(this.gameObject);
        }


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            Damage();
            Destroy(other.transform.parent.gameObject);
        }


    }


    private void PowerUpCollection(string powerUp)
    {

        switch (powerUp)
        {
            case "TripleShot":
                _tripeShotCheck = true;
                StartCoroutine(PowerUpCooldown(powerUp));
                break;
            case "SpeedBoost":
                _speedBoostCheck = true;
                _speed = 10f;
                _thrusterObject.SetActive(true);
                StartCoroutine(PowerUpCooldown(powerUp));
                break;
            case "Shield":
                _shieldCheck = true;
                _shieldObject.SetActive(true);
                _shieldRenderer.color = Color.white;
                if(_shieldCharges < 3)
                {
                    _shieldCharges = 3;
                }
                break;
            case "Health":
                _lives++;
                if (_lives == 3)
                {
                    _engines[0].SetActive(false);
                }
                else if (_lives == 2)
                {
                    _engines[1].SetActive(false);
                }

                UIManager.Instance.UpdateLives(_lives);
                break;
        }


    }


    private void StartThrusters()
    {
        
        if (_currentThrusterFuel > 0)
        {
            _thrustersActive = true;
            _speed = 15f;
            _currentThrusterFuel -= Time.deltaTime * 2; //15 seconds of thrust!
            
        }
        else if (_currentThrusterFuel < 0)
        {
            _currentThrusterFuel = 0;
            _speed = _initalSpeed;
            _thrustersActive = false;
        }

        UIManager.Instance.UpdateThrusters(_currentThrusterFuel, _maxFuel);

    }


    private IEnumerator ThrustersCoolDown()
    {
        _thrusterCooldownActive = true;

        while (_thrusterCooldownActive && _currentThrusterFuel < _maxFuel)
        {
            _currentThrusterFuel += Time.deltaTime;
            UIManager.Instance.UpdateThrusters(_currentThrusterFuel, _maxFuel);

            if (_currentThrusterFuel > _maxFuel)
            {
                _currentThrusterFuel = _maxFuel;
                _thrusterCooldownActive = false;
                UIManager.Instance.UpdateThrusters(_currentThrusterFuel, _maxFuel);
            }

           

            yield return null;
        }


    }



    private IEnumerator PowerUpCooldown(string powerUp)
    {

        switch (powerUp)
        {
            case "TripleShot":
                yield return _cooldownTimer;
                _tripeShotCheck = false;
                break;
            case "SpeedBoost":
                yield return _cooldownTimer;
                _speed = _initalSpeed;
                _speedBoostCheck = false;
                _thrusterObject.SetActive(false);
                break;
        }


    }









}
