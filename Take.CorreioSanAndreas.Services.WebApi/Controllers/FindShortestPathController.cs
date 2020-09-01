using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Take.CorreioSanAndreas.Domain.Interfaces;
using Take.CorreioSanAndreas.Services.WebApi.ExtensionMethods;
using Take.CorreioSanAndreas.Services.WebApi.Utils;

namespace Take.CorreioSanAndreas.Services.WebApi.Controllers
{
    /// <summary>
    /// Esta controller contém as operações de entrada e saída de dados
    /// para cálculo do melhor caminho para entrega das encomendas.
    /// </summary>
    [ApiController]
    [Route("MenorCaminho")]
    public class FindShortestPathController : ControllerBase
    {
        private readonly IShortestPathFinderService _shortestPathFinder;

        /// <summary>
        /// Registra os trechos disponíveis
        /// </summary>
        public FindShortestPathController(IShortestPathFinderService shortestPathFinder)
        {
            _shortestPathFinder = shortestPathFinder;
        }

        /// <summary>
        /// Registra os trechos disponíveis entre as cidades e condados.
        /// </summary>
        /// <param name="pathsFile">Arquivo contendo os trechos</param>
        /// <response code="400">Parametros incorretos.</response>
        /// <response code="500">Erro interno.</response>
        [HttpPost("Trechos")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult RegisterPaths(IFormFile pathsFile)
        {
            PathFinderTxtHandler.RegisterPaths(pathsFile.GetText());
            return NoContent();
        }

        /// <summary>
        /// Consulta os trechos disponíveis registrados entre as cidades e condados.
        /// </summary>
        /// <response code="200">Trechos disponíveis.</response>
        /// <response code="500">Erro interno.</response>
        [HttpGet("Trechos")]
        [ProducesResponseType(typeof(IEnumerable<string>),200)]
        [ProducesResponseType(500)]
        public IActionResult ListPaths()
        {
            return Ok(PathFinderTxtHandler.ListPaths());
        }

        /// <summary>
        /// Registra a origem e destino das encomendas para terem as melhores rotas
        /// calculadas, retornando as informações em um arquivo texto.
        /// </summary>
        /// <param name="ordersFile">Arquivo contendo origem e destino das encomendas</param>
        /// <response code="200">Arquivo contendo as melhores rotas calculadas.</response>
        /// <response code="400">Parametros incorretos.</response>
        /// <response code="500">Erro interno.</response>
        [HttpPost("Encomendas")]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GeneratePathsFile(IFormFile ordersFile)
        {
            var pathsText = PathFinderTxtHandler.GeneratePathsText(ordersFile.GetText(), _shortestPathFinder);
            var fileContent = Encoding.UTF8.GetBytes(pathsText);
            return File(new MemoryStream(fileContent),"text/plain","rotas.txt");
        }
    }
}
