using UnityEngine;
using System;
using System.Runtime;
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


    // ESTADOS
    private bool isRunning;
    private bool isWalking;


    // VARIÁVEIS QUE RECEBEM OS INPUTS
    private float xAxiesInputDirection; // input do eixo x ( útil para a movimentação para a esquerda ou direita )
    // private int

    // CONSTANTES QUE SÃO ACESSÍVEIS PELO "Inspector"
    public float walkSpeed = 2f;
    public float runSpeed = 5f;


    private void Start()
    {
        oRigidBody = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oSpriteRenderer = GetComponent<SpriteRenderer>();    
    }

    private void Update()
    {
        // input.GetAxisRaw("Horizontal") pode retornar 3 valores (lembre do plano cartesiano):
        // -1 : para a esquerda
        // 0  : parado
        // 1  : para a direita
        xAxiesInputDirection = Input.GetAxisRaw("Horizontal");
    
        isWalking = xAxiesInputDirection != 0 ? true : false;
        isRunning = Input.GetKey(KeyCode.LeftShift) && xAxiesInputDirection != 0;

        oAnimator.SetBool("isWalking", isWalking);
        oAnimator.SetBool("isRunning", isRunning);

        var goLeft  = xAxiesInputDirection < 0 ? true : false;
        var goRight = xAxiesInputDirection > 0 ? true : false;

        // O personagem precisa de apenas uma animação de corrida. Supondo que seja para a direita
        // Para conseguirmos a animação de corrida para a esquerda é necessário apenas
        // Inverter a imagem do personagem no eixo x.
        if (goLeft) {
            oSpriteRenderer.flipX = true;
        }

        if (goRight) {
            oSpriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        var xAxiesSpeed = speed * xAxiesInputDirection;
        oRigidBody.linearVelocity = new Vector2(xAxiesSpeed, oRigidBody.linearVelocity.y);
    }
}
