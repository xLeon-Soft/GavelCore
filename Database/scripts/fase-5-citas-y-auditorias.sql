-- =========================================
-- FASE 5: CITAS Y AUDITORÍA
-- Tablas: citas, auditoria
-- Requisito: FASES 1-4 completadas
-- Motor: SQL Server
-- =========================================

USE proyecto_derecho_informatico_v3;
GO

-- -----------------------------------------
-- 5.1 Tabla: CITAS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'citas')
BEGIN
    CREATE TABLE citas (
        id_cita INT IDENTITY(1,1) NOT NULL,
        id_abogado INT NOT NULL,
        id_cliente INT NULL,
        titulo NVARCHAR(200) NOT NULL,
        descripcion NVARCHAR(MAX) NULL,
        fecha_hora_inicio DATETIME NOT NULL,
        fecha_hora_fin DATETIME NOT NULL,
        estado NVARCHAR(20) NOT NULL DEFAULT 'PENDIENTE',
        notas NVARCHAR(MAX) NULL,
        fecha_creacion DATETIME DEFAULT GETDATE(),
        fecha_modificacion DATETIME DEFAULT GETDATE(),
        CONSTRAINT PK_citas PRIMARY KEY (id_cita)
    );
    
    PRINT '✅ Tabla citas creada.';
END
GO

-- -----------------------------------------
-- 5.2 Tabla: AUDITORIA
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'auditoria')
BEGIN
    CREATE TABLE auditoria (
        id_auditoria INT IDENTITY(1,1) NOT NULL,
        tabla_afectada NVARCHAR(100) NOT NULL,
        accion NVARCHAR(50) NOT NULL,
        valor_anterior NVARCHAR(MAX) NULL,
        valor_nuevo NVARCHAR(MAX) NULL,
        fecha_evento DATETIME DEFAULT GETDATE(),
        id_usuario INT NOT NULL,
        CONSTRAINT PK_auditoria PRIMARY KEY (id_auditoria)
    );
    
    PRINT '✅ Tabla auditoria creada.';
END
GO

-- -----------------------------------------
-- 5.3 Llaves foráneas FASE 5
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_citas_abogado')
    ALTER TABLE citas ADD CONSTRAINT FK_citas_abogado 
        FOREIGN KEY (id_abogado) REFERENCES usuarios (id_usuario);
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_citas_cliente')
    ALTER TABLE citas ADD CONSTRAINT FK_citas_cliente 
        FOREIGN KEY (id_cliente) REFERENCES personas (id_persona)
        ON DELETE SET NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_auditoria_usuarios')
    ALTER TABLE auditoria ADD CONSTRAINT FK_auditoria_usuarios 
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id_usuario);
GO

-- -----------------------------------------
-- 5.4 Índices FASE 5
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_citas_fecha_inicio')
    CREATE INDEX IX_citas_fecha_inicio ON citas (fecha_hora_inicio);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_citas_abogado')
    CREATE INDEX IX_citas_abogado ON citas (id_abogado);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_citas_estado')
    CREATE INDEX IX_citas_estado ON citas (estado);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_auditoria_tabla')
    CREATE INDEX IX_auditoria_tabla ON auditoria (tabla_afectada);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_auditoria_fecha')
    CREATE INDEX IX_auditoria_fecha ON auditoria (fecha_evento);
GO

PRINT '========================================';
PRINT '🎉 FASE 5 COMPLETADA - ¡SISTEMA LISTO!';
PRINT '========================================';