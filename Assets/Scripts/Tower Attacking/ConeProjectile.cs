using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeProjectile : MonoBehaviour
{

    [HideInInspector] public int damage =1;
    [HideInInspector] public TowerController towerOwner;
    public Quaternion IR;
    public Transform attackTarget;
    [SerializeField] float speed, ShakeTime,  _maxDistanceTraveled, rotSpeed, scale = 0f;
    [SerializeField] private LayerMask targetLayer;

    private float MaxDistanceTraveled
    {
        get => _maxDistanceTraveled;
        set => _maxDistanceTraveled = Mathf.Max(float.Epsilon, value);
    }





    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;
    private float DistanceTraveled => Vector3.Distance(_startPosition, transform.position);


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }


    void Start()
    {
        rotSpeed = 9f;
        IR = Quaternion.Euler(new Vector3(this.transform.localRotation.x, this.transform.localRotation.y, this.transform.localRotation.z));
        _rigidbody.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (DistanceTraveled > _maxDistanceTraveled)
        {
            Destroy(gameObject);
        }
       
        shake();
        
    }
    void shake()
    {
      if (attackTarget == null)
        {  Destroy(gameObject);}

        if (ShakeTime < 2.121)
        {
            Expand();
            ShakeTime += Time.deltaTime;
           

        }
    }

    void Expand()
    {
        scale += 0.0125f;
        transform.Rotate(rotSpeed, 0, 0);
        if(transform.localScale.x < 2.54)
        {
            transform.localScale = new Vector2(scale, transform.localScale.y);
           
        }  
    }

   



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<move>().TakeDamage(damage);
           
            towerOwner.AddTargetsFromProjectile(collision.GetComponent<move>());
         
            Destroy(gameObject);
        }
    }
}
