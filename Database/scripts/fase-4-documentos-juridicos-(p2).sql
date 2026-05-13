-- =========================================
-- FASE 4: DOCUMENTOS JURÍDICOS (PARTE 2)
-- Tablas: documento_partes, documento_variables_valores, 
--         documento_historial_estados
-- Requisito: FASE 1, 2 y 3 completadas
-- Motor: SQL Server
-- =========================================

USE proyecto_derecho_informatico_v3;
GO

-- -----------------------------------------
-- 4.1 Tabla: DOCUMENTO_PARTES
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'documento_partes')
BEGIN
    CREATE TABLE documento_partes (
        id_documento_parte INT IDENTITY(1,1) NOT NULL,
        id_documento INT NOT NULL,
        id_persona INT NOT NULL,
        rol_en_documento NVARCHAR(50) NOT NULL,
        CONSTRAINT PK_documento_partes PRIMARY KEY (id_documento_parte)
    );
    
    PRINT '✅ Tabla documento_partes creada.';
END
GO

-- -----------------------------------------
-- 4.2 Tabla: DOCUMENTO_VARIABLES_VALORES
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'documento_variables_valores')
BEGIN
    CREATE TABLE documento_variables_valores (
        id_documento_variable INT IDENTITY(1,1) NOT NULL,
        id_documento INT NOT NULL,
        nombre_variable NVARCHAR(100) NOT NULL,
        valor_variable NVARCHAR(MAX) NULL,
        CONSTRAINT PK_documento_variables_valores PRIMARY KEY (id_documento_variable)
    );
    
    PRINT '✅ Tabla documento_variables_valores creada.';
END
GO

-- -----------------------------------------
-- 4.3 Tabla: DOCUMENTO_HISTORIAL_ESTADOS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'documento_historial_estados')
BEGIN
    CREATE TABLE documento_historial_estados (
        id_historial INT IDENTITY(1,1) NOT NULL,
        id_documento INT NOT NULL,
        id_estado_anterior INT NULL,
        id_estado_nuevo INT NOT NULL,
        cambiado_por INT NOT NULL,
        fecha_cambio DATETIME DEFAULT GETDATE(),
        comentario NVARCHAR(500) NULL,
        CONSTRAINT PK_documento_historial_estados PRIMARY KEY (id_historial)
    );
    
    PRINT '✅ Tabla documento_historial_estados creada.';
END
GO

-- -----------------------------------------
-- 4.4 Llaves foráneas FASE 4
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_documento_partes_documentos')
    ALTER TABLE documento_partes ADD CONSTRAINT FK_documento_partes_documentos 
        FOREIGN KEY (id_documento) REFERENCES documentos_juridicos (id_documento)
        ON DELETE CASCADE;
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_documento_partes_personas')
    ALTER TABLE documento_partes ADD CONSTRAINT FK_documento_partes_personas 
        FOREIGN KEY (id_persona) REFERENCES personas (id_persona);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_docvarval_documentos')
    ALTER TABLE documento_variables_valores ADD CONSTRAINT FK_docvarval_documentos 
        FOREIGN KEY (id_documento) REFERENCES documentos_juridicos (id_documento)
        ON DELETE CASCADE;
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_historial_documentos')
    ALTER TABLE documento_historial_estados ADD CONSTRAINT FK_historial_documentos 
        FOREIGN KEY (id_documento) REFERENCES documentos_juridicos (id_documento)
        ON DELETE CASCADE;
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_historial_estado_anterior')
    ALTER TABLE documento_historial_estados ADD CONSTRAINT FK_historial_estado_anterior 
        FOREIGN KEY (id_estado_anterior) REFERENCES estados_documento (id_estado_documento);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_historial_estado_nuevo')
    ALTER TABLE documento_historial_estados ADD CONSTRAINT FK_historial_estado_nuevo 
        FOREIGN KEY (id_estado_nuevo) REFERENCES estados_documento (id_estado_documento);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_historial_usuarios')
    ALTER TABLE documento_historial_estados ADD CONSTRAINT FK_historial_usuarios 
        FOREIGN KEY (cambiado_por) REFERENCES usuarios (id_usuario);
GO

PRINT '✅ FASE 4 COMPLETADA';
GO