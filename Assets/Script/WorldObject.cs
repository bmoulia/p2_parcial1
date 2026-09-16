using UnityEngine;

// ============================================================
// WorldObject.cs  (ACTUALIZADO)
// Cambio: nuevo metodo EstablecerPolaridad(). El Spawner lo usa
// para asignarle una polaridad al azar a cada objeto que genera,
// y de paso lo repinta. La polaridad sigue siendo privada: solo
// se cambia a traves de este metodo controlado (encapsulamiento).
// ============================================================
public abstract class WorldObject : MonoBehaviour, IPoolable
{
    [SerializeField] private Polaridad polaridad;
    [SerializeField] private float zDeReciclado = -10f;

    private Renderer miRenderer;
    private TipoObjeto tipo;

    public Polaridad Polaridad
    {
        get { return polaridad; }
    }

    public TipoObjeto Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    private void Awake()
    {
        miRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        ActualizarColor();
    }

    private void Update()
    {
        Mover();
        RevisarSalidaDeEscena();
    }

    private void Mover()
    {
        float velocidad = GameManager.Instancia.Velocidad;
        transform.Translate(Vector3.back * velocidad * Time.deltaTime);
    }

    private void RevisarSalidaDeEscena()
    {
        if (transform.position.z < zDeReciclado)
        {
            ObjectPool.Instancia.Devolver(this);
        }
    }

    // Cambia la polaridad de forma controlada y repinta el objeto.
    // La usa el Spawner al generar cada objeto con polaridad al azar.
    public void EstablecerPolaridad(Polaridad nueva)
    {
        polaridad = nueva;
        ActualizarColor();
    }

    private void ActualizarColor()
    {
        if (polaridad == Polaridad.Cyan)
        {
            miRenderer.material.color = Color.cyan;
        }
        else
        {
            miRenderer.material.color = Color.magenta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player jugador = other.GetComponent<Player>();
            ResolverContacto(jugador);
        }
    }

    private void ResolverContacto(Player jugador)
    {
        bool coincide = (jugador.PolaridadActual == polaridad);
        Reaccionar(coincide, jugador);
    }

    protected abstract void Reaccionar(bool coincide, Player jugador);

    public void Activar()
    {
        gameObject.SetActive(true);
        ActualizarColor();
    }

    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}