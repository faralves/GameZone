using GameStoreFase4.Domain.Entities;

namespace GameStoreFase4.Services.Generator;
public class GeneratorDataService : IGeneratorDataService
{
    public Jogo Generate()
    {
        var jogo = new Jogo()
        {
            Nome = GerarNome(),
            Genero = GerarTipoGenero(),
            PrecoUnitario = GerarPrecoUnitario(59, 500),
            Console = GerarNomeConsole()
        };

        return jogo;
    }
    private string GerarNome()
    {
        List<string> prefix = new List<string>()
        {
            "Super", "Mega", "Max", "Ultra", "Hiper", "War", "Island"
        };

        List<string> sufix = new List<string>();
        for (int i = 1; i <= 2024; i++)
        {
            sufix.Add(i.ToString());
        }

        List<string> names = new List<string>()
        {
            "Mario", "Luigi", "Man", "Yoshi", "Peach", "Raiden", "Slash", "Gorko",
            "He-Man", "Tartarugas Ninja", "Tekken", "Fatal Fury", "King of Fighters", "Street Fighter", "Mortal Kombat", "Combate", "Fifa", "UFC", "Ferrari", "Lamborguini",
            "Speed Racer", "Hot Wheels", "Chapolin", "Chaves", "Bob Esponja", "Spiderman", "Thor", "Ironman", "Pantera Negra", "Black Panter", "Loki", "Feiticeira Escarlate",
            "Formula", "Planta vs Zumbies", "NBA", "Kart", "Ben 10", "Sonic", "Knuckles", "Vectorman", "Shinobi", "Gavião Arqueiro", "Saint Seya", "Dragon Ball", "Cavaleiros do Zodiaco",
            "Yuyu Hakusho", "Samurai", "Samurai Warriors", "Shurato", "PES", "Winning Eleven", "Gran Turismo", "Cronicas", "Harry Poter", "Senhor dos Aneis", "Zelda", "Link",
            "Turma da Monica", "Pit Fighter", "Rock'n Roll", "Jiaraya", "Jaspion", "Flashman", "Changeman", "Naruto", "My Hero Academia"
        };

        var random = new Random();

        bool enablePrefix = random.Next(0, 10) > 4 ? false : true;
        bool enableSufix = random.Next(0, 10) > 4 ? false : true;

        string prefixname = "";
        string name = "";
        string sufixname = "";

        name = names[random.Next(0, names.Count - 1)];

        if (enablePrefix)
        {
            prefixname = prefix[random.Next(0, prefix.Count - 1)];
            name = prefixname + " " + name;
        }

        if (enableSufix)
        {
            sufixname = sufix[random.Next(0, sufix.Count - 1)];
            name = name + " " + sufixname;
        }

        return name;
    }
    private string GerarTipoGenero()
    {
        List<string> genres = new List<string>()
        {
            "Aventura", "Esporte", "Luta", "Fantasia", "Terror", "Guerra", "Corrida", "Estrategia"
        };

        var random = new Random();
        string genre = genres[random.Next(0, genres.Count - 1)];
        return genre;
    }
    private double GerarPrecoUnitario(int minValue, int maxValue)
    {
        var random = new Random();
        double unitPrice = random.Next(minValue, maxValue);
        return unitPrice;
    }
    private string GerarNomeConsole()
    {
        List<string> consoles = new List<string>()
        {
            "PS4", "PS5", "Xbox S", "Xbox One", "Xbox 360", "PS3", "Nintendo Switch", "Nintendo Wii U",
            "Nintendo Wii", "PS2", "SNes", "GameCube", "MegaDrive", "NeoGeo", "Nintendo 8Bits"
        };

        var random = new Random();
        string console = consoles[random.Next(0, consoles.Count - 1)];
        return console;
    }

}
