using UnityEngine;

public class Obstacle : WorldObject
{
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