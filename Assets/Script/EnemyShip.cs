using UnityEngine;

public class EnemyShip : Obstacle
{
    [SerializeField] private float intervaloMinimo = 0.8f;
    [SerializeField] private float intervaloMaximo = 2.5f;
    [SerializeField] private Transform[] puntosDisparo;

    private float tiempoDesdeUltimoDisparo;
    private float intervaloActual;
    private Transform jugadorTransform;

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instancia.EstadoActual != EstadoJuego.Jugando)
        {
            return;
        }

        tiempoDesdeUltimoDisparo = tiempoDesdeUltimoDisparo + Time.deltaTime;

        if (tiempoDesdeUltimoDisparo >= intervaloActual)
        {
            tiempoDesdeUltimoDisparo = 0f;
            SortearIntervalo(); // proximo disparo con una cadencia nueva
            Disparar();
        }
    }

    // Elige un intervalo al azar dentro del rango.
    private void SortearIntervalo()
    {
        intervaloActual = Random.Range(intervaloMinimo, intervaloMaximo);
    }

    private void Disparar()
    {
        if (jugadorTransform == null)
        {
            GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
            if (objJugador != null)
            {
                jugadorTransform = objJugador.transform;
            }
        }

        if (jugadorTransform == null)
        {
            return;
        }

        // Si ya paso al jugador no dispara mas.
        if (transform.position.z < jugadorTransform.position.z)
        {
            return;
        }

        for (int i = 0; i < puntosDisparo.Length; i++)
        {
            Transform punto = puntosDisparo[i];

            WorldObject objeto = ObjectPool.Instancia.Obtener(TipoObjeto.Proyectil);
            objeto.transform.position = punto.position;
            objeto.EstablecerPolaridad(Polaridad);

            Projectile disparo = objeto.GetComponent<Projectile>();
            disparo.Disparar(jugadorTransform.position);
        }
    }

    public override void Activar()
    {
        base.Activar();
        // Arranca lista para disparar apenas aparece con una cadencia sorteada.
        SortearIntervalo();
        tiempoDesdeUltimoDisparo = intervaloActual;
    }
}