
public interface IPoolable
{
    void Activar();    // preparo el objeto cuando sale del pool
    void Desactivar(); // apago el objeto cuando vuelve al pool
}