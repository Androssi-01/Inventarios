USE [PortafolioDB];
GO

-- ============================================
-- Clientes
-- ============================================
INSERT INTO [dbo].[Clientes] ([Nombre], [Email], [Telefono], [Direccion])
VALUES 
    ('Ana García',   'ana.garcia@example.com',   '555-1234', 'Av. Reforma 100'),
    ('Carlos Pérez', 'carlos.perez@example.com', NULL,       NULL),
    ('María López',  'maria.lopez@example.com',  '555-5678', NULL);
GO

-- ============================================
-- Productos
-- ============================================
INSERT INTO [dbo].[Productos] ([Sku], [Nombre], [Descripcion], [Precio], [Stock])
VALUES
    ('PROD-001', 'Laptop HP 15',       'Laptop i5 8GB RAM',   12500.00, 5),
    ('PROD-002', 'Mouse Logitech',     NULL,                   350.50,  20),
    ('PROD-003', 'Monitor Samsung 24', 'Monitor 24 pulgadas',  4200.00,  0);
GO

-- ============================================
-- Proveedores
-- ============================================
INSERT INTO [dbo].[Proveedores] ([NombreEmpresa], [Contacto], [Email], [Telefono])
VALUES
    ('Tecnología Global S.A.',   'Juan Ruiz', 'ventas@tecglobal.com', '555-9000'),
    ('Distribuidora del Norte',  NULL,        NULL,                   NULL);
GO

-- ============================================
-- ProductosProveedores (con subconsultas)
-- ============================================
INSERT INTO [dbo].[ProductosProveedores] ([ProductoId], [ProveedorId], [PrecioCompra])
VALUES 
    ((SELECT [Id] FROM [dbo].[Productos]   WHERE [Sku] = 'PROD-001'),
     (SELECT [Id] FROM [dbo].[Proveedores] WHERE [NombreEmpresa] = 'Tecnología Global S.A.'),
     11000.00),
    ((SELECT [Id] FROM [dbo].[Productos]   WHERE [Sku] = 'PROD-002'),
     (SELECT [Id] FROM [dbo].[Proveedores] WHERE [NombreEmpresa] = 'Tecnología Global S.A.'),
     200.00);
GO

-- ============================================
-- Verificación
-- ============================================
SELECT * FROM [dbo].[Clientes];
SELECT * FROM [dbo].[Productos];
SELECT * FROM [dbo].[Proveedores];
SELECT * FROM [dbo].[ProductosProveedores];
GO