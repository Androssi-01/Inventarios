USE PortafolioDB
GO

--================================================
-- Tabla: Proveedores
--================================================

CREATE TABLE [dbo].[Proveedores](
[ID]			INT IDENTITY(1, 1)	NOT NULL,
[NombreEmpresa] NVARCHAR(150)		NOT NULL,
[Contacto]		NVARCHAR(255)		NULL, 
[Email]			NVARCHAR(150)		NULL,
[Telefono]		NVARCHAR(255)		NULL,
[Activo]		BIT					NOT NULL CONSTRAINT [DF_Proveedores_Activo] DEFAULT (1),
[FechaAlta]		DATETIME2			NOT NULL CONSTRAINT [DF_Proveedores_FechaAlta] DEFAULT (GETDATE()),

CONSTRAINT [PK_Proveedores_Id] PRIMARY KEY ([Id]),
CONSTRAINT [UQ_Proveedores_NombreEmpresa] UNIQUE ([NombreEmpresa])
); 
GO