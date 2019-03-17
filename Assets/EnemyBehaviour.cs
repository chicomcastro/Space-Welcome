using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float hp = 100.0f;

    public GameObject shoot;
    public Transform shotSpawn;
    public float fireRate;

    public GameObject[] damageEffect;

    private GameObject player;
    public Boundary boundary;
    public float amplitude = 5.0f;
    public float speed = 0.5f;
    public float offSet = -7f;

    private float time;
    private Vector2 initialPos;

    void Start()
    {
        InvokeRepeating("Shoot", 5.0f, fireRate);

        player = GameObject.FindGameObjectWithTag("Player");

        initialPos = new Vector2(player.transform.position.x, transform.position.y);

        time = Time.time;

        speed = Random.Range(0.5f, 1.0f);
    }

    void Update()
    {
        if (hp <= 0)
        {
            Instantiate(damageEffect[Random.Range(0, damageEffect.Length - 1)], transform.position, transform.rotation, transform);
            GameController.instance.RescuePlayer();
            AudioManager.instance.Play("Death");
            Destroy(this.gameObject);
            return;
        }

        float A = amplitude;
        float epsilon = 0.5f;
        float omega = speed;
        float t = (Time.time - time);
        Vector2 newPos = initialPos + new Vector2(
            amplitude * Mathf.Sin(speed * t),
            offSet
            + amplitude * 5.0f * Mathf.Exp(-omega * t) * Mathf.Cos(Mathf.Sqrt(1 - epsilon * epsilon) * omega * t)
            + amplitude / 5 * Mathf.Sin(speed / 3 * t)
            );

        transform.position = new Vector3
        (
            Mathf.Clamp(newPos.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(newPos.y, boundary.zMin, boundary.zMax),
            0.0f
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Shoot")
        {
            hp -= 10.0f;
            GameObject instEffect = Instantiate(damageEffect[Random.Range(0, damageEffect.Length - 1)], transform.position, transform.rotation, transform);
            Destroy(instEffect, 1.0f);
            AudioManager.instance.Play("Zap");
            return;
        }

        if (other.tag == "Player")
        {
            Instantiate(damageEffect[Random.Range(0, damageEffect.Length - 1)], transform.position, transform.rotation, transform);
            Instantiate(damageEffect[Random.Range(0, damageEffect.Length - 1)], other.transform.position, other.transform.rotation, transform);
            AudioManager.instance.Play("Zap");
            //Destroy(other.gameObject, 0.1f);
            //Destroy(gameObject, 0.1f);
        }
    }

    void Shoot()
    {
        GameObject instShot = Instantiate(shoot, shotSpawn.position, shotSpawn.rotation);
        Destroy(instShot, 1.0f);
            AudioManager.instance.Play("Shoot2");
    }
}
