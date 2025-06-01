using UnityEngine;
using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using DbHelpers;
using System.ComponentModel.Design.Serialization;

public class Jogador : MonoBehaviour
{
    // COMPONENTES
    private Rigidbody2D oRigidBody;
    private Animator oAnimator;
    private SpriteRenderer oSpriteRenderer;
    private BoxCollider2D oBoxCollider2d;

    // ESTADOS
    private bool isRunning;
    private bool correndo;
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
        /*
        ********************************************************
        *   Controller do "Animator" para os estados de:   
        *   -> Andando
        *   -> Pulando
        *   -> Atacando                       
        ********************************************************
        */


        oAnimator.SetBool("isWalking", Input.GetAxisRaw("Horizontal") != 0);

        oAnimator.SetFloat("Blend", Input.GetAxis("Horizontal"));
        oAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        oAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));



        if (Input.GetAxisRaw("Vertical") > 0 && isGrounded)
        {
            oRigidBody.linearVelocity = new Vector2(oRigidBody.linearVelocity.x, jumpSpeed);
            isGrounded = false;
            oAnimator.SetBool("isJumping", !isGrounded);
        }


        // vira o sprite para a direita ou esquerda.
        // dependendo da direção indicada pelo input (<-, ->, A, D) no eixo X
        AjustaDirecaoSprite();


        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(RealizarAtaque());
        }
        AjustaHitboxAtaque("player_animacao_ataque");




    }

    private void FixedUpdate()
    {
        // Impede movimentação horizontal durante o ataque (opcional)
        if (isAttacking) return;


        float nVelocidade = Math.Abs(Input.GetAxis("Horizontal")) == 1 ? runSpeed : walkSpeed;

        oRigidBody.linearVelocity = new Vector2(
            nVelocidade * Input.GetAxisRaw("Horizontal"), // velocidade em x
            oRigidBody.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    private void AjustaDirecaoSprite()
    {
        float eixoX = Input.GetAxisRaw("Horizontal");

        bool indoParaDireita = eixoX > 0;
        bool indoParaEsquerda = eixoX < 0;

        if (indoParaDireita)
        {
            oSpriteRenderer.flipX = false;
        }
        else if (indoParaEsquerda)
        {
            oSpriteRenderer.flipX = true;
        }
    }


    private void AjustaHitboxAtaque(string sNomeAnimacaoAtaque)
    {
        /*
        *****************************************************************
        *   Controller do tamanho do Hitbox de Ataque ("BoxCollider2D") *                       
        *****************************************************************
        */
        // Por padrão o collider de ataque é desativado
        oBoxCollider2d.enabled = false;
        // Ativamos 'isTrigger' para o uso da função 'OnTriggerEnter2D' em inimigos
        oBoxCollider2d.isTrigger = true;


        int iBaseLayer = 0;
        var oAnimacaoAtiva = oAnimator.GetCurrentAnimatorStateInfo(iBaseLayer);
        int iHashNomeAnimacaoDesejada = Animator.StringToHash(sNomeAnimacaoAtaque);
        int iHashNomeAnimacaoAtiva = oAnimacaoAtiva.shortNameHash;

        if (iHashNomeAnimacaoDesejada == iHashNomeAnimacaoAtiva)
        {
            oBoxCollider2d.enabled = true;
            float nComprimentoSprite = oSpriteRenderer.sprite.rect.width / oSpriteRenderer.sprite.pixelsPerUnit;
            float nNovoComprimentoCollider = nComprimentoSprite / 2;

            // O comprimento do collider é metade do tamanho do sprite
            oBoxCollider2d.size = new Vector2(
                nNovoComprimentoCollider,
                oBoxCollider2d.size.y
            );
            // O collider está centralizado no ponto x:0 do player, porém ancorado pelo centro do collider
            // Agora ele continuará no ponto x:0 do player, porém ancorado pela esquerda do collider.
            // e considerando se ele está virado para esquerda ou para a direita
            bool estaViradoParaDireita = oSpriteRenderer.flipX == false;

            float nOffsetX = Math.Abs(nNovoComprimentoCollider / 2);

            if (!estaViradoParaDireita) { nOffsetX = -nOffsetX; }

            oBoxCollider2d.offset = new Vector2(
                nOffsetX,
                oBoxCollider2d.offset.y
            );

        }
    }
}







// BACKUP

// using UnityEngine;
// using System;
// using System.Runtime;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using Newtonsoft.Json;
// using DbHelpers;

// public class Jogador : MonoBehaviour
// {
//     // COMPONENTES
//     private Rigidbody2D oRigidBody;
//     private Animator oAnimator;
//     private SpriteRenderer oSpriteRenderer;
//     private BoxCollider2D oBoxCollider2d;

//     // ESTADOS
//     private bool isRunning;
//     private bool isWalking;
//     // private bool isJumping;
//     private bool isGrounded;
//     private bool isAttacking = false; // NOVO

//     // VARIÁVEIS QUE RECEBEM OS INPUTS
//     private float xAxiesInputDirection; // input do eixo x ( útil para a movimentação para a esquerda ou direita )
//     private float yAxiesInputDirection;

//     // CONSTANTES QUE SÃO ACESSÍVEIS PELO "Inspector"
//     public float walkSpeed = 2f;
//     public float runSpeed = 5f;
//     public float jumpSpeed = 5f;
//     public float attackDuration = 0.5f; // Duração da animação de ataque

//     private void Start()
//     {
//         oRigidBody = GetComponent<Rigidbody2D>();
//         oAnimator = GetComponent<Animator>();
//         oSpriteRenderer = GetComponent<SpriteRenderer>();
//         oBoxCollider2d = GetComponent<BoxCollider2D>();
//     }

//     private void Update()
//     {
//         /*
//         ********************************************************
//         *   Controller do "Animator" para os estados de:   
//         *   -> Andando
//         *   -> Pulando
//         *   -> Atacando                       
//         ********************************************************
//         */

//         xAxiesInputDirection = Input.GetAxisRaw("Horizontal");
//         yAxiesInputDirection = Input.GetAxisRaw("Vertical");

//         isWalking = xAxiesInputDirection != 0 ? true : false;
//         isRunning = Input.GetKey(KeyCode.LeftShift) && xAxiesInputDirection != 0;

//         oAnimator.SetBool("isWalking", isWalking);
//         oAnimator.SetBool("isRunning", isRunning);



//         if (yAxiesInputDirection > 0 && isGrounded)
//         {
//             oRigidBody.linearVelocity = new Vector2(oRigidBody.linearVelocity.x, jumpSpeed);
//             isGrounded = false;
//             oAnimator.SetBool("isJumping", !isGrounded);
//         }

//         var goLeft = xAxiesInputDirection < 0 ? true : false;
//         var goRight = xAxiesInputDirection > 0 ? true : false;

//         if (goLeft)
//         {
//             oSpriteRenderer.flipX = true;
//         }

//         if (goRight)
//         {
//             oSpriteRenderer.flipX = false;
//         }


//         if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
//         {
//             StartCoroutine(RealizarAtaque());
//         }

//         /*
//         ********************************************************
//         *   Controller do Collider de Ataque ("BoxCollider2D") *                       
//         ********************************************************
//         */
//         // Por padrão o collider de ataque é desativado
//         // apenas é ativado na animação de ataque
//         oBoxCollider2d.enabled = false;
//         // Ativamos 'isTrigger' para o uso da função 'OnTriggerEnter2D' em inimigos
//         oBoxCollider2d.isTrigger = true;


//         int iBaseLayer = 0;
//         var oAnimacaoAtiva = oAnimator.GetCurrentAnimatorStateInfo(iBaseLayer);
//         int iHashNomeAnimacaoDesejada = Animator.StringToHash("player_animacao_ataque");
//         int iHashNomeAnimacaoAtiva = oAnimacaoAtiva.shortNameHash;

//         if (iHashNomeAnimacaoDesejada == iHashNomeAnimacaoAtiva) {
//             oBoxCollider2d.enabled = true;
//             float nComprimentoSprite = oSpriteRenderer.sprite.rect.width / oSpriteRenderer.sprite.pixelsPerUnit;
//             float nNovoComprimentoCollider = nComprimentoSprite / 2;
            
//             // O comprimento do collider é metade do tamanho do sprite
//             oBoxCollider2d.size = new Vector2(
//                 nNovoComprimentoCollider,
//                 oBoxCollider2d.size.y
//             );
//             // O collider está centralizado no ponto x:0 do player, porém ancorado pelo centro do collider
//             // Agora ele continuará no ponto x:0 do player, porém ancorado pela esquerda do collider.
//             // e considerando se ele está virado para esquerda ou para a direita
//             bool estaViradoParaDireita = oSpriteRenderer.flipX == false;
//             if (estaViradoParaDireita) {
//                 float nOffsetXParaDireita = Math.Abs(nNovoComprimentoCollider / 2);
//                 oBoxCollider2d.offset = new Vector2(
//                     nOffsetXParaDireita,
//                     oBoxCollider2d.offset.y
//                 );
//             } else {
//                 float nOffsetXParaEsquerda = -Math.Abs(nNovoComprimentoCollider / 2);
//                 oBoxCollider2d.offset = new Vector2(
//                     nOffsetXParaEsquerda,
//                     oBoxCollider2d.offset.y
//                 );
//             }
//         }

//     }

//     private void FixedUpdate()
//     {
//         // Impede movimentação horizontal durante o ataque (opcional)
//         if (isAttacking) return;

//         float speed = isRunning ? runSpeed : walkSpeed;
//         var xAxiesSpeed = speed * xAxiesInputDirection;
//         oRigidBody.linearVelocity = new Vector2(xAxiesSpeed, oRigidBody.linearVelocity.y);
//     }

//     private void OnTriggerEnter2D (Collider2D collision) 
//     {
//         isGrounded = true;
//         oAnimator.SetBool("isJumping", !isGrounded);
//     }

//     // CORROTINA DE ATAQUE
//     private IEnumerator RealizarAtaque()
//     {
//         isAttacking = true;
//         oAnimator.SetTrigger("Attack");
//         yield return new WaitForSeconds(attackDuration);
//         isAttacking = false;
//     }
// }

