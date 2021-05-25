using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoSingleton<CameraShake>
{
    //Doing this just to have ONLY 1 instance of camera shake
    public override void Init()
    {
        base.Init();
    }



    [SerializeField]
    private float _shakeDur = 0f;
    [SerializeField]
    private float _shakeMagnitude = 0.5f;
    [SerializeField]
    private float _shakeDampening = 1.0f;

    private Vector3 _intialLocation;


    private void Start()
    {

        _intialLocation = transform.position;

    }

    private void Update()
    {
        if (_shakeDur > 0)
        {
            transform.position = _intialLocation + Random.insideUnitSphere * _shakeMagnitude;
            _shakeDur -= Time.deltaTime * _shakeDampening;
        }
        else
        {
            _shakeDur = 0f;
            transform.position = _intialLocation;
        }
    }



    public void Shake(int duration)
    {
        _shakeDur = duration;



    }




}
