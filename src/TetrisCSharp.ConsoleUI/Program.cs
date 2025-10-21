using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TetrisCSharp.Application;
using TetrisCSharp.Domain;
using TetrisCSharp.Infrastructure;

namespace TetrisCSharp.ConsoleUI;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "TetrisCSharp (Fase 0)";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var services = new ServiceCollection();

        // Config
        var cfg = new GameConfig();
        configuration.GetSection("GameConfig").Bind(cfg);
        services.AddSingleton(cfg);

        // Infra / Core
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IRandomizer, SevenBagRandomizer>();
        services.AddSingleton<IRotationSystem, SrsLiteRotationSystem>();
        services.AddSingleton<IScoring, ScoringService>();
        var scorePath = configuration.GetSection("ScoreStore")["Path"] ?? "scores/top10.json";
        services.AddSingleton<IScoreStore>(_ => new JsonScoreStore(scorePath));

        // UI
        services.AddSingleton<IRenderer, RendererASCII>();
        services.AddSingleton<IInputProvider, ConsoleInputProvider>();

        var provider = services.BuildServiceProvider();

        // Fase 0: Mensaje de smoke-run.
        Console.WriteLine("╔═══════════════════════════════════════╗");
        Console.WriteLine("║   TetrisCSharp — Scaffold (Fase 0)   ║");
        Console.WriteLine("╚═══════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("El core está registrado por DI y listo para Fase 1.");
        Console.WriteLine("Presioná cualquier tecla para salir...");
        Console.ReadKey(true);
    }
}
