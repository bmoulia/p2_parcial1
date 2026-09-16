using UnityEngine;

// ============================================================
// Collectible.cs  (ACTUALIZADO)
// Cambio: al recolectarse, vuelve al pool en vez de solo apagarse.
// ============================================================
public class Collectible : WorldObject, ICollectible
{
    [SerializeField] private int valor = 1;

    public int Valor
    {
        get { return valor; }
    }

    protected override void Reaccionar(bool coincide, Player jugador)
    {
        if (coincide == true)
        {
            Recolectar();
        }
        else
        {
            Debug.Log("No juntaste el coleccionable (polaridad distinta)");
        }
    }

    public void Recolectar()
    {
        GameManager.Instancia.SumarPuntos(valor);
        Debug.Log("Coleccionable juntado! Puntaje total: " + GameManager.Instancia.Puntaje);
        ObjectPool.Instancia.Devolver(this); // vuelve al pool
    }
}