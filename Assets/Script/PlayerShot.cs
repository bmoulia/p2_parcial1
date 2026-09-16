using UnityEngine;

// ============================================================
// PlayerShot.cs  (ACTUALIZADO)
// Cambio: ahora detecta impactos. Cuando toca un WorldObject que es
// destruible, aplica la REGLA B: si la polaridad del objeto es la
// OPUESTA a la del disparo, lo destruye (ambos vuelven al pool).
// Si es del mismo color, el disparo lo atraviesa sin efecto.
// ============================================================
public class PlayerShot : WorldObject
{
    [SerializeField] private float velocidadDisparo = 25f;
    [SerializeField] private float limiteZAdelante = 50f;

    public override bool EsDestruible
    {
        get { return false; } // al disparo del jugador no se le dispara
    }

    protected override void Reaccionar(bool coincide, Player jugador)
    {
        // No hace nada: el disparo no reacciona al jugador.
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

        // Si lo que tocamos no es un WorldObject, ignoramos.
        if (objeto == null)
        {
            return;
        }

        // Solo le pega a cosas destruibles (obstaculos, naves).
        if (objeto.EsDestruible == false)
        {
            return;
        }

        // REGLA B: se destruye solo si es del color OPUESTO al disparo.
        if (objeto.Polaridad != this.Polaridad)
        {
            // Destruimos el objeto enemigo...
            ObjectPool.Instancia.Devolver(objeto);
            // ...y sumamos puntos por eliminarlo.
            GameManager.Instancia.SumarPuntos(1);
            // ...y el disparo tambien se consume.
            ObjectPool.Instancia.Devolver(this);
        }
        // Si es del mismo color, no hace nada: el disparo sigue de largo.
    }
}