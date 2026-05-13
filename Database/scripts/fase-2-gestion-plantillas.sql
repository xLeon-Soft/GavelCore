-- =========================================
-- FASE 2: GESTIÓN DE PLANTILLAS
-- Tablas: categorias_plantilla, plantillas, plantilla_variables
-- Requisito: FASE 1 completada
-- Motor: SQL Server
-- =========================================

USE proyecto_derecho_informatico_v3;
GO

-- -----------------------------------------
-- 2.1 Tabla: CATEGORIAS_PLANTILLA
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'categorias_plantilla')
BEGIN
    CREATE TABLE categorias_plantilla (
        id_categoria INT IDENTITY(1,1) NOT NULL,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(255) NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT PK_categorias_plantilla PRIMARY KEY (id_categoria),
        CONSTRAINT UK_categorias_plantilla_nombre UNIQUE (nombre)
    );
    
    PRINT '✅ Tabla categorias_plantilla creada.';
END
GO

-- -----------------------------------------
-- 2.2 Tabla: PLANTILLAS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'plantillas')
BEGIN
    CREATE TABLE plantillas (
        id_plantilla INT IDENTITY(1,1) NOT NULL,
        id_categoria INT NOT NULL,
        nombre NVARCHAR(150) NOT NULL,
        contenido_base NVARCHAR(MAX) NOT NULL,
        version INT DEFAULT 1,
        creado_por INT NOT NULL,
        CONSTRAINT PK_plantillas PRIMARY KEY (id_plantilla)
    );
    
    PRINT '✅ Tabla plantillas creada.';
END
GO

-- -----------------------------------------
-- 2.3 Tabla: PLANTILLA_VARIABLES
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'plantilla_variables')
BEGIN
    CREATE TABLE plantilla_variables (
        id_plantilla_variable INT IDENTITY(1,1) NOT NULL,
        id_plantilla INT NOT NULL,
        nombre_variable NVARCHAR(100) NOT NULL,
        etiqueta NVARCHAR(150) NOT NULL,
        tipo_dato NVARCHAR(30) NOT NULL DEFAULT 'texto',
        origen_dato NVARCHAR(30) NOT NULL DEFAULT 'formulario',
        CONSTRAINT PK_plantilla_variables PRIMARY KEY (id_plantilla_variable)
    );
    
    PRINT '✅ Tabla plantilla_variables creada.';
END
GO

-- -----------------------------------------
-- 2.4 Llaves foráneas FASE 2
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_plantillas_categorias')
    ALTER TABLE plantillas ADD CONSTRAINT FK_plantillas_categorias 
        FOREIGN KEY (id_categoria) REFERENCES categorias_plantilla (id_categoria);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_plantillas_usuarios')
    ALTER TABLE plantillas ADD CONSTRAINT FK_plantillas_usuarios 
        FOREIGN KEY (creado_por) REFERENCES usuarios (id_usuario);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_plantilla_variables_plantillas')
    ALTER TABLE plantilla_variables ADD CONSTRAINT FK_plantilla_variables_plantillas 
        FOREIGN KEY (id_plantilla) REFERENCES plantillas (id_plantilla)
        ON DELETE CASCADE;
GO

-- -----------------------------------------
-- 2.5 Datos semilla FASE 2
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM categorias_plantilla)
BEGIN
    INSERT INTO categorias_plantilla (nombre, descripcion) VALUES
    ('Contratos', 'Contratos legales: compraventa, arrendamiento, etc.'),
    ('Poderes', 'Cartas de poder y mandatos legales'),
    ('Demandas', 'Documentos de demanda y procesos legales');
END
GO

IF NOT EXISTS (SELECT * FROM plantillas WHERE nombre LIKE 'Contrato de Compraventa%')
BEGIN
    INSERT INTO plantillas (id_categoria, nombre, contenido_base, creado_por) 
    VALUES (1, 'Contrato de Compraventa de Bien Inmueble', 
    'En la ciudad de {ciudad}, el {fecha_letras}, comparecen {vendedor_nombre_completo} como VENDEDOR y {comprador_nombre_completo} como COMPRADOR, quienes celebran CONTRATO DE COMPRAVENTA del inmueble con número de finca {numero_finca}, Folio {folio}, Libro {libro}, por el precio de {precio_letras} (Q{precio_numeros}).',
    1);
    
    DECLARE @id_plantilla INT = SCOPE_IDENTITY();
    
    INSERT INTO plantilla_variables (id_plantilla, nombre_variable, etiqueta, tipo_dato, origen_dato) VALUES
    (@id_plantilla, 'ciudad', 'Ciudad', 'texto', 'formulario'),
    (@id_plantilla, 'fecha_letras', 'Fecha en letras', 'texto', 'formulario'),
    (@id_plantilla, 'vendedor_nombre_completo', 'Nombre del vendedor', 'texto', 'persona'),
    (@id_plantilla, 'comprador_nombre_completo', 'Nombre del comprador', 'texto', 'persona'),
    (@id_plantilla, 'numero_finca', 'Número de finca', 'texto', 'formulario'),
    (@id_plantilla, 'folio', 'Folio', 'texto', 'formulario'),
    (@id_plantilla, 'libro', 'Libro', 'texto', 'formulario'),
    (@id_plantilla, 'precio_letras', 'Precio en letras', 'texto', 'formulario'),
    (@id_plantilla, 'precio_numeros', 'Precio en números', 'numero', 'formulario');
END
GO

PRINT '✅ FASE 2 COMPLETADA';
GO