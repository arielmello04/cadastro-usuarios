using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// Classe que contém o ponto de entrada do programa
public class Program
{
    // Instância do serviço de usuários (camada de negócios)
    private static readonly UserService _userService = new UserService();

    // Método de entrada do programa
    public static void Main(string[] args)
    {
        RunApplication();
    }

    // Controla o fluxo principal da aplicação
    private static void RunApplication()
    {
        bool exit = false;

        // Loop principal do menu
        while (!exit)
        {
            DisplayMainMenu();
            var option = Console.ReadLine();

            // Controle de opções do menu
            switch (option)
            {
                case "1":
                    RegisterUser();
                    break;
                case "2":
                    ListUsers();
                    break;
                case "3":
                    SearchUser();
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("\nSaindo do sistema...");
                    break;
                default:
                    ShowError("Opção inválida! Tente novamente.");
                    break;
            }
            
            // Pausa a execução até que o usuário pressione uma tecla
            if (!exit)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    /// Exibe o menu principal formatado no console
    private static void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== SISTEMA DE CADASTRO DE USUÁRIOS ===");
        Console.WriteLine("1. Cadastrar novo usuário");
        Console.WriteLine("2. Listar todos os usuários");
        Console.WriteLine("3. Buscar usuário por nome");
        Console.WriteLine("4. Sair");
        Console.Write("\nDigite a opção desejada: ");
    }

    /// Método para leitura de strings com validação de campo obrigatório
    private static string ReadString(string prompt, bool required = true)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
            
            // Validação de campo obrigatório
            if (required && string.IsNullOrEmpty(input))
            {
                ShowError("Este campo é obrigatório!");
            }
        } while (required && string.IsNullOrEmpty(input));

        return input;
    }

    /// Valida o formato de e-mail usando expressão regular
    private static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return Regex.IsMatch(email, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.IgnoreCase);
    }

    /// Fluxo de cadastro de novo usuário com validações
    private static void RegisterUser()
    {
        Console.Clear();
        Console.WriteLine("=== NOVO CADASTRO ===");

        // Obtenção dos dados com validação
        var name = ReadString("Nome completo: ");
        
        string email;
        do
        {
            email = ReadString("E-mail: ");
            if (!ValidateEmail(email))
            {
                ShowError("Formato de e-mail inválido!");
            }
        } while (!ValidateEmail(email));

        // Validação de idade entre 1 e 150 anos
        var age = ReadInt("Idade: ", minValue: 1, maxValue: 150);

        try
        {
            var newUser = new User(name, email, age);
            _userService.AddUser(newUser);
            ShowSuccess("Usuário cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            // Captura erros e exibe para o usuário
            ShowError($"Erro ao cadastrar usuário: {ex.Message}");
        }
    }

    /// Método para leitura de números inteiros com validação de intervalo
    private static int ReadInt(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        int value;
        bool isValid;
        do
        {
            var input = ReadString(prompt);
            isValid = int.TryParse(input, out value);

            if (!isValid)
            {
                ShowError("Valor numérico inválido!");
            }
            else if (value < minValue || value > maxValue)
            {
                ShowError($"Valor deve estar entre {minValue} e {maxValue}");
                isValid = false;
            }
        } while (!isValid);

        return value;
    }
    
    /// Lista todos os usuários cadastrados
    private static void ListUsers()
    {
        Console.Clear();
        Console.WriteLine("=== LISTA DE USUÁRIOS ===");

        var users = _userService.GetAllUsers();

        if (!users.Any())
        {
            ShowWarning("Nenhum usuário cadastrado.");
            return;
        }

        // Exibe cabeçalho com contagem
        Console.WriteLine($"\nTotal de usuários: {users.Count}");
        Console.WriteLine(new string('-', 50));
        
        // Lista cada usuário
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }
    
    /// Realiza busca de usuários por parte do nome
    private static void SearchUser()
    {
        Console.Clear();
        Console.WriteLine("=== PESQUISA DE USUÁRIOS ===");

        var searchTerm = ReadString("Digite o nome para pesquisa: ", required: false);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            ShowWarning("Nenhum termo de pesquisa informado!");
            return;
        }

        var results = _userService.SearchUsers(searchTerm);

        Console.WriteLine($"\nResultados para '{searchTerm}':");
        Console.WriteLine(new string('-', 50));

        if (!results.Any())
        {
            ShowWarning("Nenhum usuário encontrado.");
            return;
        }

        foreach (var user in results)
        {
            Console.WriteLine(user);
        }
    }

    // Métodos de exibição de mensagens com cores diferentes
    private static void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// Exibe mensagem de alerta em amarelo
    private static void ShowWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// Exibe mensagem de erro em vermelho
    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

/// Classe de serviço responsável pela lógica de negócios e persistência
public class UserService
{
    private readonly List<User> _users = new List<User>();
    private int _nextId = 1;

    /// Adiciona novo usuário com validação de e-mail único
    public void AddUser(User user)
    {
        // Verifica duplicidade de e-mail
        if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("E-mail já cadastrado!");
        }

        // Atribui ID sequencial e adiciona a lista
        user.Id = _nextId++;
        _users.Add(user);
    }

    /// Retorna cópia da lista de usuários para encapsulamento
    public List<User> GetAllUsers() => new List<User>(_users);

    /// Busca usuários por parte do nome
    public List<User> SearchUsers(string searchTerm)
    {
        return _users
            .Where(u => u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

/// Classe que representa a entidade Usuário
public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }

    public User(string fullName, string email, int age)
    {
        FullName = fullName;
        Email = email;
        Age = age;
    }

    /// Retorna representação formatada do usuário
    public override string ToString()
    {
        return $"ID: {Id} | Nome: {FullName} | E-mail: {Email} | Idade: {Age}";
    }
}