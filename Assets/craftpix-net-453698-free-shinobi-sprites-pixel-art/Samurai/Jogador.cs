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
using UnityEngine.Tilemaps;

public class Jogador : MonoBehaviour
{
    // COMPONENTES
    private Rigidbody2D oRigidBody;
    private Animator oAnimator;
    private SpriteRenderer oSpriteRenderer;
    private BoxCollider2D oBoxCollider2d;

    // ESTADOS
    private bool bTravaParametrosAnimator = false;
    private bool isGrounded;
    private String sAtaqueAtual = "ataqueNormal";
    private Vector3 oPosMorte;

    // CONSTANTES QUE SÃO ACESSÍVEIS PELO "Inspector"
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpSpeed = 5f;

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

        AjustaDirecaoSprite();
        AjustaHitboxCorpoPersonagem();
        AjustaHitboxAtaque();

        if (oSpriteRenderer.sprite.name.ToUpper() == "CLIMBING_7")
        {
            bTravaParametrosAnimator = false;
            oAnimator.SetTrigger("parouDeEscalar");
            GameObject.Find("Ground").GetComponent<TilemapCollider2D>().enabled = true;
            oRigidBody.constraints = RigidbodyConstraints2D.None;
            oRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.transform.position = new Vector3(
                oSpriteRenderer.bounds.center.x + (oSpriteRenderer.bounds.extents.x / 2),
                oSpriteRenderer.bounds.center.y,
                0
            );
        }

        if (bTravaParametrosAnimator) { return; }

        ContactPoint2D[] aColisoesDetectadas = new ContactPoint2D[10];
        int popularaColisoesDetectadas = oRigidBody.GetContacts(aColisoesDetectadas);

        foreach (var oContato in aColisoesDetectadas)
        {

            if (
                oContato.collider?.name?.Trim().ToUpper()  == "GROUND" ||
                oContato.rigidbody?.name?.Trim().ToUpper() == "GROUND"
            )
            {
                oAnimator.SetBool("noChao", true);
            }




        }

        oAnimator.SetFloat("inputHorizontal"   , Input.GetAxis("Horizontal"));
        oAnimator.SetFloat("inputVertical"     , Input.GetAxis("Vertical"));
        oAnimator.SetFloat("velocidadeVertical", oRigidBody.linearVelocityY);

        if (oAnimator.GetBool("noChao") == true) { isGrounded = true; }

        if (Input.GetAxisRaw("Vertical") > 0 && oAnimator.GetBool("noChao") == true)
        {
            oRigidBody.linearVelocity = new Vector2(oRigidBody.linearVelocity.x, jumpSpeed);
            isGrounded = false;
            oAnimator.SetTrigger("podePular");
            oAnimator.SetBool("pulando", true);
            oAnimator.SetBool("noChao" , false);
        }




        if (
            oAnimator.GetBool("pulando") == true &&
            oRigidBody.linearVelocityY == 0
        )
        {
            oAnimator.SetBool("pulando", false);
            oAnimator.SetBool("noChao" , true);
            isGrounded = true;
        }

        StartCoroutine(Morre());




        if (Input.GetKeyDown(KeyCode.Space) && !oSpriteRenderer.sprite.name.StartsWith("ATTACK"))
        {
            oAnimator.SetTrigger("podeAtacar");
            oAnimator.SetTrigger(sAtaqueAtual);
            sAtaqueAtual = sAtaqueAtual == "ataqueNormal" ? "ataqueForte" : "ataqueNormal";

        }

        if (
            oSpriteRenderer.sprite.name.ToUpper().Contains("ATTACK") &&
            (
                oSpriteRenderer.sprite.name.Contains("2_6") ||
                oSpriteRenderer.sprite.name.Contains("3_5")
            )
        )
        {
            oAnimator.SetTrigger("parouDeAtacar");
        }


    }

    private void FixedUpdate()
    {
        float nVelocidade = Math.Abs(Input.GetAxis("Horizontal")) == 1 ? runSpeed : walkSpeed;

        oRigidBody.linearVelocity = new Vector2(
            nVelocidade * Input.GetAxisRaw("Horizontal"),
            oRigidBody.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D oContato)
    {
        if (oContato.tag.ToUpper() == "PONTADEPAREDE" && oRigidBody.linearVelocityY > 0)
        {
            if (oContato.gameObject.layer == 12) { oSpriteRenderer.flipX = false; } else { oSpriteRenderer.flipX = true; }
            oAnimator.SetFloat("inputHorizontal"   , 0);
            oAnimator.SetFloat("inputVertical"     , 0);
            oAnimator.SetFloat("velocidadeVertical", 0);
            oAnimator.SetBool("noChao", false);
            oAnimator.SetBool("pulando", false);
            bTravaParametrosAnimator = true;
            oAnimator.SetTrigger("podeEscalar");
            GameObject.Find("Ground").GetComponent<TilemapCollider2D>().enabled = false;
            oRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            this.transform.position = GameObject.Find("PontaDeParede/Ancora").GetComponent<CircleCollider2D>().bounds.center;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.ToUpper() == "GROUND")
        {
            isGrounded = true;
            oAnimator.SetBool("noChao", true);
        }
    }

    /*
    *********************
    *   Funções nossas  *                       
    *********************
    */

    private void AjustaDirecaoSprite()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            oSpriteRenderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            oSpriteRenderer.flipX = true;
        }
    }

    private void AjustaHitboxAtaque()
    {
        BoxCollider2D oHitboxCollider = GameObject.Find("PlayerAtualizado/HitboxAtaque").GetComponent<BoxCollider2D>();

        oHitboxCollider.size = new Vector2(
            (oSpriteRenderer.sprite.rect.width / oSpriteRenderer.sprite.pixelsPerUnit) / 2,
            (oSpriteRenderer.sprite.rect.height / oSpriteRenderer.sprite.pixelsPerUnit)
        );

        oHitboxCollider.offset = new Vector2(
            (oSpriteRenderer.sprite.rect.width / oSpriteRenderer.sprite.pixelsPerUnit) / 4,
            0
        );

        oHitboxCollider.enabled = false;
        if (
            oSpriteRenderer.sprite.name.StartsWith("ATTACK") &&
            (
                oSpriteRenderer.sprite.name.Contains("2_3") ||
                oSpriteRenderer.sprite.name.Contains("2_4") ||
                oSpriteRenderer.sprite.name.Contains("3_2") ||
                oSpriteRenderer.sprite.name.Contains("3_3")
            )
        )
        {
            oHitboxCollider.enabled = true;
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