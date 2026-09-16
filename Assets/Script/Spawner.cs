using UnityEngine;

// ============================================================
// Spawner.cs
// Genera objetos en la pista cada cierto tiempo, pidiendoselos
// al ObjectPool (no usa Instantiate: recicla). Elige al azar el
// tipo (obstaculo/coleccionable), el carril (X) y la polaridad.
// Solo genera mientras el juego esta en estado Jugando.
// Responsabilidad unica: decidir QUE, DONDE y CUANDO aparece algo.
// El ciclo de vida del objeto es cosa del pool, no del spawner.
// ============================================================
public class Spawner : MonoBehaviour
{
    [SerializeField] private float intervaloSpawn = 1.5f;
    [SerializeField] private float zSpawn = 40f;

    // Carriles fijos donde pueden aparecer los objetos.
    // (Es un arreglo simple: NO cuenta como estructura del parcial,
    //  solo organiza las 3 posiciones posibles en X.)
    private float[] carriles = { -2.5f, 0f, 2.5f };

    private float tiempoDesdeUltimoSpawn;

    private void Update()
    {
        // Solo generamos si el juego esta en curso.
        if (GameManager.Instancia.EstadoActual != EstadoJuego.Jugando)
        {
            return;
        }

        tiempoDesdeUltimoSpawn = tiempoDesdeUltimoSpawn + Time.deltaTime;

        if (tiempoDesdeUltimoSpawn >= intervaloSpawn)
        {
            tiempoDesdeUltimoSpawn = 0f;
            Generar();
        }
    }

    private void Generar()
    {
        TipoObjeto tipo = ElegirTipo();
        Polaridad polaridad = ElegirPolaridad();
        float x = ElegirCarril();

        // Le pedimos el objeto al pool (reciclado, no creado de cero).
        WorldObject objeto = ObjectPool.Instancia.Obtener(tipo);

        // Lo ubicamos al fondo de la pista, en el carril elegido.
        objeto.transform.position = new Vector3(x, 0.5f, zSpawn);

        // Le asignamos la polaridad al azar (esto tambien lo repinta).
        objeto.EstablecerPolaridad(polaridad);
    }

    private TipoObjeto ElegirTipo()
    {
        int azar = Random.Range(0, 2); // devuelve 0 o 1
        if (azar == 0)
        {
            return TipoObjeto.Obstaculo;
        }
        else
        {
            return TipoObjeto.Coleccionable;
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
        int indice = Random.Range(0, carriles.Length); // 0, 1 o 2
        return carriles[indice];
    }
}