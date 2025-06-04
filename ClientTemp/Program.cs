using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== Monitoramento de Temperatura ===");

        // Solicita unidade
        string unidade;
        while (true)
        {
            Console.Write("Informe a unidade (celsius, kelvin ou fahrenheit): ");
            unidade = Console.ReadLine()?
            .Trim()/*Remove espaços em branco extras ao fim da string*/
            .ToLower();
            if (unidade == "celsius" || unidade == "kelvin" || unidade == "fahrenheit")
                break;
            Console.WriteLine("Unidade inválida. Tente novamente.");
        }

        // Solicita intervalo
        int intervalo;
        while (true)
        {
            Console.Write("Informe o intervalo entre leituras (em segundos): ");
            if (int.TryParse(Console.ReadLine(), out intervalo)/*Converte pra inteiro se não conseguir retorna um erro*/
             && intervalo > 0)
                break;
            Console.WriteLine("Intervalo inválido. Tente novamente.");
        }

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };/*Mostra uma mensagen de enceramento ao inves de fechar o programa quando ctrlC é presionado*/

        double? ultimaTemp = null;
        var http = new HttpClient();
        string url = $"http://localhost:5135/temperatura/{unidade}";

        Console.WriteLine("\nIniciando monitoramento. Pressione Ctrl+C para encerrar.\n");

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                DateTime agora = DateTime.Now;
                double? tempAtual = null;
                bool erro = false;

                try
                {
                    var resp = await http.GetAsync(url, cts.Token);
                    if (!resp.IsSuccessStatusCode)
                    {
                        erro = true;
                        throw new Exception();
                    }
                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    if (!doc.RootElement.TryGetProperty("valor", out var valorElem))
                        throw new Exception();
                    tempAtual = valorElem.GetDouble();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{agora:HH:mm:ss}] Erro ao obter leitura");
                    Console.ResetColor();
                    await Task.Delay(intervalo * 1000, cts.Token);
                    continue;
                }

                // Exibe resultado
                Console.Write($"[{agora:HH:mm:ss}] Temperatura: {tempAtual:0.00} ");
                Console.Write(unidade == "celsius" ? "°C" : unidade == "kelvin" ? "K" : "°F");
                Console.Write(" → ");

                if (ultimaTemp == null)
                {
                    Console.WriteLine("SEM ALTERAÇÃO");
                }
                else if (tempAtual > ultimaTemp)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("SUBIU");
                    Console.ResetColor();
                }
                else if (tempAtual < ultimaTemp)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("DESCEU");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("SEM ALTERAÇÃO");
                }

                ultimaTemp = tempAtual;
                await Task.Delay(intervalo * 1000, cts.Token);
            }
        }
        catch (TaskCanceledException)
        {
            // Ctrl+C
        }
        finally
        {
            Console.WriteLine("\nEncerrando monitoramento de temperatura.");
        }
    }
}
