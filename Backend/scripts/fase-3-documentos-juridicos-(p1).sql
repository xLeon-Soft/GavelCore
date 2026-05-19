-- =========================================
-- FASE 3: DOCUMENTOS JURÍDICOS (PARTE 1)
-- Tablas: estados_documento, documentos_juridicos
-- Requisito: FASE 1 y 2 completadas
-- Motor: SQL Server
-- =========================================

USE proyecto_derecho_informatico_v3;
GO

-- -----------------------------------------
-- 3.1 Tabla: ESTADOS_DOCUMENTO
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'estados_documento')
BEGIN
    CREATE TABLE estados_documento (
        id_estado_documento INT IDENTITY(1,1) NOT NULL,
        nombre NVARCHAR(30) NOT NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT PK_estados_documento PRIMARY KEY (id_estado_documento),
        CONSTRAINT UK_estados_documento_nombre UNIQUE (nombre)
    );
    
    PRINT '✅ Tabla estados_documento creada.';
END
GO

-- -----------------------------------------
-- 3.2 Tabla: DOCUMENTOS_JURIDICOS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'documentos_juridicos')
BEGIN
    CREATE TABLE documentos_juridicos (
        id_documento INT IDENTITY(1,1) NOT NULL,
        numero_documento NVARCHAR(50) NOT NULL,
        id_plantilla INT NOT NULL,
        id_estado_documento INT NOT NULL,
        titulo NVARCHAR(200) NOT NULL,
        contenido_generado NVARCHAR(MAX) NOT NULL,
        ruta_pdf NVARCHAR(500) NULL,
        creado_por INT NOT NULL,
        fecha_creacion DATETIME DEFAULT GETDATE(),
        fecha_modificacion DATETIME DEFAULT GETDATE(),
        CONSTRAINT PK_documentos_juridicos PRIMARY KEY (id_documento),
        CONSTRAINT UK_documentos_numero UNIQUE (numero_documento)
    );
    
    PRINT '✅ Tabla documentos_juridicos creada.';
END
GO

-- -----------------------------------------
-- 3.3 Llaves foráneas FASE 3
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_documentos_plantillas')
    ALTER TABLE documentos_juridicos ADD CONSTRAINT FK_documentos_plantillas 
        FOREIGN KEY (id_plantilla) REFERENCES plantillas (id_plantilla);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_documentos_estados')
    ALTER TABLE documentos_juridicos ADD CONSTRAINT FK_documentos_estados 
        FOREIGN KEY (id_estado_documento) REFERENCES estados_documento (id_estado_documento);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_documentos_usuarios')
    ALTER TABLE documentos_juridicos ADD CONSTRAINT FK_documentos_usuarios 
        FOREIGN KEY (creado_por) REFERENCES usuarios (id_usuario);
GO

-- -----------------------------------------
-- 3.4 Índices FASE 3
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_documentos_estado')
    CREATE INDEX IX_documentos_estado ON documentos_juridicos (id_estado_documento);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_documentos_creador')
    CREATE INDEX IX_documentos_creador ON documentos_juridicos (creado_por);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_documentos_fecha')
    CREATE INDEX IX_documentos_fecha ON documentos_juridicos (fecha_creacion);
GO

-- -----------------------------------------
-- 3.5 Datos semilla FASE 3
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM estados_documento)
BEGIN
    INSERT INTO estados_documento (nombre) VALUES
    ('BORRADOR'),
    ('APROBADO'),
    ('FINALIZADO'),
    ('ANULADO');
    
    PRINT '✅ Estados de documento insertados.';
END
GO

PRINT '✅ FASE 3 COMPLETADA';
GO