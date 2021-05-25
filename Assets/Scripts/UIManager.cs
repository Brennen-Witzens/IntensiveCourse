using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public override void Init()
    {
        base.Init();
    }

    [Header("References")]
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Image _reloadBar;
    [SerializeField]
    private Text _thrusterText;
    [SerializeField]
    private Image _thrusterBar;

    [Header("Reference Values")]
    private int _score;
    [SerializeField]
    private int _lives;
    private int _ammoCount;

    private IEnumerator _reloadRoutine;
    private IEnumerator _thrusterRoutine;
    private void Start()
    {
        _scoreText.text = "Score: 0";
        _lives = 3;
        _reloadBar.fillAmount = 0;
        _thrusterText.text = "Fuel";

    }


    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.7f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
        }
    }


    public void UpdateScore(int score)
    {
        _score += score;
        _scoreText.text = "Score: " + _score;
    }

    public void UpdateLives(int lives)
    {
        _livesImage.fillAmount = (float)lives / (float)_lives;


    }


    public void UpdateAmmoCount(int ammo, bool isReloading)
    {
        _reloadRoutine = Reload(isReloading);
        if (isReloading)
        {
            _ammoText.color = Color.red;
            _ammoText.text = "Reloading!";
            StartCoroutine(_reloadRoutine);

        }
        else
        {
            if (_reloadBar.gameObject.activeInHierarchy == true)
            {
                _reloadBar.gameObject.SetActive(false);
                StopCoroutine("Reload");
            }
            _ammoText.color = Color.white;
            _ammoCount = ammo;
            _ammoText.text = "Ammo: " + _ammoCount;

        }

    }


    private IEnumerator Reload(bool isReloading)
    {
        _reloadBar.gameObject.SetActive(true);
        //_reloadBar.fillAmount = 0;
        float timer = 3f;
        float aniTime = 3f;
        while (isReloading && aniTime > 0)
        {
            aniTime -= Time.deltaTime;
            _reloadBar.fillAmount = aniTime / timer;
            yield return new WaitForEndOfFrame();
        }


    }


    public void UpdateThrusters(float currentFuel, float maxFuel)
    {
        _thrusterBar.fillAmount = currentFuel / maxFuel;

    }





}
