using UnityEngine;

// ============================================================
// Player.cs  (ACTUALIZADO)
// Cambio: el jugador ahora DISPARA con click izquierdo. Cada disparo
// sale de su polaridad actual y se lo pide al pool. Hay un tiempo de
// espera entre tiros para que no se pueda disparar cada frame.
// ============================================================
public class Player : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento = 8f;
    [SerializeField] private float limiteX = 3.5f;
    [SerializeField] private float limiteZAdelante = 4f;
    [SerializeField] private float limiteZAtras = -2f;
    [SerializeField] private float tiempoEntreDisparos = 0.25f;

    private Polaridad polaridadActual;
    private Renderer miRenderer;
    private float tiempoDesdeUltimoDisparo;

    public Polaridad PolaridadActual
    {
        get { return polaridadActual; }
    }

    private void Awake()
    {
        miRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        polaridadActual = Polaridad.Cyan;
        ActualizarColor();
    }

    private void Update()
    {
        Mover();
        LeerCambioDePolaridad();
        LeerDisparo();
    }

    private void Mover()
    {
        float ejeHorizontal = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * ejeHorizontal * velocidadMovimiento * Time.deltaTime);

        float ejeVertical = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.forward * ejeVertical * velocidadMovimiento * Time.deltaTime);

        Vector3 posicion = transform.position;
        if (posicion.x > limiteX)
        {
            posicion.x = limiteX;
        }
        if (posicion.x < -limiteX)
        {
            posicion.x = -limiteX;
        }
        if (posicion.z > limiteZAdelante)
        {
            posicion.z = limiteZAdelante;
        }
        if (posicion.z < limiteZAtras)
        {
            posicion.z = limiteZAtras;
        }
        transform.position = posicion;
    }

    private void LeerCambioDePolaridad()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CambiarPolaridad();
        }
    }

    // Lee el click izquierdo para disparar, respetando el tiempo de espera.
    private void LeerDisparo()
    {
        // Solo se dispara mientras se esta jugando.
        if (GameManager.Instancia.EstadoActual != EstadoJuego.Jugando)
        {
            return;
        }

        tiempoDesdeUltimoDisparo = tiempoDesdeUltimoDisparo + Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && tiempoDesdeUltimoDisparo >= tiempoEntreDisparos)
        {
            tiempoDesdeUltimoDisparo = 0f;
            Disparar();
        }
    }

    private void Disparar()
    {
        // Pedimos un disparo al pool (reciclado, no Instantiate).
        WorldObject disparo = ObjectPool.Instancia.Obtener(TipoObjeto.DisparoJugador);

        // Lo ubicamos un poco adelante del jugador para que no se solape con el.
        disparo.transform.position = transform.position + new Vector3(0f, 0f, 1f);

        // El disparo sale de la MISMA polaridad que el jugador en este instante.
        disparo.EstablecerPolaridad(polaridadActual);
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