using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region toc do quai
    //toc do cua quai
    [SerializeField]
    private float speed;
    #endregion
    #region toa do cho quai di chuyen trai phai
    //toa do cho quai di chuyen trai phai
    [SerializeField]
    private float leftEnemy = 0f;
    [SerializeField]
    private float rightEnemy = 0f;
    #endregion
    private Animator anim;
    public Collider2D colli;
    public GameObject target;
    public float range;
    bool canAttack = true;

    //check xem quai di chuyen trai hay phai
    private bool isMovingRight = true;
    
    private SpriteRenderer eScale;

    #region Health Enemy
    //thanh mau
    [SerializeField]
    private Slider healthSlider;

    private int _healthMax;

    [SerializeField]
    private GameObject hpCanvas;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        target.GetComponent<Player>();
        eScale = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        _healthMax = 100;
        healthSlider.maxValue = _healthMax;
        hpCanvas.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy();
    }
    
    void deah()
    {
        Destroy(gameObject, 1.1f);
        return;
    }
    private void MoveEnemy()
    {
        //lay vi tri hien tai cua enemy
        var currentPosition = transform.localPosition;
        if (currentPosition.x > rightEnemy)
        {
            //di chuyen sang trai
            isMovingRight = false;
            transform.localScale = new Vector3(-1,1,1);
            healthSlider.transform.localScale = new Vector3(-1,1,1);
            //eScale.flipX = true;
        }
        else if(currentPosition.x < leftEnemy)
        {   //di chuyen sang phai
            isMovingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
            healthSlider.transform.localScale = new Vector3(1, 1, 1);
            //eScale.flipX = false;
        }
        var direction = isMovingRight ? Vector3.right : Vector3.left;
        transform.Translate(direction * speed * Time.deltaTime);
        var player = target.gameObject.transform.position;
        if(target)
        {
            if(Vector3.Distance(transform.position,player) <= range && canAttack)
            {
                
                StartCoroutine(eDieAnimation());
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitBox") || collision.gameObject.CompareTag("Skill"))
        {   
            hpCanvas.SetActive(true);
            _healthMax -= 30;
            healthSlider.value = _healthMax;
            if (_healthMax <= 0)
            {
                //colli.enabled = false;
                speed = 0f;
                anim.SetTrigger("Deah");
                //deah();
                Destroy(gameObject, 1.1f);
            }
           

        }
    }

    //test 
    private IEnumerator eDieAnimation()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        speed = 0f;
        yield return new WaitForSeconds(1.5f);
        anim.ResetTrigger("Attack");
        speed = 1;
        canAttack = true;


    }
}
