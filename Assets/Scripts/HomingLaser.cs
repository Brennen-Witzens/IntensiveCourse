using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{

    [Header("Attribute Fields")]
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private float _changeSpeed;
    [SerializeField]
    private float _moveSpeed;
    private List<GameObject> _possibleTargets;


    private void OnEnable()
    {
        GetClosestTarget();
    }


    private void GetClosestTarget()
    {
        _possibleTargets = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPostion = transform.position;
        foreach(var potentalTarget in _possibleTargets)
        {
            Vector3 direction = potentalTarget.transform.position - currentPostion;
            float dirToTarget = direction.sqrMagnitude;

            if(dirToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dirToTarget;
                _target = potentalTarget.transform;
            }
        }

    }


    private void FixedUpdate()
    {
        
            MovementInfo();
        
        
    }


    private void MovementInfo()
    {
        if(_target != null)
        {
            Vector2 direction = (Vector2)_target.position - _rigidBody.position;

            direction.Normalize();

            float rotate = Vector3.Cross(direction, transform.up).z;

            _rigidBody.angularVelocity = -_changeSpeed * rotate;
            _rigidBody.velocity = transform.up * _moveSpeed;

            
        }
        else
        {
            transform.Translate(_moveSpeed * Vector3.up * Time.deltaTime);
            if (transform.position.y > 9)
            {
                Destroy(this.gameObject);
            }
        }

        if(transform.position.x < -10f || transform.position.x > 10f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }


    }





}
