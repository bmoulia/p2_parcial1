using UnityEngine;

// ============================================================
// Obstacle.cs  (ACTUALIZADO)
// Cambio: marcamos que el obstaculo SI es destruible por el disparo
// del jugador (sobrescribimos EsDestruible). Como EnemyShip hereda de
// Obstacle, las naves tambien quedan destruibles automaticamente.
// ============================================================
public class Obstacle : WorldObject
{
    // Este objeto (y sus hijos, como EnemyShip) se puede destruir a tiros.
    public override bool EsDestruible
    {
        get { return true; }
    }

    protected override void Reaccionar(bool coincide, Player jugador)
    {
        if (coincide == false)
        {
            Debug.Log("Chocaste un obstaculo de polaridad distinta");
            GameManager.Instancia.JugadorMurio();
        }
        else
        {
            Debug.Log("Pasaste el obstaculo (misma polaridad)");
        }
    }
}