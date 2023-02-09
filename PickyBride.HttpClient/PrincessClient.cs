using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using PickyBride.Data.Entities;
using PickyBride.DataContracts;

namespace PickyBride.HttpClient;

public class PrincessClient : IHostedService
{

    private System.Net.Http.HttpClient _httpClient = new();

    private JsonSerializerOptions _options;

    public PrincessClient()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback = 
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };
        _httpClient = new System.Net.Http.HttpClient(handler);
        
        _httpClient.BaseAddress = new Uri("https://localhost:7237/");
        
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<double> GetAverageResult()
    {
        double avg = 0;
        
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine(i);
            avg += await ReproduceAttempt(i);
        }

        avg /= 100;
        Console.WriteLine($"average result = {avg}");
        return avg;
    }
    
    private async Task<int> ReproduceAttempt(int attempt)
    {
        var chosenContender = await ChooseContender(attempt);

        Console.WriteLine($"Chosen contender: {chosenContender}");
        
        int result = GetResult(GetRank(attempt).Result.Rank);
        Console.WriteLine($"result: {result}");
        return result;
    }
    public async Task<ContenderDto> ChooseContender(int attempt)
    {
        Console.WriteLine("choose");
        ContenderDto? best = null;
        int i;
        for (i = 0; i < 100 / 2; i++)
        {
            //Console.WriteLine("choose i");
            var curTask = await GetNextContender(attempt);
            //var cur = curTask;
            Console.WriteLine("got result");
            if (best is null || await IsBestContenderBetterThanCurrent( curTask, best, attempt))
            {
                best = curTask;
            }
            Console.WriteLine("wqrergth");
        }
        ContenderDto? result = null;
        var curGroom = await GetNextContender(attempt);
        Console.WriteLine("got result2");
        Console.WriteLine(curGroom.Name);
        ++i;
        while (await IsBestContenderBetterThanCurrent(best, curGroom, attempt) && i < 100)
        {
            Console.WriteLine("curgroom3");
            curGroom = await GetNextContender(attempt);
            Console.WriteLine("got result3");
            ++i;
        }
        if (await IsBestContenderBetterThanCurrent(curGroom,best, attempt))
        {
            result = curGroom;
        }

        if (result is null)
        {
            return new ContenderDto
            {
                Name = null
            };
        }
        else
        {
            Console.WriteLine(result.Name);
            return new ContenderDto
            {
                Name = result.Name
            };
        }
        return new ContenderDto
        {
            Name = null
        };
    }

    private async Task<ContenderDto?> GetNextContender(int attempt)
    {
        //Console.WriteLine("before response");
        //Console.WriteLine(_httpClient.BaseAddress + $"{attempt}/next");
        var response = await _httpClient.PostAsync($"hall/{attempt}/next", null);
        
        //Console.WriteLine("after response");

        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
            var contender = await JsonSerializer
                .DeserializeAsync<ContenderDto>(stream, _options).ConfigureAwait(false);
            return contender;
        } 
    }

    private async Task<ContenderRankDto> GetRank(int tryId)
    {
        var response = await _httpClient.PostAsync($"hall/{tryId}/select", null).ConfigureAwait(false);;
        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            var rankDto = await JsonSerializer
                .DeserializeAsync<ContenderRankDto>(stream, _options);
            Console.WriteLine("Rating: " + rankDto.Rank);
            return rankDto;
        }
    }

    private async Task Reset()
    {
        await _httpClient.PostAsync("hall/reset", null);
    }

    private int GetResult(int? rating)
    {
        if (rating is null)
            return 10;
        if (rating > 50)
            return rating.Value;
        else
            return 0;

        return 0;
    }

    private async Task<bool> IsBestContenderBetterThanCurrent(ContenderDto contender1, ContenderDto contender2, int attempt)
    {
        //Console.WriteLine("before compare");
        var betterContender = await Compare(contender1, contender2, attempt);
        //Console.WriteLine("after compare");
        return contender1 == betterContender;
    }
    
    private async Task<ContenderDto> Compare(ContenderDto contender1, ContenderDto contender2, int attempt)
    {
        var compareDto = new CompareDto
        {
            Name1 = contender1.Name,
            Name2 = contender2.Name
        };
        //Console.WriteLine(compareDto.Name1 + " " + compareDto.Name2);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        //Console.WriteLine("before string");

        var content = new StringContent(
            JsonSerializer.Serialize(compareDto, options), Encoding.UTF8, "application/json");
        /*Console.WriteLine(await (new StreamContent(content.ReadAsStream())).ReadAsStringAsync());
        Console.WriteLine("before postasync");
        Console.WriteLine(content);*/
        var response = await _httpClient.PostAsync($"friend/{attempt}/compare", content);
        /*Console.WriteLine("after postasync");
        Console.WriteLine(await (new StreamContent(response.Content.ReadAsStream())).ReadAsStringAsync());*/
        /*string text = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();
        Console.WriteLine(text);
        Console.WriteLine("F");
        Console.WriteLine(JsonSerializer.Deserialize<ContenderDto>(response.Content.ReadAsStream()));
        Console.WriteLine("Uf");*/
        using (var stream = response.Content.ReadAsStream())
        {
            //Console.WriteLine("before json");
            var bestContender = await JsonSerializer
                .DeserializeAsync<ContenderDto>(stream, options);
            //Console.WriteLine("after json");
            return bestContender;
        } 

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(GetAverageResult);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}