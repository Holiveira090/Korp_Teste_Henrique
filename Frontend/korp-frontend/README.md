# Korp Frontend (Angular 17)

## Sobre
Projeto frontend em Angular 17 para integração com os microsserviços do repositório `Korp_Teste_Henrique` (.NET 8).

## Como rodar
1. Certifique-se de que os microsserviços backend estejam rodando (StockService e BillingService) e expostos nas portas usadas no `proxy.conf.json`.
2. No diretório do frontend:
   ```bash
   npm install
   npm start
   ```
3. Acesse http://localhost:4200

## Funcionalidades
- Cadastro de produtos
- Cadastro de notas fiscais com numeração sequencial
- Impressão de notas (atualiza status e saldo)
- Tratamento de falhas entre microsserviços (feedback, retry, compensação)

## Observações
- Ajuste os endpoints em `proxy.conf.json` caso suas portas/backend sejam diferentes.
