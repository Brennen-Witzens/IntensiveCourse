using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private PowerUp _collectible;

    private float _pullSpeed = 1.5f;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.C))
        {
            PullPowerUp();
        }


    }   


    private void PullPowerUp()
    {

        _collectible = GameObject.FindObjectOfType<PowerUp>();

        if(_collectible != null)
        {
            _collectible.gameObject.transform.position = Vector3.Lerp(_collectible.gameObject.transform.position, this.transform.position, _pullSpeed * Time.deltaTime);

        }

    }



}
