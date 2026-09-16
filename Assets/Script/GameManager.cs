using System;         // para poder usar "event Action" (los eventos del Observer)
using UnityEngine;
using UnityEngine.SceneManagement; // para recargar la escena al reiniciar

// ============================================================
// GameManager.cs
// El arbitro del juego. Responsabilidades:
//  - ser unico y accesible desde cualquier lado (SINGLETON)
//  - controlar la fase actual con enum + switch
//  - tener la velocidad global del mundo
//  - llevar la distancia (victoria) y el puntaje
//  - avisar a los interesados cuando algo cambia (OBSERVER)
// Patrones del parcial: SINGLETON y OBSERVER.
// ============================================================
public class GameManager : MonoBehaviour
{
    // --- SINGLETON: una unica instancia global ---
    public static GameManager Instancia;

    // --- Configurables desde el Inspector ---
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float distanciaObjetivo = 50f;

    // --- Estado interno (privado = encapsulado) ---
    private EstadoJuego estadoActual;
    private float distanciaRecorrida;
    private int puntaje;

    // --- Propiedades: se leen de afuera, no se escriben ---
    public float Velocidad
    {
        get { return velocidad; }
    }
    public EstadoJuego EstadoActual
    {
        get { return estadoActual; }
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

    // --- EVENTOS (OBSERVER): el GameManager avisa, otros escuchan ---
    public event Action OnEstadoCambiado;
    public event Action OnDistanciaCambiada;
    public event Action OnPuntajeCambiado;

    private void Awake()
    {
        // Singleton: si no hay instancia, esta es LA instancia.
        // Si ya habia otra, esta sobra y se destruye.
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
        // Arrancamos en el menu de inicio. El boton "Jugar" llamara a IniciarJuego().
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

        // Condicion de VICTORIA.
        if (distanciaRecorrida >= distanciaObjetivo)
        {
            CambiarEstado(EstadoJuego.Victoria);
        }
    }

    // El control central de fases: enum + switch.
    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;

        switch (estadoActual)
        {
            case EstadoJuego.Menu:
                Time.timeScale = 0f; // juego frenado en el menu
                break;
            case EstadoJuego.Jugando:
                Time.timeScale = 1f; // el mundo se mueve
                break;
            case EstadoJuego.Victoria:
                Time.timeScale = 0f;
                Debug.Log("VICTORIA! Llegaste a la meta");
                break;
            case EstadoJuego.Derrota:
                Time.timeScale = 0f;
                Debug.Log("DERROTA");
                break;
        }

        // Aviso a los observadores que el estado cambio.
        if (OnEstadoCambiado != null)
        {
            OnEstadoCambiado();
        }
    }

    // La llama el obstaculo cuando chocas con polaridad distinta.
    public void JugadorMurio()
    {
        CambiarEstado(EstadoJuego.Derrota);
    }

    // La llaman los coleccionables al juntarse.
    public void SumarPuntos(int cantidad)
    {
        puntaje = puntaje + cantidad;
        if (OnPuntajeCambiado != null)
        {
            OnPuntajeCambiado();
        }
    }

    // Reinicia los valores y arranca la partida.
    // La va a usar el boton "Jugar" del menu.
    public void IniciarJuego()
    {
        distanciaRecorrida = 0f;
        puntaje = 0;
        CambiarEstado(EstadoJuego.Jugando);
    }

        // Reinicia la partida recargando la escena completa.
    // Es la forma mas simple y segura de dejar todo en cero: pool,
    // objetos activos, posiciones, todo vuelve al estado inicial.
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f; // destrabamos el tiempo antes de recargar
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }
}