BEGIN TRANSACTION;

-- Precios: DECIMAL(10,2) -> DECIMAL(18,2)
ALTER TABLE dbo.Productos
    ALTER COLUMN Precio DECIMAL(18,2) NOT NULL;

ALTER TABLE dbo.ProductosProveedores
    ALTER COLUMN PrecioCompra DECIMAL(18,2) NOT NULL;

-- Longitudes: 150 -> 255 (igual que los validadores)
ALTER TABLE dbo.Productos
    ALTER COLUMN Sku NVARCHAR(255) NOT NULL;

ALTER TABLE dbo.Productos
    ALTER COLUMN Nombre NVARCHAR(255) NOT NULL;

ALTER TABLE dbo.Productos
    ALTER COLUMN Descripcion NVARCHAR(255) NULL;

COMMIT TRANSACTION;