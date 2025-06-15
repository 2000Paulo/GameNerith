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
    private CapsuleCollider2D oCapsuleCollider2d;
    private BoxCollider2D oBoxCollider2d;

    // ESTADOS
    private bool isRunning;
    private bool correndo;
    private bool isWalking;
    // private bool pulando;
    private bool isGrounded;
    private bool isAttacking = false; // NOVO

    private Vector3 oPosMorte;

    // VARIÁVEIS QUE RECEBEM OS INPUTS
    private float xAxiesInputDirection; // input do eixo x ( útil para a movimentação para a esquerda ou direita )
    private float yAxiesInputDirection;

    // CONSTANTES QUE SÃO ACESSÍVEIS PELO "Inspector"
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpSpeed = 5f;
    public float tempoAnimacaoDeAtaque = 0.5f; // Duração da animação de ataque

    private void Start()
    {
        oRigidBody = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oSpriteRenderer = GetComponent<SpriteRenderer>();
        oCapsuleCollider2d = GetComponent<CapsuleCollider2D>();
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
        oAnimator.SetFloat("inputHorizontal", Input.GetAxis("Horizontal"));
        oAnimator.SetFloat("inputVertical", Input.GetAxis("Vertical"));
        oAnimator.SetFloat("velocidadeVertical", oRigidBody.linearVelocityY);

        if (oAnimator.GetBool("noChao") == true) { isGrounded = true; }

        if (Input.GetAxisRaw("Vertical") > 0 && isGrounded)
        {
            oRigidBody.linearVelocity = new Vector2(oRigidBody.linearVelocity.x, jumpSpeed);
            isGrounded = false;
            oAnimator.SetTrigger("podePular");
            oAnimator.SetBool("pulando", !isGrounded);
            oAnimator.SetBool("noChao", isGrounded);
        }


        // vira o sprite para a direita ou esquerda.
        // dependendo da direção indicada pelo input (<-, ->, A, D) no eixo X
        AjustaHitboxCorpoPersonagem();
        AjustaDirecaoSprite();


        if (
            oAnimator.GetBool("pulando") == true &&
            oAnimator.GetFloat("velocidadeVertical") == 0
        )
        {
            oAnimator.SetBool("pulando", false);
            oAnimator.SetBool("noChao", true);
            isGrounded = true;
        }

        StartCoroutine(Morre());

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(RealizarAtaque());
        }
        // AjustaHitboxAtaque("player_animacao_ataque");




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

    // MOD
    void OnCollisionEnter2D(Collision2D collision)
    {
        bool eOChao = collision.gameObject.CompareTag("Ground");
        if (eOChao)
        {
            isGrounded = true;
            oAnimator.SetBool("noChao", isGrounded);
        }
    }


    /*
    *********************
    *   Funções nossas  *                       
    *********************
    */
    private IEnumerator RealizarAtaque()
    {
        isAttacking = true;
        oAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(tempoAnimacaoDeAtaque);
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

    private void AjustaHitboxCorpoPersonagem()
    {
        oBoxCollider2d.size = new Vector2(
            oSpriteRenderer.bounds.size.x,
            oBoxCollider2d.size.y
        );
    }


    private IEnumerator Morre()
    {
        if (GetComponent<PlayerHealth>().currentHealth <= 0 && oAnimator.GetBool("morreu") == false)
        {
            oRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            oAnimator.SetBool("morreu", true);
            oPosMorte = new Vector3(
                transform.position.x,
                transform.position.y + (-oSpriteRenderer.bounds.extents.y),
                transform.position.z
            );

        }

        if (oSpriteRenderer.sprite.name.StartsWith("DEATH"))
        {
            transform.position = oPosMorte;

        }

        if (oSpriteRenderer.sprite.name.StartsWith("DEATH_8"))
        {
            yield return new WaitForSeconds(0.4f);
            this.gameObject.SetActive(false);

            string sNomeGameObjectPontoDeSpawn = GameManager.instance?.proximoSpawnPoint;
            GameObject oPontoDeSpawn = GameObject.Find(sNomeGameObjectPontoDeSpawn);

            if (oPontoDeSpawn == null) { yield break; }

            this.transform.position = oPontoDeSpawn.transform.position;
            
        }

    }

}