using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float hp = 200.0f;
    public float speed;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;
    public GameObject[] damageEffect;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject instShot = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            Destroy(instShot, 1.0f);
            AudioManager.instance.Play("Shoot1");
        }

        if (Input.GetButtonUp("Fire1"))
        {
            nextFire = Time.time;
            AudioManager.instance.Play("Shoot1");
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        GetComponent<Rigidbody2D>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.zMin, boundary.zMax),
            0.0f
        );

        //GetComponent<Transform>().rotation = Quaternion.Euler(0.0f, GetComponent<Rigidbody2D>().velocity.x * -tilt, 0.0f);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyShoot")
        {
            //hp -= 10.0f;
            Destroy(other.gameObject);
            Instantiate(damageEffect[Random.Range(0, damageEffect.Length - 1)], transform.position, transform.rotation, transform);
            AudioManager.instance.Play("Zap");
            return;
        }
    }
}

