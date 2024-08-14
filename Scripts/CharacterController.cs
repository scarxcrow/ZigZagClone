using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform rayStart;
    public GameObject crystalEffect;
    
    private Rigidbody rigidBody;
    private bool isWalkingRight = true;
    private Animator animator;
    private GameManager gameManager;


    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!gameManager.gameStarted)
        {
            return;
        }else
        {
            animator.SetTrigger("gameStarted");
        }

        rigidBody.transform.position = transform.position + transform.forward * 2 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Switch();
        }

        RaycastHit hit;

        if(!Physics.Raycast(rayStart.position, -transform.up, out hit, Mathf.Infinity))
        {
            animator.SetTrigger("isFalling");
        }
        else
        {
            animator.SetTrigger("notFallingAnymore");
        }

        if(transform.position.y < -2)
        {
            gameManager.EndGame();
        }
    }

    private void Switch()
    {

        if (!gameManager.gameStarted)
        {
            return;
        }

        isWalkingRight = !isWalkingRight;

        if(isWalkingRight)
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Crystal")
        {
            Destroy(other.gameObject);
            gameManager.IncreaseScore();

            GameObject g = Instantiate(crystalEffect, rayStart.transform.position, Quaternion.identity);
            Destroy(g, 2);
            Destroy(other.gameObject);
        }
    }
}
