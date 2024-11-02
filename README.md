# README - Aplicação Web de Gerenciamento de Endereços

## O que a Aplicação Faz

### Tela de Login
- **Autenticação de Usuário:** Os usuários podem fazer login com suas credenciais.
- **Validação de Credenciais:** Apenas quem tem as informações corretas pode acessar a aplicação.
- **Redirecionamento:** Após o login, os usuários são levados diretamente para a página de endereços.

### CRUD de Endereços
- Os usuários podem adicionar, visualizar, editar e excluir seus endereços.
- Cada endereço inclui: CEP, logradouro, complemento (opcional), bairro, cidade, UF e número.
- Exportação fácil dos endereços salvos para um arquivo CSV.

### Banco de Dados
- **Tabela Usuários:** Armazena informações como Id, nome, usuário e senha.
- **Tabela Endereços:** Contém Id, CEP, logradouro, complemento (opcional), bairro, cidade, UF, número e Id do usuário.

*Os scripts de criação das tabelas estão disponíveis no projeto.*

## Tecnologias Utilizadas
- **Backend:** ASP.NET MVC
- **Banco de Dados:** Entity Framework e SQL Server
- **Frontend:** HTML, CSS e JavaScript (incluindo jQuery)

## Como Começar
Para iniciar a aplicação, é bem simples: basta executá-la! Para configurar o banco de dados, você só precisa rodar o seguinte comando no Gerenciador de Pacotes do Visual Studio:

```bash
EntityFrameworkCore\Update-Database
