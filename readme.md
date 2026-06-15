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
