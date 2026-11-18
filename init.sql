CREATE DATABASE PortafolioDB;
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


--================================================
-- Tabla: ProductosProveedores
--================================================

CREATE TABLE [dbo].[ProductosProveedores](
[ProductoId]		INT				NOT NULL, 
[ProveedorId]		INT				NOT NULL,
[PrecioCompra]		DECIMAL(10, 2)	NOT NULL,
[FechaAsociacion]	DATETIME2		NOT NULL CONSTRAINT [DF_ProductosProveedores_FechaAsociacion] DEFAULT (GETDATE()),


CONSTRAINT [PK_ProductosProveedores] PRIMARY KEY ([ProductoId], [ProveedorId])

);
GO

--============================================
-- Foreign keys
--============================================

ALTER TABLE [dbo].[ProductosProveedores]

ADD CONSTRAINT [FK_ProductosProveedores_Productos]
	FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
	ON UPDATE NO ACTION
	ON DELETE NO ACTION;
GO

ALTER TABLE [dbo].[ProductosProveedores]

ADD CONSTRAINT [FK_ProductosProveedores_Proveedores]
	FOREIGN KEY ([ProveedorId]) REFERENCES [dbo].[Proveedores] ([Id])
	ON UPDATE NO ACTION
	ON DELETE NO ACTION;
GO

SELECT 
    OBJECT_NAME(parent_object_id) AS Tabla,
    name AS Restriccion,
    type_desc AS Tipo
FROM sys.objects
WHERE type IN ('PK', 'UQ', 'F', 'D')
ORDER BY Tabla, Tipo;
