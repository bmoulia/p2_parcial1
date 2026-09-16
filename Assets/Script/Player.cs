using UnityEngine;

// ============================================================
// Player.cs
// Responsabilidad: manejar al jugador -> movimiento lateral (A/D),
// cambio de polaridad (Space) y color segun la polaridad actual.
// Concepto del parcial que toca: ENCAPSULAMIENTO
// (campo privado + propiedad publica de solo lectura).
// ============================================================
public class Player : MonoBehaviour
{
    // --- Campos configurables desde el Inspector ---
    [SerializeField] private float velocidadMovimiento = 8f;
    [SerializeField] private float limiteX = 3.5f;

    // --- Campos privados internos ---
    private Polaridad polaridadActual;
    private Renderer miRenderer;

    // --- Propiedad publica: los demas scripts LEEN mi polaridad,
    //     pero no pueden cambiarla (solo get). ---
    public Polaridad PolaridadActual
    {
        get { return polaridadActual; }
    }

    private void Awake()
    {
        // Guardamos la referencia al Renderer una sola vez.
        miRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        // El jugador arranca en Cyan.
        polaridadActual = Polaridad.Cyan;
        ActualizarColor();
    }

    private void Update()
    {
        Mover();
        LeerCambioDePolaridad();
    }

    // Movimiento lateral con A/D (o flechas izquierda/derecha).
    private void Mover()
    {
        float ejeHorizontal = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * ejeHorizontal * velocidadMovimiento * Time.deltaTime);

        // Que el jugador no se salga de la pista.
        Vector3 posicion = transform.position;
        if (posicion.x > limiteX)
        {
            posicion.x = limiteX;
        }
        if (posicion.x < -limiteX)
        {
            posicion.x = -limiteX;
        }
        transform.position = posicion;
    }

    // Detecta si se apreto Space para alternar la polaridad.
    private void LeerCambioDePolaridad()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CambiarPolaridad();
        }
    }

    private void CambiarPolaridad()
    {
        if (polaridadActual == Polaridad.Cyan)
        {
            polaridadActual = Polaridad.Magenta;
        }
        else
        {
            polaridadActual = Polaridad.Cyan;
        }
        ActualizarColor();
    }

    // Pinta el cubo del color de su polaridad actual.
    private void ActualizarColor()
    {
        if (polaridadActual == Polaridad.Cyan)
        {
            miRenderer.material.color = Color.cyan;
        }
        else
        {
            miRenderer.material.color = Color.magenta;
        }
    }
}