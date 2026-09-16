using UnityEngine;

// ============================================================
// Spawner.cs  (ACTUALIZADO)
// Cambio: DIFICULTAD PROGRESIVA. El intervalo entre spawns ya no es
// fijo: arranca lento y se va achicando segun la distancia recorrida
// (que leemos del GameManager), hasta un minimo. Ademas, cuanto mas
// lejos llegas, mas probable es que salga una nave enemiga.
// ============================================================
public class Spawner : MonoBehaviour
{
    [SerializeField] private float intervaloInicial = 1.8f; // al empezar (lento)
    [SerializeField] private float intervaloMinimo = 0.6f;  // lo mas rapido posible
    [SerializeField] private float zSpawn = 40f;

    private float[] carriles = { -2.5f, 0f, 2.5f };

    private float tiempoDesdeUltimoSpawn;

    private void Update()
    {
        if (GameManager.Instancia.EstadoActual != EstadoJuego.Jugando)
        {
            return;
        }

        tiempoDesdeUltimoSpawn = tiempoDesdeUltimoSpawn + Time.deltaTime;

        // El intervalo actual depende de cuanto avanzaste.
        float intervaloActual = CalcularIntervalo();

        if (tiempoDesdeUltimoSpawn >= intervaloActual)
        {
            tiempoDesdeUltimoSpawn = 0f;
            Generar();
        }
    }

    // Devuelve el intervalo segun el progreso (mas avance = mas rapido).
    private float CalcularIntervalo()
    {
        // Progreso de 0 (arranque) a 1 (llegaste al objetivo).
        float progreso = GameManager.Instancia.DistanciaRecorrida / GameManager.Instancia.DistanciaObjetivo;

        // Por las dudas, lo dejamos entre 0 y 1.
        if (progreso > 1f)
        {
            progreso = 1f;
        }

        // Interpolamos: en progreso 0 devuelve el inicial, en 1 el minimo.
        float intervalo = Mathf.Lerp(intervaloInicial, intervaloMinimo, progreso);
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

        // La probabilidad de nave enemiga sube con el avance:
        // arranca en 20% y llega hasta ~60% al final.
        float probabilidadNave = Mathf.Lerp(0.2f, 0.6f, progreso);

        float azar = Random.value; // numero al azar entre 0 y 1

        if (azar < probabilidadNave)
        {
            return TipoObjeto.NaveEnemiga;
        }
        else
        {
            // El resto se reparte mitad y mitad entre obstaculo y coleccionable.
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