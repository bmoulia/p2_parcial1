using UnityEngine;

// ============================================================
// Projectile.cs
// Hija de Obstacle (Obstacle -> WorldObject). Es el disparo enemigo.
// Comparte con el obstaculo la regla de polaridad: si tu polaridad
// coincide lo atravesas, si no, morís. Lo UNICO que cambia es como
// se mueve -> por eso pisa Mover().
// Clave del pedido: la direccion se calcula UNA vez al nacer (apunta
// al jugador en ese instante) y despues vuela recta. No persigue.
// Concepto del parcial: HERENCIA MULTINIVEL (WorldObject -> Obstacle
// -> Projectile) = segundo caso de herencia. Y POLIMORFISMO en Mover().
// ============================================================
public class Projectile : Obstacle
{
    [SerializeField] private float velocidadDisparo = 18f;

    // La direccion fija en la que viaja. Se setea al dispararse.
    private Vector3 direccion;

    // La llama la nave enemiga al momento de disparar.
    // Recibe hacia donde apuntar (la posicion del jugador en ese instante).
    public void Disparar(Vector3 posicionObjetivo)
    {
        // Vector desde el disparo hacia el jugador, normalizado (largo 1).
        direccion = posicionObjetivo - transform.position;
        direccion = direccion.normalized;
    }

    // Pisamos el movimiento de la base: en vez de venir recto por Z,
    // el proyectil vuela en su direccion fija calculada al nacer.
    protected override void Mover()
    {
        transform.Translate(direccion * velocidadDisparo * Time.deltaTime, Space.World);
    }
}