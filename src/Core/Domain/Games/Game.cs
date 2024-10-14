using Domain.Primitives.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Games;

public class Game
{
    [Key]
    public GameId Id { get; private set; } 

    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; private set; } = string.Empty;

    [Required]
    public Money Price { get; private set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Publisher { get; set; } = string.Empty;

}
