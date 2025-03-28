using System;
using Microsoft.Extensions.Configuration;
using DotNetEnv;

public class ConfigManager
{
    private static IConfigurationRoot _configuration;

    static ConfigManager()
    {
        // Carrega variáveis do arquivo .env
        Env.Load();

        // Inicializa a configuração do appsettings.json e das variáveis de ambiente
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static string GetDatabaseConnection() => _configuration["ConnectionStrings:DefaultConnection"];
    public static string GetGeminiApiKey() => _configuration["GeminiApi:ApiKey"];
    public static string GetGoogleClientId() => _configuration["GoogleAuth:ClientId"];
    public static string GetGoogleClientSecret() => _configuration["GoogleAuth:ClientSecret"];
    public static string GetPagSeguroToken() => _configuration["PagSeguro:Token"];
    public static string GetPagSeguroEmail() => _configuration["PagSeguro:Email"];

    // Método para exibir as configurações (somente para debug, sem expor credenciais sensíveis)
    public static void ShowConfig()
    {
        Console.WriteLine($"Banco de Dados: {GetDatabaseConnection()}");
        Console.WriteLine($"Chave do Gemini: {GetGeminiApiKey()}");
    }
}
