using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;
    float shotCounter;    
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;      
    [SerializeField] float projectileSpeed = -2f;

    [Header("Enemy FX")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float explosionDuration = 1f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] [Range(0,1)] float hitSoundVolume = 0.75f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }    

    private void Fire()
    {
        GameObject laser = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume);
        if (health <= 0)
        {            
            Die();
        }
    }

    private void Die()
    {

        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, explosionDuration);
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
