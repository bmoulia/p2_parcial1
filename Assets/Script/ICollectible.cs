// ============================================================
// ICollectible.cs
// Contrato de "algo que se puede recolectar".
// Solo lo implementa Collectible (a diferencia de IPoolable,
// que lo cumplen todos los WorldObject).
// Sabe cuanto vale y sabe recolectarse.
// ============================================================
public interface ICollectible
{
    int Valor { get; }   // cuantos puntos otorga al juntarlo
    void Recolectar();   // que pasa cuando lo juntas
}