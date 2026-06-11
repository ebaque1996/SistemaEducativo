
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/13/2026 09:34:57
-- Generated from EDMX file: C:\Users\EBAQUE1996\source\repos\ProyectoFinalModel\ProyectoFinalModel\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProyectoFinal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Alumno]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Alumno];
GO
IF OBJECT_ID(N'[dbo].[CargaLectiva]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CargaLectiva];
GO
IF OBJECT_ID(N'[dbo].[Curso]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Curso];
GO
IF OBJECT_ID(N'[dbo].[Materia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Materia];
GO
IF OBJECT_ID(N'[dbo].[Matricula]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Matricula];
GO
IF OBJECT_ID(N'[dbo].[Modulo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modulo];
GO
IF OBJECT_ID(N'[dbo].[Nota]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Nota];
GO
IF OBJECT_ID(N'[dbo].[Oferta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Oferta];
GO
IF OBJECT_ID(N'[dbo].[Opcion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Opcion];
GO
IF OBJECT_ID(N'[dbo].[Paralelo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Paralelo];
GO
IF OBJECT_ID(N'[dbo].[PeriodoLectivo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PeriodoLectivo];
GO
IF OBJECT_ID(N'[dbo].[Profesor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Profesor];
GO
IF OBJECT_ID(N'[dbo].[Rol]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rol];
GO
IF OBJECT_ID(N'[dbo].[RolOpcion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolOpcion];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuario];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Alumno'
CREATE TABLE [dbo].[Alumno] (
    [IdAlumno] int  NOT NULL,
    [Cedula] varchar(50)  NOT NULL,
    [Nombres] varchar(300)  NOT NULL,
    [Apellidos] varchar(300)  NOT NULL,
    [FechaNac] datetime  NOT NULL,
    [Sexo] char(1)  NOT NULL,
    [Direccion] varchar(300)  NOT NULL,
    [Telefono] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UltimoNivel] int  NOT NULL,
    [CedulaRepresentante] varchar(50)  NOT NULL,
    [NombresRepresentante] varchar(300)  NOT NULL,
    [ApellidosRepresentante] varchar(300)  NOT NULL,
    [TelefonoRepresentante] varchar(50)  NOT NULL,
    [DireccionRepresentante] varchar(300)  NOT NULL,
    [ColegioAnterior] varchar(150)  NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'CargaLectiva'
CREATE TABLE [dbo].[CargaLectiva] (
    [IdCargaLectiva] int  NOT NULL,
    [IdProfesor] int  NOT NULL,
    [IdOferta] int  NOT NULL,
    [IdPeriodoLectivo] int  NOT NULL,
    [IdMateria] int  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Curso'
CREATE TABLE [dbo].[Curso] (
    [IdCurso] int  NOT NULL,
    [Nivel] int  NOT NULL,
    [Descripcion] varchar(30)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Materia'
CREATE TABLE [dbo].[Materia] (
    [IdMateria] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Matricula'
CREATE TABLE [dbo].[Matricula] (
    [IdMatricula] int  NOT NULL,
    [IdAlumno] int  NOT NULL,
    [IdOferta] int  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [Anulado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Modulo'
CREATE TABLE [dbo].[Modulo] (
    [IdModulo] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Nota'
CREATE TABLE [dbo].[Nota] (
    [IdOferta] int  NOT NULL,
    [IdCargaLectiva] int  NOT NULL,
    [IdAlumno] int  NOT NULL,
    [IdPeriodoLectivo] int  NOT NULL,
    [PpQ1] decimal(18,6)  NOT NULL,
    [SpQ1] decimal(18,6)  NOT NULL,
    [TpQ1] decimal(18,6)  NOT NULL,
    [SumQ1] decimal(18,6)  NOT NULL,
    [PromQ1] decimal(18,6)  NOT NULL,
    [OchenPorQ1] decimal(18,6)  NOT NULL,
    [ExQ1] decimal(18,6)  NOT NULL,
    [VeinQ1] decimal(18,6)  NOT NULL,
    [PromTotQ1] decimal(18,6)  NOT NULL,
    [PpQ2] decimal(18,6)  NOT NULL,
    [SpQ2] decimal(18,6)  NOT NULL,
    [TpQ2] decimal(18,6)  NOT NULL,
    [SumQ2] decimal(18,6)  NOT NULL,
    [PromQ2] decimal(18,6)  NOT NULL,
    [OchenPorQ2] decimal(18,6)  NOT NULL,
    [ExQ2] decimal(18,6)  NOT NULL,
    [VeinQ2] decimal(18,6)  NOT NULL,
    [PromTotQ2] decimal(18,6)  NOT NULL,
    [Total] decimal(18,6)  NOT NULL
);
GO

-- Creating table 'Oferta'
CREATE TABLE [dbo].[Oferta] (
    [IdOferta] int  NOT NULL,
    [IdPeriodoLectivo] int  NOT NULL,
    [IdCurso] int  NOT NULL,
    [IdParalelo] int  NOT NULL,
    [IdProfesor] int  NOT NULL,
    [Jornada] char(3)  NOT NULL,
    [Capacidad] int  NOT NULL,
    [Ocupado] int  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Opcion'
CREATE TABLE [dbo].[Opcion] (
    [IdOpcion] int  NOT NULL,
    [IdModulo] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [Url] varchar(100)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Paralelo'
CREATE TABLE [dbo].[Paralelo] (
    [IdParalelo] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'PeriodoLectivo'
CREATE TABLE [dbo].[PeriodoLectivo] (
    [IdPeriodoLectivo] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [FechaInicio] datetime  NOT NULL,
    [FechaFin] datetime  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Profesor'
CREATE TABLE [dbo].[Profesor] (
    [IdProfesor] int  NOT NULL,
    [Cedula] varchar(13)  NOT NULL,
    [Nombres] varchar(300)  NOT NULL,
    [Apellidos] varchar(300)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Direccion] varchar(300)  NOT NULL,
    [Telefono] varchar(15)  NOT NULL,
    [Sexo] char(1)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'Rol'
CREATE TABLE [dbo].[Rol] (
    [IdRol] int  NOT NULL,
    [Descripcion] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'RolOpcion'
CREATE TABLE [dbo].[RolOpcion] (
    [IdOpcion] int  NOT NULL,
    [IdRol] int  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Usuario'
CREATE TABLE [dbo].[Usuario] (
    [IdUsuario] varchar(50)  NOT NULL,
    [IdRol] int  NOT NULL,
    [Cedula] varchar(13)  NOT NULL,
    [Nombres] varchar(300)  NOT NULL,
    [Apellidos] varchar(300)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Password] varchar(50)  NOT NULL,
    [Estado] char(1)  NOT NULL,
    [EstadoReset] char(1)  NOT NULL,
    [UsuarioCreacion] varchar(50)  NOT NULL,
    [UsuarioActualizacion] varchar(50)  NULL,
    [FechaCreacion] datetime  NOT NULL,
    [FechaActualizacion] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdAlumno] in table 'Alumno'
ALTER TABLE [dbo].[Alumno]
ADD CONSTRAINT [PK_Alumno]
    PRIMARY KEY CLUSTERED ([IdAlumno] ASC);
GO

-- Creating primary key on [IdCargaLectiva] in table 'CargaLectiva'
ALTER TABLE [dbo].[CargaLectiva]
ADD CONSTRAINT [PK_CargaLectiva]
    PRIMARY KEY CLUSTERED ([IdCargaLectiva] ASC);
GO

-- Creating primary key on [IdCurso] in table 'Curso'
ALTER TABLE [dbo].[Curso]
ADD CONSTRAINT [PK_Curso]
    PRIMARY KEY CLUSTERED ([IdCurso] ASC);
GO

-- Creating primary key on [IdMateria] in table 'Materia'
ALTER TABLE [dbo].[Materia]
ADD CONSTRAINT [PK_Materia]
    PRIMARY KEY CLUSTERED ([IdMateria] ASC);
GO

-- Creating primary key on [IdMatricula] in table 'Matricula'
ALTER TABLE [dbo].[Matricula]
ADD CONSTRAINT [PK_Matricula]
    PRIMARY KEY CLUSTERED ([IdMatricula] ASC);
GO

-- Creating primary key on [IdModulo] in table 'Modulo'
ALTER TABLE [dbo].[Modulo]
ADD CONSTRAINT [PK_Modulo]
    PRIMARY KEY CLUSTERED ([IdModulo] ASC);
GO

-- Creating primary key on [IdOferta], [IdCargaLectiva], [IdAlumno], [IdPeriodoLectivo] in table 'Nota'
ALTER TABLE [dbo].[Nota]
ADD CONSTRAINT [PK_Nota]
    PRIMARY KEY CLUSTERED ([IdOferta], [IdCargaLectiva], [IdAlumno], [IdPeriodoLectivo] ASC);
GO

-- Creating primary key on [IdOferta] in table 'Oferta'
ALTER TABLE [dbo].[Oferta]
ADD CONSTRAINT [PK_Oferta]
    PRIMARY KEY CLUSTERED ([IdOferta] ASC);
GO

-- Creating primary key on [IdOpcion] in table 'Opcion'
ALTER TABLE [dbo].[Opcion]
ADD CONSTRAINT [PK_Opcion]
    PRIMARY KEY CLUSTERED ([IdOpcion] ASC);
GO

-- Creating primary key on [IdParalelo] in table 'Paralelo'
ALTER TABLE [dbo].[Paralelo]
ADD CONSTRAINT [PK_Paralelo]
    PRIMARY KEY CLUSTERED ([IdParalelo] ASC);
GO

-- Creating primary key on [IdPeriodoLectivo] in table 'PeriodoLectivo'
ALTER TABLE [dbo].[PeriodoLectivo]
ADD CONSTRAINT [PK_PeriodoLectivo]
    PRIMARY KEY CLUSTERED ([IdPeriodoLectivo] ASC);
GO

-- Creating primary key on [IdProfesor] in table 'Profesor'
ALTER TABLE [dbo].[Profesor]
ADD CONSTRAINT [PK_Profesor]
    PRIMARY KEY CLUSTERED ([IdProfesor] ASC);
GO

-- Creating primary key on [IdRol] in table 'Rol'
ALTER TABLE [dbo].[Rol]
ADD CONSTRAINT [PK_Rol]
    PRIMARY KEY CLUSTERED ([IdRol] ASC);
GO

-- Creating primary key on [IdOpcion], [IdRol] in table 'RolOpcion'
ALTER TABLE [dbo].[RolOpcion]
ADD CONSTRAINT [PK_RolOpcion]
    PRIMARY KEY CLUSTERED ([IdOpcion], [IdRol] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [IdUsuario] in table 'Usuario'
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT [PK_Usuario]
    PRIMARY KEY CLUSTERED ([IdUsuario] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------