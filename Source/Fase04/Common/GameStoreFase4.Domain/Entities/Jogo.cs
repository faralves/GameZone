namespace GameStoreFase4.Domain.Entities;
public class Jogo
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Genero { get; set; }
    public double PrecoUnitario { get; set; }
    public string Console { get; set; }

    public override string ToString()
    {
        var message = $"{{ \"Id\": \'{this.Id}\', \"Nome\": '{this.Nome}', \"Genero\": '{this.Genero}', \"PrecoUnitario\": '{this.PrecoUnitario}', \"Console\": '{this.Console}'  }}";
        return message;
    }
}
