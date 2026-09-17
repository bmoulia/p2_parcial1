using System; 
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float distanciaObjetivo = 250f;

    private EstadoJuego estadoActual;
    private Dificultad dificultadActual = Dificultad.Media; 
    private float distanciaRecorrida;
    private int puntaje;

    public float Velocidad
    {
        get { return velocidad; }
    }
    public EstadoJuego EstadoActual
    {
        get { return estadoActual; }
    }
    public Dificultad DificultadActual
    {
        get { return dificultadActual; }
    }
    public float DistanciaRecorrida
    {
        get { return distanciaRecorrida; }
    }
    public float DistanciaObjetivo
    {
        get { return distanciaObjetivo; }
    }
    public int Puntaje
    {
        get { return puntaje; }
    }
    public event Action OnEstadoCambiado;
    public event Action OnDistanciaCambiada;
    public event Action OnPuntajeCambiado;

    private void Awake()
    {
    
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Arrancamos en el menu de inicio. Los botones de dificultad
        // configuran la partida y llaman a IniciarJuego()
        CambiarEstado(EstadoJuego.Menu);
    }

    private void Update()
    {
        if (estadoActual == EstadoJuego.Jugando)
        {
            AvanzarDistancia();
        }
    }

    private void AvanzarDistancia()
    {
        distanciaRecorrida = distanciaRecorrida + (velocidad * Time.deltaTime);

        // Aviso a los observadores que la distancia cambio.
        if (OnDistanciaCambiada != null)
        {
            OnDistanciaCambiada();
        }

        // Condicion de Victoria.
        if (distanciaRecorrida >= distanciaObjetivo)
        {
            CambiarEstado(EstadoJuego.Victoria);
        }
    }

    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;

        switch (estadoActual)
        {
            case EstadoJuego.Menu:
                Time.timeScale = 0f;
                break;
            case EstadoJuego.Jugando:
                Time.timeScale = 1f;
                break;
            case EstadoJuego.Victoria:
                Time.timeScale = 0f;
                Debug.Log("GANASTE! Llegaste a la meta");
                break;
            case EstadoJuego.Derrota:
                Time.timeScale = 0f;
                Debug.Log("DERROTA");
                break;
        }

        // Cambio el estado.
        if (OnEstadoCambiado != null)
        {
            OnEstadoCambiado();
        }
    }

    public void JugadorMurio()
    {
        CambiarEstado(EstadoJuego.Derrota);
    }
    public void SumarPuntos(int cantidad)
    {
        puntaje = puntaje + cantidad;
        if (OnPuntajeCambiado != null)
        {
            OnPuntajeCambiado();
        }
    }

    // Configuro la partida segun la dificultad elegida en el menu.
    public void ConfigurarDificultad(Dificultad dificultad)
    {
        dificultadActual = dificultad;

        if (dificultad == Dificultad.Normal)
        {
            distanciaObjetivo = 150f;
        }
        else if (dificultad == Dificultad.Media)
        {
            distanciaObjetivo = 250f;
        }
        else // Dificil
        {
            distanciaObjetivo = 400f;
        }
    }

    // Reinicia los valores y arranca la partida.
    public void IniciarJuego()
    {
        distanciaRecorrida = 0f;
        puntaje = 0;
        CambiarEstado(EstadoJuego.Jugando);
    }

    // Reinicia la partida recargando la escena completa.
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }
}