using System;                        // para "event Action" (Observer)
using UnityEngine;
using UnityEngine.SceneManagement;   // para recargar la escena al reiniciar

// ============================================================
// GameManager.cs  (COMPLETO)
// El arbitro del juego. Responsabilidades:
//  - ser unico y accesible desde cualquier lado (SINGLETON)
//  - controlar la fase actual con enum + switch
//  - tener la velocidad global del mundo
//  - guardar la dificultad elegida en el menu
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
    [SerializeField] private float distanciaObjetivo = 250f;

    // --- Estado interno (privado = encapsulado) ---
    private EstadoJuego estadoActual;
    private Dificultad dificultadActual = Dificultad.Media; // por defecto, media
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
        // Arrancamos en el menu de inicio. Los botones de dificultad
        // configuran la partida y llaman a IniciarJuego().
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
                Time.timeScale = 0f;
                break;
            case EstadoJuego.Jugando:
                Time.timeScale = 1f;
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

    // La llaman los coleccionables al juntarse y el disparo al destruir.
    public void SumarPuntos(int cantidad)
    {
        puntaje = puntaje + cantidad;
        if (OnPuntajeCambiado != null)
        {
            OnPuntajeCambiado();
        }
    }

    // Configura la partida segun la dificultad elegida en el menu.
    // La dificultad se siente por la CANTIDAD de objetos (lo maneja el
    // Spawner leyendo DificultadActual). Aca solo guardamos el nivel y
    // ajustamos la distancia objetivo.
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
        Time.timeScale = 1f; // destrabamos el tiempo antes de recargar
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }
}