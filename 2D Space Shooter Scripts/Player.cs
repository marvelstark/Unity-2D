
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    
    [SerializeField]
    public float _speed = 10.0f;
    public float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject[] _hurtPrefab;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    [SerializeField]
    private float _firerate = 0.001f;
    private float _nextfire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    //[SerializeField]
    //private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private int _score;
  
    private UIManager _uiManager;


    
   
    
    void Start()
    {
        
        transform.position = new Vector3(0, -4, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
       
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");


        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI manager is NULL");
        }
        
    }

    void Update()
    {
        CalculateMovement();
#if UNITY_ANDROID

        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && (Time.time > _nextfire))
        {
            ShootLaser();
        }
#elif UNITY_IOS
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && (Time.time > _nextfire))
        {
            ShootLaser();
        }
#else
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0)) && (Time.time > _nextfire))
        {
            ShootLaser();
        }
#endif
    }


    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");  
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        transform.Translate(direction * _speed * Time.deltaTime);

        /*if(_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);

        }
        */


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, -2), 0);
        
        if (transform.position.x > 15.0f)
        {
            transform.position = new Vector3(-14.0f, transform.position.y, 0);
        }
        else if (transform.position.x < -14.0f)
        {
            transform.position = new Vector3(15.0f, transform.position.y, 0);
        }
    }
    
    void ShootLaser()
    {
         _nextfire = Time.time + _firerate;

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);                       
        }
        else
        {            
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
        }

        _audioSource.Play();
        
         Debug.Log("space key pressed");
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }
        
        _lives --;

        if(_lives == 2)
        {
            _hurtPrefab[0].SetActive(true);
        }
        if(_lives == 1)
        {
            _hurtPrefab[1].SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            
        }

    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;

        StartCoroutine(TripleShotPowerDownRoutine());


    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;

    }

    public void SpeedBoostActive()
    {
        //_isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        //_isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualiser.SetActive(true);
       // _lives += 3;
        StartCoroutine(ShieldPowerDownRoutine());
        
    }
    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShieldActive = false;
        _shieldVisualiser.SetActive(false);

        
       // _lives -= 3;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    //communicate with the UI to update the score!

    
}