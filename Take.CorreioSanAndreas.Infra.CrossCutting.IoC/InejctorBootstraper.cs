using Microsoft.Extensions.DependencyInjection;
using System;
using Take.CorreioSanAndreas.Domain;
using Take.CorreioSanAndreas.Domain.Interfaces;
using Take.CorreioSanAndreas.Domain.Services;

namespace Take.CorreioSanAndreas.Infra.CrossCutting.IoC
{
    public static class InjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IShortestPathFinderService, DijkstraService>();
        }
    }
}
