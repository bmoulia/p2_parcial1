using UnityEngine;

// ============================================================
// Spawner.cs  (ACTUALIZADO)
// Cambio: la dificultad (elegida en el menu, guardada en el GameManager)
// ahora define la CANTIDAD de objetos y la proporcion de enemigos.
// Mas dificultad = aparecen mas seguido y hay mas naves enemigas.
// La velocidad del mundo NO cambia con la dificultad.
// ============================================================
public class Spawner : MonoBehaviour
{
    [SerializeField] private float zSpawn = 40f;

    // Intervalos base (dificultad media). Los otros niveles los derivamos
    // multiplicando estos valores.
    [SerializeField] private float intervaloInicialBase = 1.8f;
    [SerializeField] private float intervaloMinimoBase = 0.6f;

    private float[] carriles = { -2.5f, 0f, 2.5f };
    private float tiempoDesdeUltimoSpawn;

    private void Update()
    {
        if (GameManager.Instancia.EstadoActual != EstadoJuego.Jugando)
        {
            return;
        }

        tiempoDesdeUltimoSpawn = tiempoDesdeUltimoSpawn + Time.deltaTime;

        float intervaloActual = CalcularIntervalo();

        if (tiempoDesdeUltimoSpawn >= intervaloActual)
        {
            tiempoDesdeUltimoSpawn = 0f;
            Generar();
        }
    }

    // Devuelve un multiplicador de cantidad segun la dificultad.
    // Menor multiplicador = intervalos mas cortos = MAS objetos.
    private float MultiplicadorIntervalo()
    {
        Dificultad dificultad = GameManager.Instancia.DificultadActual;

        if (dificultad == Dificultad.Normal)
        {
            return 1.4f; // intervalos mas largos -> menos objetos
        }
        else if (dificultad == Dificultad.Media)
        {
            return 1f;   // valores base
        }
        else // Dificil
        {
            return 0.6f; // intervalos mas cortos -> mas objetos
        }
    }

    private float CalcularIntervalo()
    {
        float progreso = GameManager.Instancia.DistanciaRecorrida / GameManager.Instancia.DistanciaObjetivo;
        if (progreso > 1f)
        {
            progreso = 1f;
        }

        // Interpolamos entre inicial y minimo segun el progreso...
        float intervalo = Mathf.Lerp(intervaloInicialBase, intervaloMinimoBase, progreso);

        // ...y lo escalamos segun la dificultad elegida.
        intervalo = intervalo * MultiplicadorIntervalo();
        return intervalo;
    }

    private void Generar()
    {
        TipoObjeto tipo = ElegirTipo();
        Polaridad polaridad = ElegirPolaridad();
        float x = ElegirCarril();

        WorldObject objeto = ObjectPool.Instancia.Obtener(tipo);
        objeto.transform.position = new Vector3(x, 0.5f, zSpawn);
        objeto.EstablecerPolaridad(polaridad);
    }

    private TipoObjeto ElegirTipo()
    {
        float progreso = GameManager.Instancia.DistanciaRecorrida / GameManager.Instancia.DistanciaObjetivo;

        // Probabilidad base de nave segun el progreso.
        float probabilidadNave = Mathf.Lerp(0.2f, 0.6f, progreso);

        // La dificultad suma probabilidad de nave enemiga.
        Dificultad dificultad = GameManager.Instancia.DificultadActual;
        if (dificultad == Dificultad.Normal)
        {
            probabilidadNave = probabilidadNave - 0.1f;
        }
        else if (dificultad == Dificultad.Dificil)
        {
            probabilidadNave = probabilidadNave + 0.2f;
        }
        // En Media queda como esta.

        float azar = Random.value;

        if (azar < probabilidadNave)
        {
            return TipoObjeto.NaveEnemiga;
        }
        else
        {
            float azar2 = Random.value;
            if (azar2 < 0.5f)
            {
                return TipoObjeto.Obstaculo;
            }
            else
            {
                return TipoObjeto.Coleccionable;
            }
        }
    }

    private Polaridad ElegirPolaridad()
    {
        int azar = Random.Range(0, 2);
        if (azar == 0)
        {
            return Polaridad.Cyan;
        }
        else
        {
            return Polaridad.Magenta;
        }
    }

    private float ElegirCarril()
    {
        int indice = Random.Range(0, carriles.Length);
        return carriles[indice];
    }
}