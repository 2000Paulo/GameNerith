using UnityEngine;
using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using DbHelpers;

public class Jogador : MonoBehaviour
{
    // COMPONENTES
    private Rigidbody2D oRigidBody;
    private Animator oAnimator;
    private SpriteRenderer oSpriteRenderer;
    private BoxCollider2D oBoxCollider2d;

    // ESTADOS
    private bool isRunning;
    private bool isWalking;
    // private bool isJumping;
    private bool isGrounded;
    private bool isAttacking = false; // NOVO

    // VARIÁVEIS QUE RECEBEM OS INPUTS
    private float xAxiesInputDirection; // input do eixo x ( útil para a movimentação para a esquerda ou direita )
    private float yAxiesInputDirection;

    // CONSTANTES QUE SÃO ACESSÍVEIS PELO "Inspector"
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpSpeed = 5f;
    public float attackDuration = 0.5f; // Duração da animação de ataque

    private void Start()
    {
        oRigidBody = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oSpriteRenderer = GetComponent<SpriteRenderer>();
        oBoxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        var oAnimatorStateInfo = oAnimator.GetCurrentAnimatorStateInfo(0);
        // var estaEmAnimacaoDeAtaque = oAnimatorStateInfo.IsName("Attack") && oAnimatorStateInfo.normalizedTime < 1f ? true : false;

        DbDebugger.DebugObject( oAnimatorStateInfo.IsName("Attack"));


        

        // if (estaEmAnimacaoDeAtaque)
        // {
        //     oBoxCollider2d.enabled = true;
        //     var bViradoParaEsquerda = oSpriteRenderer.flipX == true;

        //     // inverte a posição do colider para onde o personagem está olhando
        //     if (bViradoParaEsquerda)
        //     {
        //         float nOffsetXParaEsquerda = -Math.Abs(oBoxCollider2d.offset.x);
        //         oBoxCollider2d.offset = new Vector2(nOffsetXParaEsquerda, oBoxCollider2d.offset.y);
        //     }
        //     else
        //     {
        //         float nOffsetXParaDireita = Math.Abs(oBoxCollider2d.offset.x);
        //         oBoxCollider2d.offset = new Vector2(nOffsetXParaDireita, oBoxCollider2d.offset.y);

        //     }
        // }
        // else
        // {
        //     oBoxCollider2d.enabled = false;
        // }



        xAxiesInputDirection = Input.GetAxisRaw("Horizontal");
        yAxiesInputDirection = Input.GetAxisRaw("Vertical");

        isWalking = xAxiesInputDirection != 0 ? true : false;
        isRunning = Input.GetKey(KeyCode.LeftShift) && xAxiesInputDirection != 0;

        oAnimator.SetBool("isWalking", isWalking);
        oAnimator.SetBool("isRunning", isRunning);

        // ATAQUE (NOVO)
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(RealizarAtaque());
        }

        if (yAxiesInputDirection > 0 && isGrounded) {
            oRigidBody.linearVelocity = new Vector2(oRigidBody.linearVelocity.x, jumpSpeed);
            isGrounded = false;
            oAnimator.SetBool("isJumping", !isGrounded);
        }

        var goLeft  = xAxiesInputDirection < 0 ? true : false;
        var goRight = xAxiesInputDirection > 0 ? true : false;

        if (goLeft) {
            oSpriteRenderer.flipX = true;
        }

        if (goRight) {
            oSpriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        // Impede movimentação horizontal durante o ataque (opcional)
        if (isAttacking) return;

        float speed = isRunning ? runSpeed : walkSpeed;
        var xAxiesSpeed = speed * xAxiesInputDirection;
        oRigidBody.linearVelocity = new Vector2(xAxiesSpeed, oRigidBody.linearVelocity.y);
    }

    private void OnTriggerEnter2D (Collider2D collision) 
    {
        isGrounded = true;
        oAnimator.SetBool("isJumping", !isGrounded);
    }

    // CORROTINA DE ATAQUE
    private IEnumerator RealizarAtaque()
    {
        isAttacking = true;
        oAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}
