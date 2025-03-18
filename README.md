# Case de Desenvolvimento - AutoMind

[![.NET](https://img.shields.io/badge/.NET-6.0-%23512bd4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-%23239120)](https://learn.microsoft.com/pt-br/dotnet/csharp/)

Solução para o desafio técnico do processo seletivo da AutoMind, desenvolvendo um sistema de cadastro de usuários com funcionalidades CRUD em console application.

## 📋 Funcionalidades

- **Cadastro de Usuários**
  - Validação de campos obrigatórios
  - Verificação de formato de e-mail válido
  - Validação de faixa etária (1-150 anos)
  - Prevenção de e-mails duplicados
  
- **Listagem de Usuários**
  - Exibição formatada de todos os registros
  - Contagem total de cadastros
  - Ordenação por ID sequencial

- **Busca de Usuários**
  - Pesquisa parcial por nome (case-insensitive)
  - Feedback visual dos resultados
  - Tratamento de termos vazios

- **Melhorias Implementadas**
  - Sistema de cores para feedback (erros, alertas, sucessos)
  - Tratamento robusto de exceções
  - Separação em camadas (apresentação, negócios, dados)
  - Documentação XML completa

## 🛠️ Tecnologias Utilizadas

- **Plataforma**: .NET 6.0
- **Linguagem**: C# 10
- **Pacotes**:
  - `System.Text.RegularExpressions` para validação de e-mails
  - `System.Collections.Generic` para coleções tipadas

## 🚀 Como Executar

### Pré-requisitos
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) ou superior

### Passo a Passo
```bash
# Clone o repositório

# Acesse o diretório

# Restaure as dependências

# Execute o projeto
dotnet run
