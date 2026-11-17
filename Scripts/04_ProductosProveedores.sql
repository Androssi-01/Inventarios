USE PortafolioDB
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

