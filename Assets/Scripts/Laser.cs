using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [Header("Attribute Fields")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _yBounds;
    [SerializeField]
    private bool _isEnemy;

    private Vector3 _direction;
    


    void Update()
    {
        Movement();
    }

    private void Movement()
    {

        if (!_isEnemy)
        {
            MoveUp();
        }
        else
        {

            MoveDown();

        }

    }

    private void MoveUp()
    {
        _direction = Vector3.up * (_speed * Time.deltaTime);

        transform.Translate(_direction);

        if (transform.position.y >= _yBounds)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }


    private void MoveDown()
    {
        _direction = Vector2.down * (_speed * Time.deltaTime);
        transform.Translate(_direction);

        if (transform.position.y <= -4)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetEnemy()
    {
        _isEnemy = true;
        this.gameObject.tag = "EnemyLaser";
    }

}
