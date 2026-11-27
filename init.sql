CREATE DATABASE PortafolioDB;
GO
USE PortafolioDB;
GO
--================================================
-- Tabla: Clientes
--================================================
CREATE TABLE [dbo].[Clientes] (
[Id]			INT				IDENTITY (1, 1) NOT NULL,
[Nombre]		NVARCHAR (255)	NOT NULL,
[Email]			NVARCHAR (255)	NOT NULL,
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
[Sku]			NVARCHAR(255)	NOT NULL,
[Nombre]		NVARCHAR(255)	NOT NULL,
[Descripcion]	NVARCHAR(255)	NULL,
[Precio]		DECIMAL(18,2)	NOT NULL, 
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
[Id]			INT IDENTITY(1, 1)	NOT NULL,
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
[PrecioCompra]		DECIMAL(18, 2)	NOT NULL,
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


USE [PortafolioDB];
GO

-- ============================================
-- 1. MÁS CLIENTES (Genera 25 clientes)
-- ============================================
INSERT INTO [dbo].[Clientes] ([Nombre], [Email], [Telefono], [Direccion])
VALUES 
    ('Laura Martínez', 'laura.mtz@example.com', '555-1111', 'Calle Falsa 123'),
    ('Pedro Ramírez', 'pedro.ramirez@example.com', '555-7777', 'Av. Siempre Viva 742'),
    ('Sofía Castro', 'sofia.castro@example.com', NULL, NULL),
    ('Diego Fernández', 'diego.f@example.com', '555-2222', 'Blvd. Central 45'),
    ('Valeria Gómez', 'valeria.gomez@example.com', '555-3333', 'Calle Luna 10'),
    ('Jorge Mendoza', 'jorge.mendoza@example.com', NULL, 'Av. Sol 88'),
    ('Camila Rivas', 'camila.rivas@example.com', '555-4444', NULL),
    ('Andrés Silva', 'andres.silva@example.com', '555-8888', 'Plaza Mayor 12'),
    ('Isabella Torres', 'isa.torres@example.com', NULL, 'Calle Río 5'),
    ('Mateo Navarro', 'mateo.nav@example.com', '555-9999', NULL),
    ('Lucía Domínguez', 'lucia.dom@example.com', '555-1234', 'Av. Las Flores 33'),
    ('Gabriel Vázquez', 'gabriel.vaz@example.com', NULL, NULL),
    ('Daniela Reyes', 'dani.reyes@example.com', '555-5678', 'Calle Pino 90'),
    ('Alejandro Castro', 'alex.castro@example.com', '555-8765', NULL),
    ('Mariana Ortiz', 'mariana.ortiz@example.com', NULL, 'Av. Universidad 100'),
    ('Nicolás Rubio', 'nico.rubio@example.com', '555-4321', NULL),
    ('Elena Mendoza', 'elena.mendoza@example.com', NULL, 'Calle Girasol 15'),
    ('Samuel Guerrero', 'samuel.g@example.com', '555-1357', NULL),
    ('Valentina Peña', 'valen.pena@example.com', '555-2468', 'Av. Las Palmas 40'),
    ('David Rojas', 'david.rojas@example.com', NULL, 'Calle Roble 22');
GO

-- ============================================
-- 2. MÁS PRODUCTOS (Genera 20 productos)
-- ============================================
INSERT INTO [dbo].[Productos] ([Sku], [Nombre], [Descripcion], [Precio], [Stock])
VALUES
    ('PROD-004', 'Teclado Mecánico', 'Teclado RGB switches azules', 850.00, 15),
    ('PROD-005', 'Mouse Gamer', 'Mouse 16000 DPI', 650.00, 30),
    ('PROD-006', 'Audífonos Bluetooth', 'Cancelación de ruido', 1500.00, 10),
    ('PROD-007', 'Silla Ergonómica', 'Silla de oficina con soporte lumbar', 3500.00, 4),
    ('PROD-008', 'Webcam Full HD', 'Cámara 1080p con micrófono', 900.00, 8),
    ('PROD-009', 'Base para Laptop', 'Base refrigerante 5 ventiladores', 450.00, 25),
    ('PROD-010', 'Disco Duro Externo', '1TB USB 3.0', 1200.00, 12),
    ('PROD-011', 'Memoria RAM 16GB', 'DDR4 3200MHz', 950.00, 20),
    ('PROD-012', 'Procesador Ryzen 5', '6 núcleos 12 hilos', 3200.00, 5),
    ('PROD-013', 'Tarjeta Madre B550', 'Soporte Ryzen AM4', 2100.00, 6),
    ('PROD-014', 'Fuente de Poder 750W', '80 Plus Bronze', 1100.00, 9),
    ('PROD-015', 'Gabinete ATX', 'Cristal templado RGB', 1300.00, 7),
    ('PROD-016', 'Monitor Curvo 27"', '144Hz 1ms', 4800.00, 3),
    ('PROD-017', 'Impresora Láser', 'Monocromática WiFi', 2500.00, 2),
    ('PROD-018', 'Tablet 10"', '64GB Android', 3400.00, 11),
    ('PROD-019', 'Smartwatch', 'Monitor de ritmo cardíaco', 1800.00, 14),
    ('PROD-020', 'Cargador Rápido 65W', 'Carga rápida USB-C', 550.00, 40),
    ('PROD-021', 'Hub USB-C', 'Adaptador 7 en 1', 750.00, 18),
    ('PROD-022', 'Micrófono USB', 'Para streaming y grabación', 1400.00, 6),
    ('PROD-023', 'Router WiFi 6', 'Doble banda AX1800', 1600.00, 10);
GO

-- ============================================
-- 3. MÁS PROVEEDORES (Genera 10 proveedores)
-- ============================================
INSERT INTO [dbo].[Proveedores] ([NombreEmpresa], [Contacto], [Email], [Telefono])
VALUES
    ('Insumos Tecnológicos MX', 'Roberto Sánchez', 'contacto@insumostec.com', '555-1122'),
    ('MegaDistribuidora S.A. de C.V.', 'Patricia Luna', 'ventas@megadist.com', '555-3344'),
    ('Hardware Pro', 'Luis Torres', NULL, '555-5566'),
    ('Global Tech Import', 'Carmen Ortiz', 'compras@globaltech.com', '555-7788'),
    ('Soluciones Digitales', 'Fernando Ruiz', NULL, NULL),
    ('Distribuidora del Pacífico', 'Alejandra Gil', 'info@distpacifico.com', '555-9900'),
    ('Suministros Industriales', NULL, NULL, '555-2233'),
    ('Componentes Electrónicos', 'Javier Méndez', 'javier@componentes.com', '555-4455'),
    ('Redes y Comunicaciones', 'Elena Flores', NULL, '555-6677'),
    ('Importadora del Valle', 'Raúl Castro', 'raul@importvalle.com', '555-8899');
GO

-- ============================================
-- 4. ASOCIAR PRODUCTOS CON PROVEEDORES
-- ============================================
-- Usamos subconsultas para buscar los IDs reales y no fallar
INSERT INTO [dbo].[ProductosProveedores] ([ProductoId], [ProveedorId], [PrecioCompra])
VALUES 
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-004'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Hardware Pro'), 600.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-005'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Hardware Pro'), 450.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-006'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Global Tech Import'), 1100.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-007'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'MegaDistribuidora S.A. de C.V.'), 2800.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-010'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Insumos Tecnológicos MX'), 900.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-011'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Componentes Electrónicos'), 750.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-016'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Soluciones Digitales'), 3800.00),
    ((SELECT Id FROM Productos WHERE Sku = 'PROD-020'), (SELECT Id FROM Proveedores WHERE NombreEmpresa = 'Redes y Comunicaciones'), 350.00);
GO