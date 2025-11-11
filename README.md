# 🖥️ Frontend  

## 🔄 Ciclos de Vida do Angular Utilizados  
Durante o desenvolvimento, foi utilizado o ciclo de vida **`ngOnInit()`**, presente em diversos componentes.  
Esse hook é executado assim que o componente é inicializado, sendo usado principalmente para:

- Buscar dados iniciais através dos services.  
- Configurar variáveis de estado.  
- Inicializar listas e formulários.  

---

## ⚙️ Uso da Biblioteca RxJS  

A biblioteca **RxJS (Reactive Extensions for JavaScript)** é amplamente utilizada no projeto, principalmente para manipular fluxos assíncronos e trabalhar com os dados retornados pela API.

### 📍 Onde é utilizada  
O RxJS aparece nos services do projeto, como:

- `invoice.service.ts`  
- `product.service.ts`  
- `customer.service.ts`  
- `invoice-item.service.ts`  

Cada método de comunicação com o backend retorna um **Observable**, permitindo que os componentes **assinem** os resultados das requisições.

### 🧩 Como é utilizada  
- Todos os métodos HTTP (`get`, `post`, `put`, `delete`) retornam **Observable<T>**.  
- Nos componentes, o método **`.subscribe()`** é usado para capturar os dados emitidos.  

---

## 📚 Outras Bibliotecas Utilizadas  

Além do Angular e RxJS, o projeto também faz uso de outras bibliotecas que auxiliam na **navegação**, **validação** e **exibição de dados**.

| Biblioteca | Finalidade |
|-------------|------------|
| `@angular/forms` | Criação e validação de formulários. |
| `@angular/router` | Controle de rotas e navegação entre páginas. |
| `@angular/common/http` | Comunicação com o backend via HTTP. |
| `rxjs/operators` | Manipulação de fluxos e tratamento de erros em Observables. |
| `ngx-toastr` | Exibição de notificações (toasts) ao usuário. |
| `bootstrap` | Estilização e layout responsivo. |

---

## 🎨 Bibliotecas de Componentes Visuais  

Para a interface, o projeto utiliza **Bootstrap** e **ngx-toastr** para construir componentes visuais e mensagens interativas.

- **Bootstrap:** fornece estrutura de layout, grid responsivo e componentes prontos (botões, formulários, modais).  
- **ngx-toastr:** exibe notificações amigáveis ao usuário durante ações como salvar, excluir ou atualizar registros.

## 📌 Backend

### 🔹 Frameworks utilizados
- **ASP.NET Core (C#)** como framework principal para construção da API.
- Estrutura baseada em **Controllers** e **Services**.
- Uso de **Dependency Injection (DI)** para gerenciamento de serviços.
- **Entity Framework Core** em conjunto com LINQ para manipulação de dados.

---

### 🔹 Bibliotecas utilizadas
- **AutoMapper** → utilizada para mapear objetos (ex.: DTOs ↔ entidades) de forma automática.
- **Swashbuckle.AspNetCore** → utilizada para gerar documentação interativa da API com Swagger/OpenAPI.

---

### 🔹 Tratamento de erros e exceções
- Implementação de **blocos `try/catch`** em operações críticas.
- Retorno de respostas padronizadas via **`IActionResult`**:
  - `BadRequest()` para erros de validação.
  - `NotFound()` quando recursos não são encontrados.
  - `StatusCode()` para erros inesperados.
- Validações aplicadas antes de operações para evitar exceções desnecessárias.

---

### 🔹 Uso de LINQ
- O projeto faz uso de **LINQ** para manipulação de coleções e consultas:
  - Métodos como `Where()`, `Select()`, `FirstOrDefault()` são utilizados para filtrar e buscar dados.
  - Aplicado principalmente em serviços como **StockService** e **BillingService**.
- LINQ é usado de forma direta sobre coleções em memória, simplificando operações de busca e cálculo.


## ▶️ Como rodar o Backend

### 🔹 Pré-requisitos
Antes de começar, certifique-se de ter instalado:
- .NET 8 SDK
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/) com extensão C#
- Banco de dados (PostgreSQL)
- Git para clonar o repositório

---

### 🔹 Passo a passo

1. **Clonar o repositório**  
   git clone https://github.com/Holiveira090/Korp_Teste_Henrique.git  
   cd Korp_Teste_Henrique/Backend  

2. **Entrar em cada microsserviço e configurar o appsettings.json**  
   Exemplo de conexão:  
   "ConnectionStrings": {  
   "DefaultConnection": "Host=localhost;Port=5432;Database=NomeDoBanco;Username=postgres;Password=SuaSenha"  
   }  

3. **Restaurar dependências em cada microsserviço**  
   dotnet restore  

4. **Compilar o projeto**  
   dotnet build  

5. **Rodar cada microsserviço**  
   dotnet run --launch-profile "https"  

6. **Documentação Swagger**  
   Após rodar, acesse:  
   BillingService → https://localhost:7011/swagger  
   StockService → https://localhost:7020/swagger  

   Lá você encontra todos os endpoints expostos pelo backend.

### 🚀 Como Rodar o Frontend

### ⚙️ Requisitos
- Node.js 18 ou superior  
- Angular CLI instalado globalmente (`npm install -g @angular/cli`)

1. **Acesse a pasta do projeto**  
   cd Frontend/korp-frontend

2. **Instale as dependências**  
   npm install

3. **Inicie o servidor de desenvolvimento**  
   npm start

4. **Acesse no navegador**  
   http://localhost:4200/

### 💡 Observação

Certifique-se de que o **backend (BillingService e StockService)** esteja rodando antes de iniciar o frontend,  
para que a comunicação com a API funcione corretamente.
