Abaixo está um exemplo de um arquivo `README.md` com documentação completa para o projeto `IaFit` em C#, baseado no código que desenvolvemos até agora. Ele inclui uma visão geral do projeto, instruções de instalação, configuração, uso das rotas, exemplos de requisições e solução de problemas comuns (como o status 500). O README está escrito em português para alinhar com o contexto da nossa conversa.

---

# IaFit - API de Geração de Planos de Nutrição

Bem-vindo ao **IaFit**, uma API desenvolvida em C# (.NET 8) que gera planos de nutrição personalizados utilizando a API Gemini da Google. Este projeto é uma conversão de um código originalmente escrito em TypeScript, adaptado para uma arquitetura em C# com Entity Framework Core, injeção de dependências e controle de limite de requisições.

## Sumário
- [Visão Geral](#visão-geral)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Rotas da API](#rotas-da-api)
- [Exemplos de Requisições](#exemplos-de-requisições)
- [Solução de Problemas](#solução-de-problemas)
- [Contribuição](#contribuição)
- [Licença](#licença)

---

## Visão Geral
O **IaFit** permite que usuários enviem informações pessoais (nome, gênero, idade, altura, peso, objetivo e nível de atividade) para gerar um plano de nutrição personalizado. A API integra-se ao modelo `gemini-1.5-flash` da Google para criar dietas em formato JSON, incluindo refeições e sugestões de suplementos. O projeto também inclui um limitador de requisições para controlar o uso por IP e um endpoint de teste fixo.

---

## Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) (ou outro banco compatível com EF Core)
- [Thunder Client](https://marketplace.visualstudio.com/items?itemName=rangav.vscode-thunder-client) ou [Postman](https://www.postman.com/) para testes
- Chave API do Gemini (obtida no [Google Cloud Console](https://console.cloud.google.com/))

---

## Instalação

1. **Clone o Repositório**:
   ```bash
   git clone https://github.com/seu-usuario/IaFit.git
   cd IaFit
   ```

2. **Restaure as Dependências**:
   ```bash
   dotnet restore IaFit.csproj
   ```

3. **Configure o Banco de Dados**:
   - Abra o arquivo `appsettings.json` e configure a string de conexão:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DietSaaSDb;User Id=sa;Password=SeuPasswordForte;TrustServerCertificate=True;"
     }
     ```
   - Crie o banco de dados e aplique as migrações (se necessário):
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

4. **Configure a Chave API do Gemini**:
   - Adicione a chave ao `appsettings.json`:
     ```json
     "GeminiApi": {
       "ApiKey": "SUA_CHAVE_API_AQUI"
     }
     ```

5. **Compile o Projeto**:
   ```bash
   dotnet build IaFit.csproj
   ```

6. **Execute a API**:
   ```bash
   dotnet run --project IaFit.csproj
   ```
   A API estará disponível em `http://localhost:5000`.

---

## Configuração

### `appsettings.json`
Exemplo completo:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DietSaaSDb;User Id=sa;Password=SeuPasswordForte;TrustServerCertificate=True;"
  },
  "GeminiApi": {
    "ApiKey": "SUA-CHAVE"
  },
  "RateLimit": {
    "MaxRequests": 10,
    "TimeWindowMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "IaFit.Services": "Debug"
    }
  }
}
```

- **`ConnectionStrings`**: String de conexão com o SQL Server.
- **`GeminiApi:ApiKey`**: Chave API do Gemini.
- **`RateLimit`**: Configurações do limitador de requisições (máximo de 10 requisições por IP em 60 minutos).
- **`Logging`**: Níveis de log para depuração.

---

## Estrutura do Projeto

```
IaFit/
├── Controllers/
│   └── NutritionController.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── IApplicationDbContext.cs
├── Entities/
│   ├── Meal.cs
│   ├── NutritionPlan.cs
│   └── UserRequestLog.cs
├── Models/
│   └── NutritionPlanDto.cs
├── Services/
│   ├── INutritionService.cs
│   ├── NutritionService.cs
│   ├── IPaymentService.cs
│   ├── PaymentService.cs
│   └── RequestLimiter.cs
├── appsettings.json
├── Program.cs
└── IaFit.csproj
```

- **Controllers**: Lógica das rotas da API.
- **Data**: Contexto do Entity Framework para o banco de dados.
- **Entities**: Modelos de dados (plano de nutrição, refeições, logs de requisições).
- **Models**: DTOs para entrada de dados.
- **Services**: Serviços de negócio (geração de planos, limite de requisições).

---

## Rotas da API

### 1. **POST /api/nutrition/generate**
Gera um plano de nutrição personalizado usando a API Gemini.

- **Método**: POST
- **URL**: `http://localhost:5000/api/nutrition/generate`
- **Body** (JSON):
  ```json
  {
    "name": "João",
    "gender": "Masculino",
    "age": "30",
    "height": "1.75",
    "weight": "80",
    "objective": "Perda de peso",
    "activityLevel": "Moderado"
  }
  ```
- **Headers**: `Content-Type: application/json`
- **Resposta** (Status 200):
  ```json
  {
    "data": {
      "name": "João",
      "gender": "Masculino",
      "age": 30,
      "height": 1.75,
      "weight": 80,
      "objective": "Perda de peso",
      "activityLevel": "Moderado",
      "meals": [
        {
          "time": "08:00",
          "name": "Café da Manhã",
          "foods": ["2 ovos cozidos", "1 fatia de pão integral"]
        }
      ],
      "supplements": ["Whey Protein"],
      "createdAt": "2025-04-09T12:00:00Z"
    }
  }
  ```

### 2. **GET /api/test**
Retorna um plano de nutrição fixo para teste.

- **Método**: GET
- **URL**: `http://localhost:5000/api/test`
- **Resposta** (Status 200):
  ```json
  {
    "data": {
      "name": "Matheus",
      "gender": "Masculino",
      "age": 28,
      "height": 1.80,
      "weight": 74,
      "objective": "Hipertrofia",
      "activityLevel": "Moderado",
      "meals": [
        {
          "time": "08:00",
          "name": "Café da Manhã",
          "foods": ["2 fatias de pão integral", "2 ovos mexidos", "1 banana", "200ml de leite desnatado"]
        },
        {
          "time": "10:00",
          "name": "Lanche da Manhã",
          "foods": ["1 iogurte grego natural", "1 scoop de whey protein", "1 colher de sopa de granola"]
        }
      ],
      "supplements": ["Whey Protein", "Creatina"],
      "createdAt": "2025-04-09T12:00:00Z"
    }
  }
  ```

---

## Exemplos de Requisições

### Usando Thunder Client
1. **POST /api/nutrition/generate**:
   - Método: POST
   - URL: `http://localhost:5000/api/nutrition/generate`
   - Body: Copie o JSON acima para o campo "Body" no Thunder Client.
   - Headers: Adicione `Content-Type: application/json`.
   - Clique em "Send".

2. **GET /api/test**:
   - Método: GET
   - URL: `http://localhost:5000/api/test`
   - Clique em "Send".

---

## Solução de Problemas

### 1. **Erro ao Compilar: "O arquivo está em uso" (MSB3027/MSB3021)**
- **Sintoma**: `dotnet build` falha com "The process cannot access the file 'IaFit.exe' because it is being used by another process".
- **Solução**:
  1. Feche o terminal ou pressione `Ctrl + C` para parar o `dotnet run`.
  2. Abra o Gerenciador de Tarefas e finalize o processo `IaFit` ou `dotnet`.
  3. Limpe o projeto:
     ```bash
     dotnet clean IaFit.csproj
     dotnet build IaFit.csproj
     ```

### 2. **Status 500 Internal Server Error**
- **Sintoma**: A rota POST retorna status 500.
- **Solução**:
  1. Verifique os logs no console:
     - `info: IaFit.Services.NutritionService[0] Chave API carregada: AIzaS...`
     - `error: IaFit.Services.NutritionService[0] Erro ao chamar a API Gemini...`
  2. Possíveis causas:
     - **Chave API inválida**: Confirme a chave em `appsettings.json` e teste-a manualmente no Thunder Client:
       ```
       POST https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key=SUA_CHAVE
       Body: {"contents":[{"parts":[{"text":"Teste"}]}]}
       ```
     - **Erro de desserialização**: Verifique o log `Resposta bruta da API` e ajuste o modelo `NutritionPlan` se necessário.
     - **Conexão com o banco**: Certifique-se de que o SQL Server está rodando e a string de conexão está correta.

### 3. **Aviso CS8618**
- **Sintoma**: Propriedade não anulável sem valor inicial.
- **Solução**: Tornar a propriedade anulável (ex.: `string? UserId`) ou inicializá-la.

---

## Contribuição
1. Faça um fork do repositório.
2. Crie uma branch para sua feature: `git checkout -b minha-feature`.
3. Commit suas mudanças: `git commit -m "Adiciona minha feature"`.
4. Envie para o repositório remoto: `git push origin minha-feature`.
5. Abra um Pull Request.

---

## Licença
Este projeto é licenciado sob a [MIT License](LICENSE).

---

### Notas Finais
Este README reflete o estado atual do projeto até 09 de abril de 2025, com base no código que desenvolvemos. Se precisar de ajustes adicionais ou mais detalhes, é só me avisar! Teste o projeto com as instruções acima e compartilhe os resultados para resolvermos qualquer problema remanescente, como o status 500. Boa sorte com o **IaFit**!
