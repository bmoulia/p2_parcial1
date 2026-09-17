public interface ICollectible
{
    int Valor { get; }   // cuantos puntos otorga al juntarlo
    void Recolectar();   // que pasa cuando lo juntas
}