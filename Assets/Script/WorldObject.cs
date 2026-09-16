using UnityEngine;

// ============================================================
// WorldObject.cs  (ACTUALIZADO)
// Cambios:
//  1) indicadorRenderer: un Renderer opcional que se pinta con el
//     color de la polaridad. Si queda vacio en el Inspector, se usa
//     el Renderer propio (asi los cubos siguen andando igual, y las
//     naves importadas pintan un cubito hijo como indicador).
//  2) Activar() pasa a virtual: la nave enemiga lo pisa para resetear
//     su temporizador de disparo cada vez que sale del pool.
// ============================================================
public abstract class WorldObject : MonoBehaviour, IPoolable
{
    [SerializeField] private Polaridad polaridad;
    [SerializeField] private float zDeReciclado = -10f;
    [SerializeField] private Renderer indicadorRenderer; // opcional (vacio = uso el mio)

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
    
    // Indica si el disparo del jugador puede destruir este objeto.
    // Por defecto NO. Las clases que sí (Obstacle, EnemyShip) lo pisan.
    public virtual bool EsDestruible
    {
        get { return false; }
    }

    protected virtual void Awake()
    {
        // Si no asignaron un indicador en el Inspector, uso mi propio Renderer.
        if (indicadorRenderer == null)
        {
            indicadorRenderer = GetComponent<Renderer>();
        }
    }

    protected virtual void Start()
    {
        ActualizarColor();
    }

    protected virtual void Update()
    {
        Mover();
        RevisarSalidaDeEscena();
    }

    protected virtual void Mover()
    {
        float velocidad = GameManager.Instancia.Velocidad;
        transform.Translate(Vector3.back * velocidad * Time.deltaTime, Space.World);
    }

    private void RevisarSalidaDeEscena()
    {
        if (transform.position.z < zDeReciclado)
        {
            ObjectPool.Instancia.Devolver(this);
        }
    }

    public void EstablecerPolaridad(Polaridad nueva)
    {
        polaridad = nueva;
        ActualizarColor();
    }

    private void ActualizarColor()
    {
        // Si no hay nada que pintar, no hacemos nada (evita errores en las naves).
        if (indicadorRenderer == null)
        {
            return;
        }

        if (polaridad == Polaridad.Cyan)
        {
            indicadorRenderer.material.color = Color.cyan;
        }
        else
        {
            indicadorRenderer.material.color = Color.magenta;
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

    public virtual void Activar()
    {
        gameObject.SetActive(true);
        ActualizarColor();
    }

    public void Desactivar()
    {
        gameObject.SetActive(false);
    }
}