using UnityEngine;
using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using DbHelpers;
using System.ComponentModel.Design.Serialization;
using UnityEngine.Analytics;

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

    // 1. Nova variável pública para a hitbox
    public GameObject hitboxAtaque;

    private void Start()
    {
        oRigidBody = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oSpriteRenderer = GetComponent<SpriteRenderer>();
        oCapsuleCollider2d = GetComponent<CapsuleCollider2D>();
        oBoxCollider2d = GetComponent<BoxCollider2D>();

        // 2. Garante que a hitbox comece desativada
        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(false);
        }
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

        ContactPoint2D[] aColisoesDetectadas = new ContactPoint2D[10];
        int popularaColisoesDetectadas = oRigidBody.GetContacts(aColisoesDetectadas);

        foreach (var oColisaoDetectada in aColisoesDetectadas)
        {
            if (
                oColisaoDetectada.collider?.name?.Trim().ToUpper() == "GROUND" ||
                oColisaoDetectada.rigidbody?.name?.Trim().ToUpper() == "GROUND"
            )
            {
                oAnimator.SetBool("noChao", true);
            }
        }

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

        // 3. Substitua o bloco de ataque por este:
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(RealizarAtaque());
        }

        // 6. Remover o bloco que verifica o nome do sprite para ataque
        // (bloco removido)
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

    // 4. Substitua o conteúdo da função RealizarAtaque
    private IEnumerator RealizarAtaque()
    {
        isAttacking = true;
        oAnimator.SetTrigger("podeAtacar");

        // Espera um pouco para a animação começar
        yield return new WaitForSeconds(0.1f);
        if (hitboxAtaque != null) hitboxAtaque.SetActive(true);

        // Deixa a hitbox ativa por um curto período
        yield return new WaitForSeconds(0.2f);
        if (hitboxAtaque != null) hitboxAtaque.SetActive(false);

        // Espera o resto da animação para poder atacar de novo
        yield return new WaitForSeconds(tempoAnimacaoDeAtaque - 0.3f);
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

    // 5. Função AjustaHitboxAtaque removida

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