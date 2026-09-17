using UnityEngine;
using TMPro;

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
        GameManager.Instancia.OnEstadoCambiado += ActualizarPantallas;
        GameManager.Instancia.OnDistanciaCambiada += ActualizarDistancia;
        GameManager.Instancia.OnPuntajeCambiado += ActualizarPuntaje;

        ActualizarPantallas();
        ActualizarDistancia();
        ActualizarPuntaje();
    }

    private void OnDestroy()
    {
        if (GameManager.Instancia != null)
        {
            GameManager.Instancia.OnEstadoCambiado -= ActualizarPantallas;
            GameManager.Instancia.OnDistanciaCambiada -= ActualizarDistancia;
            GameManager.Instancia.OnPuntajeCambiado -= ActualizarPuntaje;
        }
    }

    private void ActualizarPantallas()
    {
        EstadoJuego estado = GameManager.Instancia.EstadoActual;

        panelMenu.SetActive(false);
        panelHUD.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);

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

    // Botones de dificultad
    public void BotonDificultadNormal()
    {
        GameManager.Instancia.ConfigurarDificultad(Dificultad.Normal);
        GameManager.Instancia.IniciarJuego();
    }

    public void BotonDificultadMedia()
    {
        GameManager.Instancia.ConfigurarDificultad(Dificultad.Media);
        GameManager.Instancia.IniciarJuego();
    }

    public void BotonDificultadDificil()
    {
        GameManager.Instancia.ConfigurarDificultad(Dificultad.Dificil);
        GameManager.Instancia.IniciarJuego();
    }

    // --- Boton de reiniciar (victoria / derrota) ---
    public void BotonReiniciar()
    {
        GameManager.Instancia.ReiniciarJuego();
    }
}