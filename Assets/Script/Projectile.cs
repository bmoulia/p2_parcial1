using UnityEngine;


public class Projectile : Obstacle
{
    [SerializeField] private float velocidadDisparo = 18f;

    // La direccion fija en la que viaja. Se setea al dispararse.
    private Vector3 direccion;

    public void Disparar(Vector3 posicionObjetivo)
    {
        // Vector desde el disparo hacia el jugador, normalizado (largo 1).
        direccion = posicionObjetivo - transform.position;
        direccion = direccion.normalized;
    }

    protected override void Mover()
    {
        transform.Translate(direccion * velocidadDisparo * Time.deltaTime, Space.World);
    }
}