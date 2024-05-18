using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
   [SerializeField]
    private float _rotateSpeed = 10.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    private Animator _anim2;

    private SpawnManager _spawnManager;

    private Player _player;
    private float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.25f);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            
        }
    }

}
