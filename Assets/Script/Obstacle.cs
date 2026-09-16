using UnityEngine;

// ============================================================
// Obstacle.cs  (ACTUALIZADO)
// Cambio: en vez de congelar el tiempo a mano, avisa al
// GameManager que el jugador murio.
// ============================================================
public class Obstacle : WorldObject
{
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