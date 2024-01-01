using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceProjectileController : MonoBehaviour
{
    public Vector2 moveDir;
    public int popsStartingAmount = 3;
    private int popsLeft;
    public float moveSpeed = 10;
    public float projectileTimer = 0;
    public float maxProjTime = 0.5f;
    [HideInInspector] public int damage = 1;

    /*[HideInInspector] public TowerManager towerManager;*/ //set by pierceAttackScript
    [HideInInspector] public TowerController towerOwner;

    // Start is called before the first frame update
    void Start()
    {
        transform.up = moveDir;
        popsLeft = popsStartingAmount;
        /*StartCoroutine(CheckIfOffScreen());*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);

        projectileTimer += Time.deltaTime;

        if (projectileTimer > maxProjTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            /*Debug.Log("hit enemy " + collision.GetComponent<move>().see);*/
            collision.GetComponent<move>().TakeDamage(damage);
            towerOwner.AddTargetsFromProjectile(collision.GetComponent<move>());
            /*Debug.Log(popsLeft);*/
            popsLeft--;
            if (popsLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator CheckIfOffScreen()
    {
        Camera mainCam = Camera.main;
        Vector2 camOffset = mainCam.transform.position;
        Vector2 topRight = new Vector2(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize) + camOffset;
        Vector2 bottomLeft = (topRight * -1) + camOffset;
        while (transform.position.x < topRight.x && transform.position.x > bottomLeft.x
            && transform.position.y < topRight.y && transform.position.y > bottomLeft.y)
        {
            yield return new WaitForSeconds(1);
            camOffset = mainCam.transform.position;
            topRight = new Vector2(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize) + camOffset;
            bottomLeft = topRight * -1 + camOffset;
        }
        Destroy(gameObject);
    }

    /*private void OnDrawGizmos()
    {
        Camera mainCam = Camera.main;
        Gizmos.DrawWireSphere(new Vector3(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize, -10), 5f);
        Gizmos.DrawWireSphere(-1 * new Vector3(mainCam.orthographicSize * mainCam.aspect, mainCam.orthographicSize, -10), 5f);
    }*/
}
