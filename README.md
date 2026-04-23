# 🍔 Good Hamburger

Sistema completo de gerenciamento de pedidos para lanchonete, desenvolvido com C#/.NET 8.0 seguindo Clean Architecture com CQRS e MediatR.

Inclui **API REST** e **Frontend Blazor WebAssembly** com interface completa para criação de pedidos, visualização de cardápio e cálculo de descontos em tempo real.

## 📋 Funcionalidades

- **API REST** completa com CRUD de pedidos
- **Frontend Blazor** com interface interativa
- Cálculo automático de descontos baseado em combinações de itens
- **Preview de descontos em tempo real** no frontend
- Cardápio com sanduíches, acompanhamentos e bebidas
- Validação de regras de negócio (máximo 1 item de cada tipo por pedido)
- Soft delete de pedidos
- Paginação na listagem de pedidos
- Design responsivo com Bootstrap

## 🏗️ Arquitetura

O projeto segue os princípios de Clean Architecture com separação em 4 camadas + CQRS com MediatR:

```
src/
├── GoodHamburger.Domain       # Entidades, Value Objects, Regras de Negócio
├── GoodHamburger.Application  # CQRS: Commands, Queries, Handlers, DTOs
├── GoodHamburger.Infrastructure # DbContext (MySQL), Repositórios
├── GoodHamburger.API          # API REST (Controllers + MediatR)
├── GoodHamburger.Web          # Frontend Blazor WebAssembly ⭐
└── GoodHamburger.Tests        # Testes Unitários
```

### Padrões Utilizados

- **CQRS**: Separação de Commands (escrita) e Queries (leitura)
- **MediatR**: Implementação do padrão Mediator para dispatch de commands/queries
- **Strategy Pattern**: Cálculo de descontos (`DiscountCalculator`)
- **Repository Pattern**: Abstração de persistência
- **Value Objects**: `Money` para valores monetários
- **Domain Exceptions**: Exceções específicas de negócio
- **Dependency Injection**: Injeção de dependências em toda a aplicação
- **CORS**: Comunicação segura entre frontend e API

## 💰 Regras de Desconto

| Combinação | Desconto |
|------------|----------|
| Sanduíche + Batata + Refrigerante | 20% |
| Sanduíche + Refrigerante | 15% |
| Sanduíche + Batata | 10% |
| Apenas sanduíche | 0% |

**Observação**: Apenas um desconto é aplicado por pedido (o maior que se encaixar).

## 🚀 Como Executar

### Pré-requisitos

- .NET 8.0 SDK
- MySQL Server (localhost ou configurar connection string)

### Configuração do Banco de Dados

Criar o banco de dados MySQL:
```bash
mysql -u root -p
CREATE DATABASE goodhamburger;
```

Ou atualize a connection string em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=goodhamburger;User=root;Password=your_password;"
  }
}
```

### Executar Migrations

Para criar e aplicar as migrations do Entity Framework Core:

```bash
# Navegar até o projeto Infrastructure
cd src/GoodHamburger.Infrastructure

# Criar a migration inicial
dotnet ef migrations add InitialCreate --startup-project ../GoodHamburger.API

# Aplicar a migration ao banco de dados
dotnet ef database update --startup-project ../GoodHamburger.API
```

**Nota**: Se preferir, o banco de dados será criado automaticamente com os dados de seed na primeira execução da API (usando `Database.EnsureCreated()` no `Program.cs`).

### Opção 1: Executar API + Frontend (Recomendado)

Execute a **API** e o **Frontend Blazor** em terminais separados:

**Terminal 1 - API:**
```bash
cd src/GoodHamburger.API
dotnet run
```
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/`

**Terminal 2 - Frontend Blazor:**
```bash
cd src/GoodHamburger.Web
dotnet run
```
- Aplicação Web: `http://localhost:5001`

Acesse `http://localhost:5001` no navegador para usar a interface completa.

### Opção 2: Executar apenas a API

Se preferir usar apenas a API (para testes com Postman/insomnia):

```bash
cd src/GoodHamburger.API
dotnet run
```

A API estará disponível em `http://localhost:7109`

## 🧪 Executar Testes

```bash
cd src/GoodHamburger.Tests
dotnet test
```

## 📚 Endpoints da API

### Menu

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/menu` | Listar cardápio |

### Pedidos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/orders` | Listar pedidos (paginado) |
| GET | `/api/orders/{id}` | Buscar pedido por ID |
| POST | `/api/orders` | Criar novo pedido |
| PUT | `/api/orders/{id}` | Atualizar pedido |
| DELETE | `/api/orders/{id}` | Remover pedido (soft delete) |

### Exemplos de Requisições

#### Criar Pedido

```bash
curl -X POST "http://localhost:7109/api/orders" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "João Silva",
    "menuItemIds": [1, 4, 5]
  }'
```

**Resposta (20% de desconto):**
```json
{
  "id": 1,
  "customerName": "João Silva",
  "status": "Created",
  "items": [
    { "menuItemId": 1, "name": "X Burger", "type": "Sandwich", "unitPrice": 5.00 },
    { "menuItemId": 4, "name": "Batata frita", "type": "Side", "unitPrice": 2.00 },
    { "menuItemId": 5, "name": "Refrigerante", "type": "Drink", "unitPrice": 2.50 }
  ],
  "subtotal": 9.50,
  "discountPercentage": 0.20,
  "discountAmount": 1.90,
  "total": 7.60
}
```

#### Erro - Item Duplicado

```bash
curl -X POST "http://localhost:5000/api/orders" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "João Silva",
    "menuItemIds": [1, 2]
  }'
```

**Resposta (400 Bad Request):**
```json
{
  "statusCode": 400,
  "message": "Only one Sandwich allowed per order",
  "timestamp": "2026-04-22T16:30:00Z"
}
```

## 🌐 Frontend Blazor

O projeto `GoodHamburger.Web` é uma aplicação Blazor WebAssembly que consome a API REST.

### Funcionalidades da Interface

- **🏠 Home**: Dashboard com navegação e promoções
- **📋 Cardápio**: Visualização dos produtos agrupados por categoria
- **🛒 Criar Pedido**: 
  - Seleção múltipla de produtos com checkbox
  - Carrinho com cálculo de descontos em **tempo real**
  - Preview de economia antes de finalizar
- **� Meus Pedidos**: Listagem paginada com cards
- **✅ Confirmação**: Resumo do pedido com valor economizado

### Tecnologias Frontend

- Blazor WebAssembly
- Bootstrap 5
- HttpClient para consumo da API
- Injeção de dependências

## �️ Estrutura do Projeto

### Domain
- `Entities/`: Order, OrderItem, MenuItem
- `ValueObjects/`: Money
- `Enums/`: ItemType, OrderStatus
- `Services/`: DiscountCalculator (Strategy Pattern)
- `Exceptions/`: DomainException, NotFoundException
- `Repositories/`: Interfaces de repositório

### Application
- `Commands/`: Commands para operações de escrita (Create, Update, Delete)
- `Queries/`: Queries para operações de leitura (GetById, GetAll)
- `Handlers/`: Implementações de IRequestHandler para Commands e Queries
- `DTOs/`: Objetos de transferência de dados
- `Mappings/`: Extensões de mapeamento

### Infrastructure
- `Data/`: ApplicationDbContext com EF Core + MySQL
- `Repositories/`: Implementações concretas dos repositórios

### API
- `Controllers/`: OrdersController, MenuController (usando IMediator)
- `Middleware/`: ExceptionMiddleware para tratamento global de erros

## 🎯 Decisões Técnicas

1. **.NET 8.0**: Versão LTS estável e moderna
2. **MySQL**: Banco de dados relacional robusto para produção
3. **CQRS + MediatR**: Separação clara entre leitura e escrita, facilitando manutenção e escalabilidade
4. **Value Object Money**: Evita problemas de precisão com decimal
5. **Strategy Pattern para Descontos**: Facilita adição de novas regras
6. **Soft Delete**: Preserva histórico de pedidos
7. **Paginação**: Previne problemas de performance com grandes volumes
8. **Middleware de Exceções**: Retorna respostas padronizadas para erros

## 📝 Escopo

### Implementado
- ✅ CRUD completo de pedidos (API com CQRS + MediatR)
- ✅ Frontend Blazor WebAssembly com interface interativa
- ✅ Cálculo de descontos automático (API + Frontend)
- ✅ Preview de descontos em tempo real no frontend
- ✅ Validação de itens duplicados
- ✅ Listagem paginada
- ✅ Tratamento de erros global
- ✅ CORS configurado para comunicação API-Web
- ✅ Testes unitários (22 testes)
- ✅ Documentação Swagger
- ✅ Seed de dados do cardápio
- ✅ Banco de dados MySQL
- ✅ .NET 8.0

### Fora de Escopo (para este desafio)
- Autenticação/Autorização
- Cache distribuído
- Mensageria para notificações
- Relatórios e analytics
- API Versioning

## 📄 Licença

Este projeto foi desenvolvido para fins de avaliação técnica.

---

**Desenvolvedor:** Paulo Fabricio Galrão
