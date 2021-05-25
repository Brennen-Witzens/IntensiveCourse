using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4.5f;
    private Vector3 _direction;


    public static event Action<string> onCollection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {

        _direction = Vector3.down * (_speed * Time.deltaTime);
        transform.Translate(_direction);
        
        if(transform.position.y < -4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onCollection?.Invoke(this.gameObject.tag);
            Destroy(this.gameObject);
        }


    }

}
