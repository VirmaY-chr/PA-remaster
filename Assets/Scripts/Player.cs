using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class Player : MonoBehaviour
{
    [Header("Gameplay")]
    public float speed = 350;
    public float boostMultiplier = 10;

    bool isHit = false;

    SpriteRenderer sr;
    Rigidbody2D rb;
    [SerializeField]
    Vector2 movementDir;
    bool d = false;


    private void Awake()
    {
        TryGetComponent(out sr);
        TryGetComponent(out rb);
    }

    private void LateUpdate()
    {
        //keep object on the screen
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.01f, 0.99f);
        pos.y = Mathf.Clamp(pos.y, 0.02f, 0.97f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }


    private void Update()
    {
        if(!d)
            Dash();
    }

    private void FixedUpdate()
    {
        //movement
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        movementDir = new Vector2(horInput, verInput);
        rb.AddForce(movementDir * speed);


        //player angle
        if (movementDir != Vector2.zero)
            transform.right = movementDir;
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && movementDir != Vector2.zero)
        {
            StartCoroutine(Dash());
        }

        IEnumerator Dash()
        {
            d = true;
            rb.drag *= 3;
            speed *= boostMultiplier;
            yield return new WaitForSeconds(0.1f);
            rb.drag /= 3;
            speed /= boostMultiplier;
            yield return new WaitForSeconds(0.1f);
            d = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isHit == false)
        {
            isHit = true;
            StartCoroutine(Hit());
        }

        IEnumerator Hit()
        {
            UI.inst.Hit();
            AudioManager.inst.sfx.Play();
            for (int i = 0; i < 14; i++)
            {
                yield return new WaitForSeconds(0.03f);
                sr.transform.localScale = Vector2.zero;
                yield return new WaitForSeconds(0.03f);
                sr.transform.localScale = Vector2.one;
            }
            isHit = false;
            if (UI.inst.hp == 0)
                gameObject.SetActive(false);
        }
    }
}
