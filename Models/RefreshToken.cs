using GranjaLosAres_API.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public bool IsRevoked { get; set; }

    // Cambiar UserId de string a int
    public int UserId { get; set; }

    // Relación con el modelo Usuario
    public virtual Usuario? Usuario { get; set; }
}
