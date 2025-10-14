/*
 ===================================================================================
 SCRIPT COMPLETO DE CRIAÇÃO DO BANCO DE DADOS E ESTRUTURA
 Banco de Dados: GCPIntellectDB
 Servidor: gcpintellectserver001
 ===================================================================================

 SUMÁRIO DA ESTRUTURA DO BANCO DE DADOS

 -- 1. TABELAS AUXILIARES 
    -- Propósito: Servir como "dicionários" para padronizar os dados do sistema.
    -- Entidades: Categoria, Tipo, StatusChamado, Prioridade

 -- 2. TABELAS DE USUÁRIOS E AUTENTICAÇÃO
    -- Propósito: Lidar com quem pode acessar o sistema e quais são suas permissões.
    -- Entidades: Usuario

 -- 3. TABELAS PRINCIPAIS
    -- Propósito: O coração do sistema de tickets.
    -- Entidades: Chamado

 -- 4. TABELAS DE RELACIONAMENTO E SUPORTE AO CHAMADO
    -- Propósito: Adicionar funcionalidades e detalhes aos chamados.
    -- Entidades: Anexo, ChamadoTecnico, ChamadoMensagem

 -- 5. TABELAS DO MÓDULO DE IA
    -- Propósito: Entidades que dão vida à inteligência artificial integrada.
    -- Entidades: BaseConhecimento, PalavraChave, BaseConhecimento_PalavraChave, ConsultaIA

 -- 6. TABELAS DO MÓDULO DE RELATÓRIOS
    -- Propósito: Estrutura profissional para a geração e gerenciamento de relatórios.
    -- Entidades: RelatorioDefinicao, RelatorioExecucao

 -- 7. INSERTS INICIAIS (DADOS BÁSICOS PARA O SISTEMA FUNCIONAR)
    -- Propósito: Popular as tabelas com dados essenciais e criar o primeiro administrador.
*/
-- ===================================================================================

-- Lembrar-se de conectar sua ferramenta de banco de dados DIRETAMENTE ao 'GCPIntellectDB'.

-- ===================================================================================
-- 1. TABELAS AUXILIARES 
-- ===================================================================================

IF OBJECT_ID('dbo.Categoria', 'U') IS NULL
CREATE TABLE Categoria (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL UNIQUE
);
GO

IF OBJECT_ID('dbo.Tipo', 'U') IS NULL
CREATE TABLE Tipo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL UNIQUE
);
GO

IF OBJECT_ID('dbo.StatusChamado', 'U') IS NULL
CREATE TABLE StatusChamado (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(50) NOT NULL UNIQUE
);
GO

IF OBJECT_ID('dbo.Prioridade', 'U') IS NULL
CREATE TABLE Prioridade (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- ===================================================================================
-- 2. TABELAS DE USUÁRIOS E AUTENTICAÇÃO
-- ===================================================================================

IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(150) NOT NULL,
    Login VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(255) NOT NULL UNIQUE,
    SenhaHash VARBINARY(64) NOT NULL,
    TipoAcesso VARCHAR(20) NOT NULL,
    DataCadastro DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    Ativo BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_Usuario_TipoAcesso CHECK (TipoAcesso IN ('Administrador', 'Tecnico', 'Colaborador'))
);
GO

-- ===================================================================================
-- 3. TABELAS PRINCIPAIS 
-- ===================================================================================

IF OBJECT_ID('dbo.Chamado', 'U') IS NULL
CREATE TABLE Chamado (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    IdStatus INT NOT NULL,
    IdCategoria INT NOT NULL,
    IdTipo INT NOT NULL,
    IdPrioridade INT NOT NULL,
    DataAbertura DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    DataConclusao DATETIME2(7) NULL,
    Titulo NVARCHAR(200) NOT NULL,
    Descricao NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_Chamado_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    CONSTRAINT FK_Chamado_Status FOREIGN KEY (IdStatus) REFERENCES StatusChamado(Id),
    CONSTRAINT FK_Chamado_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(Id),
    CONSTRAINT FK_Chamado_Tipo FOREIGN KEY (IdTipo) REFERENCES Tipo(Id),
    CONSTRAINT FK_Chamado_Prioridade FOREIGN KEY (IdPrioridade) REFERENCES Prioridade(Id)
);
GO

-- ===================================================================================
-- 4. TABELAS DE RELACIONAMENTO E SUPORTE AO CHAMADO
-- ===================================================================================

IF OBJECT_ID('dbo.Anexo', 'U') IS NULL
CREATE TABLE Anexo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdChamado INT NOT NULL,
    NomeArquivo NVARCHAR(255) NOT NULL,
    CaminhoArquivo NVARCHAR(500) NOT NULL,
    TipoArquivo VARCHAR(100) NOT NULL,
    TamanhoBytes BIGINT NOT NULL,
    DataUpload DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Anexo_Chamado FOREIGN KEY (IdChamado) REFERENCES Chamado(Id)
);
GO

IF OBJECT_ID('dbo.ChamadoTecnico', 'U') IS NULL
CREATE TABLE ChamadoTecnico (
    IdChamado INT NOT NULL,
    IdTecnico INT NOT NULL,
    PRIMARY KEY (IdChamado, IdTecnico),
    CONSTRAINT FK_ChamadoTecnico_Chamado FOREIGN KEY (IdChamado) REFERENCES Chamado(Id),
    CONSTRAINT FK_ChamadoTecnico_Tecnico FOREIGN KEY (IdTecnico) REFERENCES Usuario(Id)
);
GO

IF OBJECT_ID('dbo.ChamadoMensagem', 'U') IS NULL
CREATE TABLE ChamadoMensagem (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdChamado INT NOT NULL,
    IdUsuarioAutor INT NOT NULL,
    Conteudo NVARCHAR(MAX) NOT NULL,
    DataEnvio DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Mensagem_Chamado FOREIGN KEY (IdChamado) REFERENCES Chamado(Id),
    CONSTRAINT FK_Mensagem_Usuario FOREIGN KEY (IdUsuarioAutor) REFERENCES Usuario(Id)
);
GO

-- ===================================================================================
-- 5. TABELAS DO MÓDULO DE IA
-- ===================================================================================

IF OBJECT_ID('dbo.BaseConhecimento', 'U') IS NULL
CREATE TABLE BaseConhecimento (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo NVARCHAR(300) NOT NULL,
    Resposta NVARCHAR(MAX) NOT NULL,
    IdCategoria INT NULL,
    IdUsuarioCriador INT NOT NULL,
    DataCriacao DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    DataUltimaAtualizacao DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_BaseConhecimento_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(Id),
    CONSTRAINT FK_BaseConhecimento_Usuario FOREIGN KEY (IdUsuarioCriador) REFERENCES Usuario(Id)
);
GO

IF OBJECT_ID('dbo.PalavraChave', 'U') IS NULL
CREATE TABLE PalavraChave (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Texto NVARCHAR(100) NOT NULL UNIQUE
);
GO

IF OBJECT_ID('dbo.BaseConhecimento_PalavraChave', 'U') IS NULL
CREATE TABLE BaseConhecimento_PalavraChave (
    IdBaseConhecimento INT NOT NULL,
    IdPalavraChave INT NOT NULL,
    PRIMARY KEY (IdBaseConhecimento, IdPalavraChave),
    CONSTRAINT FK_BCP_BaseConhecimento FOREIGN KEY (IdBaseConhecimento) REFERENCES BaseConhecimento(Id),
    CONSTRAINT FK_BCP_PalavraChave FOREIGN KEY (IdPalavraChave) REFERENCES PalavraChave(Id)
);
GO

IF OBJECT_ID('dbo.ConsultaIA', 'U') IS NULL
CREATE TABLE ConsultaIA (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdSessao VARCHAR(100) NOT NULL,
    IdUsuario INT NULL,
    PerguntaUsuario NVARCHAR(MAX) NOT NULL,
    RespostaGerada NVARCHAR(MAX) NOT NULL,
    DataConsulta DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    FoiUtil BIT NULL,
    IdBaseConhecimentoSugerido INT NULL,
    ChamadoGeradoId INT NULL,
    CONSTRAINT FK_ConsultaIA_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    CONSTRAINT FK_ConsultaIA_BaseConhecimento FOREIGN KEY (IdBaseConhecimentoSugerido) REFERENCES BaseConhecimento(Id),
    CONSTRAINT FK_ConsultaIA_Chamado FOREIGN KEY (ChamadoGeradoId) REFERENCES Chamado(Id)
);
GO

-- ===================================================================================
-- 6. TABELAS DO MÓDULO DE RELATÓRIOS (OTIMIZADO)
-- ===================================================================================

IF OBJECT_ID('dbo.RelatorioDefinicao', 'U') IS NULL
CREATE TABLE RelatorioDefinicao (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(150) NOT NULL UNIQUE,
    Descricao NVARCHAR(500) NULL,
    IdentificadorLogica VARCHAR(100) NOT NULL UNIQUE,
    PapeisPermitidos NVARCHAR(100) NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID('dbo.RelatorioExecucao', 'U') IS NULL
CREATE TABLE RelatorioExecucao (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdRelatorioDefinicao INT NOT NULL,
    IdUsuarioGerador INT NOT NULL,
    DataGeracao DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    Parametros NVARCHAR(MAX) NULL,
    FormatoSaida VARCHAR(10) NOT NULL,
    CaminhoArquivo NVARCHAR(500) NULL,
    CONSTRAINT FK_Execucao_Definicao FOREIGN KEY (IdRelatorioDefinicao) REFERENCES RelatorioDefinicao(Id),
    CONSTRAINT FK_Execucao_Usuario FOREIGN KEY (IdUsuarioGerador) REFERENCES Usuario(Id)
);
GO

/*
 ===================================================================================
 7. INSERTS INICIAIS (DADOS BÁSICOS PARA O SISTEMA FUNCIONAR)
 ===================================================================================
*/
IF NOT EXISTS (SELECT 1 FROM Tipo WHERE Nome = 'Requisição') INSERT INTO Tipo (Nome) VALUES ('Requisição');
IF NOT EXISTS (SELECT 1 FROM Tipo WHERE Nome = 'Incidente') INSERT INTO Tipo (Nome) VALUES ('Incidente');
GO

IF NOT EXISTS (SELECT 1 FROM StatusChamado WHERE Nome = 'Aberto') INSERT INTO StatusChamado (Nome) VALUES ('Aberto');
IF NOT EXISTS (SELECT 1 FROM StatusChamado WHERE Nome = 'Em Andamento') INSERT INTO StatusChamado (Nome) VALUES ('Em Andamento');
IF NOT EXISTS (SELECT 1 FROM StatusChamado WHERE Nome = 'Resolvido') INSERT INTO StatusChamado (Nome) VALUES ('Resolvido');
IF NOT EXISTS (SELECT 1 FROM StatusChamado WHERE Nome = 'Cancelado') INSERT INTO StatusChamado (Nome) VALUES ('Cancelado');
IF NOT EXISTS (SELECT 1 FROM StatusChamado WHERE Nome = 'Fechado') INSERT INTO StatusChamado (Nome) VALUES ('Fechado');
GO

IF NOT EXISTS (SELECT 1 FROM Prioridade WHERE Nome = 'Baixo') INSERT INTO Prioridade (Nome) VALUES ('Baixo');
IF NOT EXISTS (SELECT 1 FROM Prioridade WHERE Nome = 'Médio') INSERT INTO Prioridade (Nome) VALUES ('Médio');
IF NOT EXISTS (SELECT 1 FROM Prioridade WHERE Nome = 'Alto') INSERT INTO Prioridade (Nome) VALUES ('Alto');
IF NOT EXISTS (SELECT 1 FROM Prioridade WHERE Nome = 'Crítico') INSERT INTO Prioridade (Nome) VALUES ('Crítico');
GO

IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Reset de Senha') INSERT INTO Categoria (Nome) VALUES ('Reset de Senha');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Desbloqueio de Usuário') INSERT INTO Categoria (Nome) VALUES ('Desbloqueio de Usuário');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Criação de Novo Usuário') INSERT INTO Categoria (Nome) VALUES ('Criação de Novo Usuário');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Solicitação de Acesso a Sistema') INSERT INTO Categoria (Nome) VALUES ('Solicitação de Acesso a Sistema');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Solicitação de Acesso a Pasta de Rede') INSERT INTO Categoria (Nome) VALUES ('Solicitação de Acesso a Pasta de Rede');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Problema com Impressora') INSERT INTO Categoria (Nome) VALUES ('Problema com Impressora');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Problema com Mouse ou Teclado') INSERT INTO Categoria (Nome) VALUES ('Problema com Mouse ou Teclado');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Problema com Monitor') INSERT INTO Categoria (Nome) VALUES ('Problema com Monitor');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Solicitação de Celular Corporativo') INSERT INTO Categoria (Nome) VALUES ('Solicitação de Celular Corporativo');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Solicitação de Notebook ou Desktop') INSERT INTO Categoria (Nome) VALUES ('Solicitação de Notebook ou Desktop');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Erro em Sistema Interno (ERP)') INSERT INTO Categoria (Nome) VALUES ('Erro em Sistema Interno (ERP)');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Dúvida sobre Pacote Office (Word, Excel)') INSERT INTO Categoria (Nome) VALUES ('Dúvida sobre Pacote Office (Word, Excel)');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Instalação de Software Aprovado') INSERT INTO Categoria (Nome) VALUES ('Instalação de Software Aprovado');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Atualização de Antivírus') INSERT INTO Categoria (Nome) VALUES ('Atualização de Antivírus');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Internet Lenta ou Indisponível') INSERT INTO Categoria (Nome) VALUES ('Internet Lenta ou Indisponível');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Não consigo conectar na VPN') INSERT INTO Categoria (Nome) VALUES ('Não consigo conectar na VPN');
IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Nome = 'Problema com Wi-Fi') INSERT INTO Categoria (Nome) VALUES ('Problema com Wi-Fi');
GO

IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Login = 'admin')
INSERT INTO Usuario (Nome, Login, Email, SenhaHash, TipoAcesso, Ativo)
VALUES (
    'Administrador do Sistema',
    'admin',
    'admin@seudominio.com',
    0x03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4, -- HASH SHA2_256 da senha 'admin123'
    'Administrador',
    1
);
GO

PRINT 'Banco de dados, tabelas e dados iniciais criados/verificados com sucesso!';