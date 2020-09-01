# Problema: Correios de San Andreas

A solução foi implementada utilizando asp.Net core 3.0, contendo os seguintes projetos:

# 1) Take.CorreioSanAndreas.Services.WebApi
Projeto  web api contendo a controller com as respectivas operações para registro de rotas, consulta de rotas registradas e geração dos melhores caminhos para as encomendas.
A api está documentada com geração automática através do swagger, com a url de acesso já configurada para ser inicializada no launchsettings.

# 2) Take.CorreioSanAndreas.Domain
Projeto to tipo class library .net core, contendo o domínio da aplicação, com as entidades e serviços referentes às regras de negócio (com implementação do algoritmo Dijkstra para encontrar o melhor caminho).

# 3) Take.CorreioSanAndreas.Infra.CrossCutting.IoC
Projeto responsável pelo registro das injeções de dependências. (inversion of control).

# 4) Take.CorreioSanAndreas.Tests,XUnitTest
Projeto que contém os testes unitários.
