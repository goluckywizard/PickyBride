using Microsoft.Extensions.Hosting;
using PickyBride.Data.Entities;

namespace pickyPride2;

public interface IPrincess : IHostedService
{
    public Contender ChooseContender();
}