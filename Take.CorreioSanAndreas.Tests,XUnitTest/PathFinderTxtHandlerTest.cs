using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Take.CorreioSanAndreas.Domain.Interfaces;
using Take.CorreioSanAndreas.Infra.CrossCutting.IoC;
using Take.CorreioSanAndreas.Services.WebApi.Utils;
using Xunit;

namespace Take.CorreioSanAndreas.Tests_XUnitTest
{
    public class PathFinderTxtHandlerTest
    {
        private readonly IShortestPathFinderService _shortestPathFinder;

        public PathFinderTxtHandlerTest()
        {
            var services = new ServiceCollection();
            InjectorBootStrapper.RegisterServices(services);
            _shortestPathFinder = services.BuildServiceProvider().GetService<IShortestPathFinderService>();
        }

        [Fact]
        [Trait(nameof(PathFinderTxtHandler), "Success")]
        public void GeneratePathsText_Success()
        {
            string paths =
            $"LS SF 1 {Environment.NewLine}"
           +$"SF LS 2 {Environment.NewLine}"
           +$"LS LV 1 {Environment.NewLine}"
           +$"LV LS 1 {Environment.NewLine}"
           +$"SF LV 2 {Environment.NewLine}"
           +$"LV SF 2 {Environment.NewLine}"
           +$"LS RC 1 {Environment.NewLine}"
           +$"RC LS 2 {Environment.NewLine}"
           +$"SF WS 1 {Environment.NewLine}"
           +$"WS SF 2 {Environment.NewLine}"
           +$"LV BC 1 {Environment.NewLine}"
           +$"BC LV 1";

            string orders =
            $"SF WS {Environment.NewLine}"
           +$"LS BC {Environment.NewLine}"
           +$"WS BC";

            PathFinderTxtHandler.RegisterPaths(paths);
            var pathsResult = PathFinderTxtHandler.GeneratePathsText(orders, _shortestPathFinder);

            var pathsLines = pathsResult.Split(Environment.NewLine);

            Assert.True(pathsLines.Length == 3);
            Assert.Equal("SF WS 1", pathsLines[0].Trim());
            Assert.Equal("LS LV BC 2", pathsLines[1].Trim());
            Assert.Equal("WS SF LV BC 5", pathsLines[2].Trim());
        }
    }
}
