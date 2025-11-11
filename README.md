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

## 📌 Backend - Documentação Técnica

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

