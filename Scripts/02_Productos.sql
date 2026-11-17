Use PortafolioDB
GO

--================================================
-- Tabla: Productos
--================================================

CREATE TABLE [dbo].[Productos](
[Id]			INT				IDENTITY(1, 1) NOT NULL,
[Sku]			NVARCHAR(150)	NOT NULL,
[Nombre]		NVARCHAR(150)	NOT NULL,
[Descripcion]	NVARCHAR(150)	NULL,
[Precio]		DECIMAL(10,2)	NOT NULL, 
[Stock]			INT				NOT NULL CONSTRAINT [DF_Productos_Stock] DEFAULT (0),
[Activo]		BIT				NOT NULL CONSTRAINT [DF_Productos_Activo] DEFAULT (1),
[FechaCreacion]	DATETIME2		NOT NULL CONSTRAINT	[DF_Productos_FechaCreacion] DEFAULT (GETDATE()),

CONSTRAINT [PK_Productos_Id]	PRIMARY KEY ([Id]),
CONSTRAINT [UQ_Productos_Sku]	UNIQUE ([SKU])
);
GO