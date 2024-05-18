using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerupID;

    [SerializeField]
    private AudioClip _powerClip;



    
    void Start()
    {
       
    }

    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < - 9.0f)
        {
            float randomX = Random.Range(-4.8f, 6.5f);
            transform.position= new Vector3(randomX, 9.0f, 0);


        }

       

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerClip, transform.position);
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                
                switch (powerupID)
                {
                    case 0:
                        
                        player.TripleShotActive();
                        break;
                    case 1:
                        
                        player.SpeedBoostActive();
                        Debug.Log("Collected speed boost");
                        break;
                    case 2:
                        
                        player.ShieldActive();
                        Debug.Log("Collected shield");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }           
            Destroy(this.gameObject);
        }
    }
}
