# Agenda Clínica

Sistema de gerenciamento de agendamentos de consultas para uma clínica fictícia.

O projeto foi desenvolvido como parte de um desafio técnico para demonstrar conhecimentos em:

- ASP.NET Core
- DDD
- CQRS com MediatR
- Dapper
- Autenticação JWT
- Testes automatizados
- Separação em camadas
- Boas práticas de arquitetura

---

## Funcionalidades

O sistema permite:

- Cadastro de pacientes
- Cadastro de profissionais da saúde
- Cadastro e agendamento de consultas
- Visualização da agenda de um profissional
- Cancelamento de consultas
- Login com autenticação JWT
- Documentação da API via Swagger
- Testes automatizados por camada

---
## Login 

Usuário: admin@clinic.com
Senha: 123456

---

## Execução com Docker

O projeto também pode ser executado utilizando Docker e Docker Compose, facilitando a criação do ambiente local com API e banco de dados.

---

## Requisitos para execução com Docker

Antes de executar com Docker, é necessário ter instalado:

- Docker Desktop
- Docker Compose
- WSL habilitado, caso esteja utilizando Windows
- Git

---

## Estrutura esperada para Docker

A solução deve conter arquivos semelhantes a estes na raiz do projeto:

```txt
AgendaClinica
├── docker-compose.yml
├── src
│   ├── AgendaClinica.Api
│   │   └── Dockerfile
│   ├── AgendaClinica.Application
│   ├── AgendaClinica.Domain
│   └── AgendaClinica.Infrastructure
└── tests

```
```txt
docker compose up -d --build
```

## Arquitetura do Projeto

A solução foi organizada em camadas seguindo princípios de DDD e separação de responsabilidades.

```txt
AgendaClinica
├── src
│   ├── AgendaClinica.Api
│   ├── AgendaClinica.Application
│   ├── AgendaClinica.Domain
│   └── AgendaClinica.Infrastructure
│
└── tests
    ├── AgendaClinica.Domain.Tests
    └── AgendaClinica.Application.Tests

```
---

## Documentação da API - Swagger

Após executar a aplicação localmente ou via Docker, a documentação da API estará disponível pelo Swagger.

### Acesso ao Swagger

Se estiver executando via Docker, acesse:

```txt
http://localhost:8080/swagger
```
