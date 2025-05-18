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

    private void Start()
    {
        oAnimator = GetComponent<Animator>();
        startPosition = transform.position;
        SetNextTarget();
    }

    private void Update()
    {
        bool bEstaApertandoBotaoEsquerdoMouse = Input.GetMouseButton(0);
        bool bEstaApertandoBotaoDireitoMouse = Input.GetMouseButton(1);

        if (bEstaApertandoBotaoEsquerdoMouse)
        {
            vidaAtual += 10;
        }

        if (bEstaApertandoBotaoDireitoMouse)
        {
            vidaAtual -= 10;
        }
        // a vida do inimigo só pode estar entre 0 e a vida máxima
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        // vidaAtual = Math.Clamp()


        // DbDebugger.DebugObject()
        //  Input.GetKey(KeyCode.LeftShift)
        var bApertouEspaco = Input.GetKey(KeyCode.Space);
        // Debug.Log(bApertouEspaco);

        if (bApertouEspaco)
        {
            isHurt = true;
        }
        else
        {
            isHurt = false;
         }
        oAnimator.SetBool("isHurt", isHurt);
        //  Debug.Log(Input.GetKey(KeyCode.Space));
        if (isWaiting)
        {
            oAnimator.SetBool("isHurt", false);
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
}
