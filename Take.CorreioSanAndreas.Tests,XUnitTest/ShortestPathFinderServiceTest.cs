using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Take.CorreioSanAndreas.Domain;
using Take.CorreioSanAndreas.Domain.Entities;
using Take.CorreioSanAndreas.Domain.Interfaces;
using Take.CorreioSanAndreas.Infra.CrossCutting.IoC;
using Xunit;

namespace Take.CorreioSanAndreas.Tests_XUnitTest
{
    public class ShortestPathFinderTest
    {
        private readonly IShortestPathFinderService _shortestPathFinder;

        private readonly Node _losSantos =      new Node("LS", "Los Santos");
        private readonly Node _sanFierro =      new Node("SF", "San Fierro");
        private readonly Node _lasVenturas =    new Node("LV", "Las Venturas");
        private readonly Node _redCounty =      new Node("RC", "Red County");
        private readonly Node _whetstone =      new Node("WS", "Whetstone");
        private readonly Node _boneCounty =     new Node("BC", "Bone County");

        private void InitializeNodes()
        {
            _losSantos.ConnectTo(_sanFierro, 1);
            _sanFierro.ConnectTo(_losSantos, 2);

            _losSantos.ConnectTo(_lasVenturas, 1);
            _lasVenturas.ConnectTo(_losSantos, 1);

            _sanFierro.ConnectTo(_lasVenturas, 2);
            _lasVenturas.ConnectTo(_sanFierro, 2);

            _losSantos.ConnectTo(_redCounty, 1);
            _redCounty.ConnectTo(_losSantos, 2);

            _sanFierro.ConnectTo(_whetstone, 1);
            _whetstone.ConnectTo(_sanFierro, 2);

            _lasVenturas.ConnectTo(_boneCounty, 1);
            _boneCounty.ConnectTo(_lasVenturas, 1);
        }

        public ShortestPathFinderTest()
        {
            var services = new ServiceCollection();
            InjectorBootStrapper.RegisterServices(services);
            _shortestPathFinder = services.BuildServiceProvider().GetService<IShortestPathFinderService>();
        }

        [Fact]
        [Trait(nameof(IShortestPathFinderService.FindShortestPath), "Success")]
        public void ShortestPathSanFierroToWhetstone_Success()
        {
            InitializeNodes();

            var path = _shortestPathFinder.FindShortestPath(_sanFierro, _whetstone);

            Assert.True(path.Length == 2);
            Assert.Equal("SF", path[0].Id);
            Assert.Equal("WS", path[1].Id);
            Assert.Equal(1, path[0].Edges.First(x => x.Node2.Id == "WS").Value);
        }

        [Fact]
        [Trait(nameof(IShortestPathFinderService.FindShortestPath), "Success")]
        public void ShortestPathLosSantosToBoneCounty_Success()
        {
            InitializeNodes();

            var path = _shortestPathFinder.FindShortestPath(_losSantos, _boneCounty);

            Assert.True(path.Length == 3);
            Assert.Equal("LS", path[0].Id);
            Assert.Equal("LV", path[1].Id);
            Assert.Equal("BC", path[2].Id);
            Assert.Equal(1, path[0].Edges.First(x => x.Node2.Id == "LV").Value);
            Assert.Equal(1, path[1].Edges.First(x => x.Node2.Id == "BC").Value);
        }

        [Fact]
        [Trait(nameof(IShortestPathFinderService.FindShortestPath), "Success")]
        public void ShortestPathWhetstoneToBoneCounty_Success()
        {
            InitializeNodes();

            var path = _shortestPathFinder.FindShortestPath(_whetstone, _boneCounty);

            Assert.True(path.Length == 4);
            Assert.Equal("WS", path[0].Id);
            Assert.Equal("SF", path[1].Id);
            Assert.Equal("LV", path[2].Id);
            Assert.Equal("BC", path[3].Id);

            Assert.Equal(2, path[0].Edges.First(x => x.Node2.Id == path[1].Id).Value);
            Assert.Equal(2, path[1].Edges.First(x => x.Node2.Id == path[2].Id).Value);
            Assert.Equal(1, path[2].Edges.First(x => x.Node2.Id == path[3].Id).Value);
        }
    }
}
