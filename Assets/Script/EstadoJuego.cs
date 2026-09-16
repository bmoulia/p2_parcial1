// ============================================================
// EstadoJuego.cs
// Enum con las fases del juego.
// El GameManager decide en que fase estamos usando un switch.
// Nota para el video: elegimos enum + switch en vez del patron
// State porque la consigna SOLO puntua Singleton, Strategy, Pool
// y Observer. State no suma, asi que no lo usamos.
// ============================================================
public enum EstadoJuego
{
    Menu,
    Jugando,
    Victoria,
    Derrota
}