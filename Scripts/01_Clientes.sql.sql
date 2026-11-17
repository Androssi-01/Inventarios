Use PortafolioDB
GO
--================================================
-- Tabla: Clientes
--================================================
CREATE TABLE [dbo].[Clientes] (
[Id]			INT				IDENTITY (1, 1) NOT NULL,
[Nombre]		NVARCHAR (150)	NOT NULL,
[Email]			NVARCHAR (150)	NOT NULL,
[Telefono]		NVARCHAR (30), 
[Direccion]		NVARCHAR (255),
[FechaAlta]		DATETIME2		NOT NULL CONSTRAINT [DF_Clientes_FechaAlta] DEFAULT (GETDATE()),
[Activo]		BIT				NOT NULL CONSTRAINT [DF_Clientes_Activo] DEFAULT (1),

CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id]),
CONSTRAINT [UQ_Clientes] UNIQUE ([Email])
);


