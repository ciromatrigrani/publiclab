using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Text; // Para simular requisições HTTP

public class IOHelper
{
    public static void CreateOrUpdateFile(string path, int lineCount)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (StreamWriter sw = new StreamWriter(path))
        {
            for (int i = 0; i < lineCount; i++)
            {
                sw.WriteLine($"Line {i:000000}: Some sample data to fill the file.");
            }
        }
    }
}

public class AdvancedPerformanceAnalyzer
{
    public static async Task RunAnotherAdvancedAnalysis() // Alterado para async Task
    {
        Console.WriteLine("Iniciando mais uma análise de desempenho avançada...");
        Stopwatch stopwatch = Stopwatch.StartNew();

        string largeFilePath = "another_large_data.txt";
        IOHelper.CreateOrUpdateFile(largeFilePath, 500000); // 500.000 linhas





        // Cenário 1: Leitura de Arquivo Grande com muitas operações de string (síncrono)
        int totalCharsProcessed = 0;
        using (StreamReader reader = new StreamReader(largeFilePath))
        {
            StringBuilder line;
            while ((line = new StringBuilder(await reader.ReadLineAsync())) != null)
            {
                // Operaçães de string que geram novas strings
                string processedLine = line.Replace("data", "text").ToString().ToLowerInvariant();
                totalCharsProcessed += processedLine.Length;
            }
        }
        Console.WriteLine($"Cenário 1 (Leitura de Arquivo Síncrono e String Ops) concluído em: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();






        // Cenário 2: Requisições HTTP Síncronas em Loop
        string[] urls = new string[]
        {
            "http://www.google.com",
            "http://www.bing.com",
            "http://www.yahoo.com"
        };

        HttpClient client = new HttpClient();
        long totalContentLength = 0;
        for (int i = 0; i < 50; i++) // 50 iterações para simular várias requisições
        {
            foreach (string url in urls)
            {
                // Esta é a parte crítica
                HttpResponseMessage response = client.GetAsync(url).Result; // .Result bloqueia
                string content = response.Content.ReadAsStringAsync().Result; // .Result bloqueia
                totalContentLength += content.Length;
            }
        }
        client.Dispose();
        Console.WriteLine($"Cenário 2 (Requisições HTTP Síncronas) concluído em: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();

        // Cenário 3: Alocação de Memória em Loop de Alto Volume
        List<byte[]> buffers = new List<byte[]>();
        for (int i = 0; i < 1000; i++)
        {
            // Aloca um buffer de 1MB em cada iteração
            byte[] buffer = new byte[1024 * 1024];
            // buffers.Add(buffer); // Comentei esta linha para não estourar a memória tão rápido, mas o problema é a alocação
        }
        Console.WriteLine($"Cenário 3 (Alocação de Memória em Loop) concluído em: {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();

        Console.WriteLine("Análise de desempenho avançada concluída.");
    }
}






















