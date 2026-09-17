using UnityEngine;

public class PlayerShot : WorldObject
{
    [SerializeField] private float velocidadDisparo = 25f;
    [SerializeField] private float limiteZAdelante = 50f;

    public override bool EsDestruible
    {
        get { return false; }
    }

    protected override void Reaccionar(bool coincide, Player jugador)
    {
        
    }

    protected override void Mover()
    {
        transform.Translate(Vector3.forward * velocidadDisparo * Time.deltaTime, Space.World);
    }

    protected override void Update()
    {
        Mover();

        if (transform.position.z > limiteZAdelante)
        {
            ObjectPool.Instancia.Devolver(this);
        }
    }

    // Detecta cuando el disparo toca a otro objeto.
    private void OnTriggerEnter(Collider other)
    {
        WorldObject objeto = other.GetComponent<WorldObject>();

        // Si lo que tocamos no es un WorldObject, ignoro
        if (objeto == null)
        {
            return;
        }

        // Solo le pega a cosas que puedo destruir.
        if (objeto.EsDestruible == false)
        {
            return;
        }


        if (objeto.Polaridad != this.Polaridad)
        {
            // Destruimos el objeto enemigo
            ObjectPool.Instancia.Devolver(objeto);
            GameManager.Instancia.SumarPuntos(1);
            ObjectPool.Instancia.Devolver(this);
        }
        // Si es del mismo color el disparo sigue de largo.
    }
}