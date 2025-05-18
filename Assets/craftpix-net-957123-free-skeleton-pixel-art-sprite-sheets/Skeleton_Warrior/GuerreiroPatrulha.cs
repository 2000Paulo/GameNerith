using UnityEngine;
using System.Collections;
using DbHelpers;

public class GuerreiroPatrulha : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaAtual = 100;
    public CanvasBarraDeVida CanvasBarraDeVida;

    [Header("Patrulha")]
    public float speed = 1f;
    public int leftSteps = 2;
    public int rightSteps = 2;
    public float waitTime = 2f;

    private Animator oAnimator;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingLeft = true;

    // ESTADOS
    private bool isWaiting = false;
    private bool isHurt = false;
    private float waitCounter = 0f;

    public float nTempoAnimacaoTomaDano;

    private void Start()
    {
        oAnimator = GetComponent<Animator>();
        startPosition = transform.position;
        SetNextTarget();
        // Debug.Log("Tempo de animacao: ");
        // Debug.Log(nTempoAnimacaoTomaDano);
        DefineTempoDasAnimacoes();
        // Debug.Log(nTempoAnimacaoTomaDano);


    }

    private void Update()
    {

        if (isWaiting)
        {
            oAnimator.SetBool("isHurt", isHurt);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                isWaiting = false;
                movingLeft = !movingLeft;
                SetNextTarget();
            }
        }
        else
        {
            oAnimator.SetBool("isWalking", true);

            // Direção visual
            Vector3 scale = transform.localScale;
            scale.x = targetPosition.x < transform.position.x ? -1 : 1;
            transform.localScale = scale;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                isWaiting = true;
                waitCounter = waitTime;
            }
        }
    }

    void SetNextTarget()
    {
        float direction = movingLeft ? -1 : 1;
        float steps = movingLeft ? leftSteps : rightSteps;
        targetPosition = startPosition + new Vector2(direction * steps, 0);
    }

    /*
    ************************************************************
    *   Controller para:                                       *
    *   -> Detectar o ataque do player, para então:            *
    *      -> Recebe dano                                      *
    *      -> Muda a animação: "esqueleto_animacao_toma_dano"  *         
    ************************************************************
    */
    void OnTriggerEnter2D(Collider2D jogadorColliderAtaque)
    {
        bool eTagPlayer = jogadorColliderAtaque.CompareTag("Player");
        if (!eTagPlayer) { return; }
        BoxCollider2D BoxCollider2dDoPlayer = jogadorColliderAtaque as BoxCollider2D;
        if (!BoxCollider2dDoPlayer) { return; }

        // toma dano
        vidaAtual = Mathf.Clamp(vidaAtual - 10, 0, vidaMaxima);
        CanvasBarraDeVida.AtualizaVida(vidaAtual, vidaMaxima);
        // muda a animação
        oAnimator.SetBool("isWalking", false);
        oAnimator.SetBool("isHurt", true);
    }

    void OnTriggerExit2D(Collider2D jogadorColliderAtaque)
    {
        bool eTagPlayer = jogadorColliderAtaque.CompareTag("Player");
        if (!eTagPlayer) { return; }
        BoxCollider2D BoxCollider2dDoPlayer = jogadorColliderAtaque as BoxCollider2D;
        if (!BoxCollider2dDoPlayer) { return; }
        StartCoroutine(esperaTempoAnimacaoTomaDano());
        IEnumerator esperaTempoAnimacaoTomaDano() {
            yield return new WaitForSeconds(nTempoAnimacaoTomaDano * 4);
            oAnimator.SetBool("isHurt", false);
            oAnimator.SetBool("isWalking", true);
        }
    }

    void DefineTempoDasAnimacoes()
    {
        var Animacoes = oAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip Animacao in Animacoes)
        {
            switch (Animacao.name)
            {
                case "skeleton.hurt":
                    nTempoAnimacaoTomaDano = Animacao.length;
                    break;
            }
        }
    }
}
