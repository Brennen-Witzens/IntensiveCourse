using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed;
    private float _horizontalInput;
    private float _verticalInput;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_horizontalInput, _verticalInput, 0) * (_speed * Time.deltaTime);
        transform.Translate(movement);


    }
}
