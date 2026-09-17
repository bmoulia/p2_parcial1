using System.Collections.Generic; // List, Queue y Dictionary
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instancia;

    [SerializeField] private WorldObject prefabObstaculo;
    [SerializeField] private WorldObject prefabColeccionable;
    [SerializeField] private WorldObject prefabProyectil;
    [SerializeField] private WorldObject[] prefabsNaveEnemiga;
    [SerializeField] private WorldObject prefabDisparoJugador;
    [SerializeField] private int cantidadInicial = 5;
    private Dictionary<TipoObjeto, Queue<WorldObject>> objetosLibres;
    private List<WorldObject> objetosActivos;

    private void Awake()
    {
        // Singleton
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Creo las estructuras vacias
        objetosLibres = new Dictionary<TipoObjeto, Queue<WorldObject>>();
        objetosActivos = new List<WorldObject>();
    }

    private void Start()
    {
        Precargar();
    }

    // Precargo cada tipo
    private void Precargar()
    {
        CrearColaParaTipo(TipoObjeto.Obstaculo);
        CrearColaParaTipo(TipoObjeto.Coleccionable);
        CrearColaParaTipo(TipoObjeto.Proyectil);
        CrearColaParaTipo(TipoObjeto.NaveEnemiga);
        CrearColaParaTipo(TipoObjeto.DisparoJugador);

        Debug.Log("Pool listo. Obstaculos libres: " + objetosLibres[TipoObjeto.Obstaculo].Count
            + " | Coleccionables libres: " + objetosLibres[TipoObjeto.Coleccionable].Count
            + " | Activos: " + objetosActivos.Count);
    }

    private void CrearColaParaTipo(TipoObjeto tipo)
    {
        Queue<WorldObject> cola = new Queue<WorldObject>();

        for (int i = 0; i < cantidadInicial; i++)
        {
            WorldObject nuevo = CrearNuevo(tipo);
            cola.Enqueue(nuevo);
        }

        // Guardamos la cola de ese tipo en el diccionario.
        objetosLibres.Add(tipo, cola);
    }

    // Instancia un objeto nuevo, lo etiqueta con su tipo y lo apaga.
    private WorldObject CrearNuevo(TipoObjeto tipo)
    {
        WorldObject prefab = ObtenerPrefab(tipo);
        WorldObject nuevo = Instantiate(prefab);
        nuevo.Tipo = tipo;
        nuevo.transform.SetParent(transform); // ordena la Hierarchy
        nuevo.Desactivar();
        return nuevo;
    }

    private WorldObject ObtenerPrefab(TipoObjeto tipo)
    {
        if (tipo == TipoObjeto.Obstaculo)
        {
            return prefabObstaculo;
        }
        else if (tipo == TipoObjeto.Coleccionable)
        {
            return prefabColeccionable;
        }
        else if (tipo == TipoObjeto.Proyectil)
        {
            return prefabProyectil;
        }
        else if (tipo == TipoObjeto.NaveEnemiga)
        {
            int indice = Random.Range(0, prefabsNaveEnemiga.Length);
            return prefabsNaveEnemiga[indice];
        }
        else
        {
            return prefabDisparoJugador;
        }
    }

    // aca un objeto del tipo pedido que despues lo manda al spawner
    public WorldObject Obtener(TipoObjeto tipo)
    {
        Queue<WorldObject> cola = objetosLibres[tipo];
        WorldObject objeto;

        if (cola.Count > 0)
        {
            objeto = cola.Dequeue();
        }
        else
        {
            // Si no quedan, el pool crece y creo uno nuevo.
            objeto = CrearNuevo(tipo);
        }

        objetosActivos.Add(objeto);
        objeto.Activar();
        return objeto;
    }
    public void Devolver(WorldObject objeto)
    {
        objetosActivos.Remove(objeto);
        objeto.Desactivar();

        Queue<WorldObject> cola = objetosLibres[objeto.Tipo];
        cola.Enqueue(objeto);
    }
    public void DevolverTodos()
    {
        List<WorldObject> copia = new List<WorldObject>(objetosActivos);

        foreach (WorldObject objeto in copia)
        {
            Devolver(objeto);
        }
    }
}