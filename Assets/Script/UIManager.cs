using UnityEngine;
using TMPro; // para los textos TextMeshPro desde codigo

// ============================================================
// UIManager.cs  (COMPLETO)
// Maneja TODAS las pantallas y el HUD. Se suscribe a los eventos
// del GameManager (patron OBSERVER):
//  - OnEstadoCambiado    -> muestra menu / HUD / victoria / derrota
//  - OnDistanciaCambiada -> actualiza el texto de distancia
//  - OnPuntajeCambiado   -> actualiza el texto de puntaje
// El GameManager avisa; el UIManager reacciona. Desacople total:
// el GameManager no sabe que la UI existe.
// ============================================================
public class UIManager : MonoBehaviour
{
    // --- Paneles de cada pantalla ---
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;

    // --- Textos del HUD ---
    [SerializeField] private TextMeshProUGUI textoDistancia;
    [SerializeField] private TextMeshProUGUI textoPuntaje;

    private void Start()
    {
        // OBSERVER: nos suscribimos a los tres eventos.
        // Lo hacemos en Start para asegurarnos de que el GameManager ya exista.
        GameManager.Instancia.OnEstadoCambiado += ActualizarPantallas;
        GameManager.Instancia.OnDistanciaCambiada += ActualizarDistancia;
        GameManager.Instancia.OnPuntajeCambiado += ActualizarPuntaje;

        // Mostramos el estado inicial correcto.
        ActualizarPantallas();
        ActualizarDistancia();
        ActualizarPuntaje();
    }

    private void OnDestroy()
    {
        // Nos desuscribimos al destruirse (evita errores al recargar la escena).
        if (GameManager.Instancia != null)
        {
            GameManager.Instancia.OnEstadoCambiado -= ActualizarPantallas;
            GameManager.Instancia.OnDistanciaCambiada -= ActualizarDistancia;
            GameManager.Instancia.OnPuntajeCambiado -= ActualizarPuntaje;
        }
    }

    // Muestra el panel correcto segun el estado del juego.
    private void ActualizarPantallas()
    {
        EstadoJuego estado = GameManager.Instancia.EstadoActual;

        // Apagamos todo primero.
        panelMenu.SetActive(false);
        panelHUD.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);

        // Prendemos el que corresponde.
        if (estado == EstadoJuego.Menu)
        {
            panelMenu.SetActive(true);
        }
        else if (estado == EstadoJuego.Jugando)
        {
            panelHUD.SetActive(true);
        }
        else if (estado == EstadoJuego.Victoria)
        {
            panelVictoria.SetActive(true);
        }
        else if (estado == EstadoJuego.Derrota)
        {
            panelDerrota.SetActive(true);
        }
    }

    private void ActualizarDistancia()
    {
        int distancia = (int)GameManager.Instancia.DistanciaRecorrida;
        int objetivo = (int)GameManager.Instancia.DistanciaObjetivo;
        textoDistancia.text = "Distancia: " + distancia + " / " + objetivo;
    }

    private void ActualizarPuntaje()
    {
        textoPuntaje.text = "Puntaje: " + GameManager.Instancia.Puntaje;
    }

    // --- Botones ---
    public void BotonJugar()
    {
        GameManager.Instancia.IniciarJuego();
    }

    public void BotonReiniciar()
    {
        GameManager.Instancia.ReiniciarJuego();
    }
}