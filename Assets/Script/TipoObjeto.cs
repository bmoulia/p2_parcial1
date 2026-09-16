// ============================================================
// TipoObjeto.cs
// Enum con los tipos de objeto que maneja el pool.
// Es la CLAVE del Dictionary: cada tipo tiene su propia cola.
// ============================================================
public enum TipoObjeto
{
    Obstaculo,
    Coleccionable
}