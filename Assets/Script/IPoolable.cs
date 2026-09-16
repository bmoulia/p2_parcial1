// ============================================================
// IPoolable.cs
// Contrato para todo objeto que el ObjectPool pueda reciclar.
// El pool no necesita saber si es un obstaculo o un coleccionable:
// solo le pide que sepa Activar() y Desactivar().
// ============================================================
public interface IPoolable
{
    void Activar();    // preparar el objeto cuando sale del pool
    void Desactivar(); // apagar el objeto cuando vuelve al pool
}