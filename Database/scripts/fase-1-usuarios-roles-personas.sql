-- =========================================
-- FASE 1: BASE DEL SISTEMA
-- Tablas: roles, usuarios, personas
-- Motor: SQL Server
-- =========================================

-- Verificar si la base de datos existe, si no, crearla
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'gestion_de_documentos_v3')
BEGIN
    CREATE DATABASE gestion_de_documentos_v3;
END
GO

USE gestion_de_documentos_v3;
GO

-- -----------------------------------------
-- 1.1 Tabla: ROLES
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'roles')
BEGIN
    CREATE TABLE roles (
        id_rol INT IDENTITY(1,1) NOT NULL,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(255) NULL,
        activo BIT NOT NULL DEFAULT 1,
        CONSTRAINT PK_roles PRIMARY KEY (id_rol),
        CONSTRAINT UK_roles_nombre UNIQUE (nombre)
    );
    
    PRINT '✅ Tabla roles creada correctamente.';
END
ELSE
BEGIN
    PRINT '⚠️ La tabla roles ya existe.';
END
GO

-- -----------------------------------------
-- 1.2 Tabla: USUARIOS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'usuarios')
BEGIN
CREATE TABLE usuarios (
    id_usuario INT IDENTITY(1,1) NOT NULL,
    id_rol INT NOT NULL,
    nombres NVARCHAR(150) NOT NULL,
    apellidos NVARCHAR(150) NOT NULL,
    nombre_usuario NVARCHAR(100) NOT NULL,
    correo NVARCHAR(150) NOT NULL,
    dpi NVARCHAR(13) NOT NULL,
    colegiado NVARCHAR(20) NULL,
    password_hash NVARCHAR(255) NOT NULL,
    estado NVARCHAR(20) NOT NULL DEFAULT 'ACTIVO',
    fecha_creacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_usuarios PRIMARY KEY (id_usuario),
    CONSTRAINT UK_usuarios_nombre_usuario UNIQUE (nombre_usuario),
    CONSTRAINT UK_usuarios_correo UNIQUE (correo),
    CONSTRAINT UK_usuarios_dpi UNIQUE (dpi),
    CONSTRAINT UK_usuarios_colegiado UNIQUE (colegiado)
);
    
    PRINT '✅ Tabla usuarios creada correctamente.';
END
ELSE
BEGIN
    PRINT '⚠️ La tabla usuarios ya existe.';
END
GO

-- -----------------------------------------
-- 1.3 Tabla: PERSONAS
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'personas')
BEGIN
    CREATE TABLE personas (
        id_persona INT IDENTITY(1,1) NOT NULL,
        tipo_persona NVARCHAR(20) DEFAULT 'INDIVIDUAL',
        nombre_completo NVARCHAR(255) NOT NULL,
        numero_identificacion NVARCHAR(50) NULL,
        nit NVARCHAR(30) NULL,
        telefono NVARCHAR(30) NULL,
        correo NVARCHAR(150) NULL,
        direccion NVARCHAR(500) NULL,
        estado NVARCHAR(20) DEFAULT 'ACTIVO',
        id_usuario INT NULL,
        CONSTRAINT PK_personas PRIMARY KEY (id_persona)
    );
    
    PRINT '✅ Tabla personas creada correctamente.';
END
ELSE
BEGIN
    PRINT '⚠️ La tabla personas ya existe.';
END
GO

-- -----------------------------------------
-- 1.4 Llaves foráneas FASE 1
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_usuarios_roles')
BEGIN
    ALTER TABLE usuarios 
        ADD CONSTRAINT FK_usuarios_roles 
        FOREIGN KEY (id_rol) REFERENCES roles (id_rol);
    
    PRINT '✅ FK usuarios → roles creada.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_personas_usuarios')
BEGIN
    ALTER TABLE personas 
        ADD CONSTRAINT FK_personas_usuarios 
        FOREIGN KEY (id_usuario) REFERENCES usuarios (id_usuario);
    
    PRINT '✅ FK personas → usuarios creada.';
END
GO

-- -----------------------------------------
-- 1.5 Índices FASE 1
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_personas_identificacion')
BEGIN
    CREATE INDEX IX_personas_identificacion ON personas (numero_identificacion);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_personas_nombre')
BEGIN
    CREATE INDEX IX_personas_nombre ON personas (nombre_completo);
END
GO

-- -----------------------------------------
-- 1.6 Datos semilla FASE 1
-- -----------------------------------------
IF NOT EXISTS (SELECT * FROM roles WHERE nombre = 'ADMINISTRADOR')
BEGIN
    INSERT INTO roles (nombre, descripcion) VALUES
    ('ADMINISTRADOR', 'Acceso completo al sistema'),
    ('ABOGADO', 'Crea y gestiona documentos y citas'),
    ('ASISTENTE', 'Acceso limitado a consultas');
    
    PRINT '✅ Roles iniciales insertados.';
END
GO

IF NOT EXISTS (SELECT * FROM usuarios WHERE nombre_usuario = 'admin')
BEGIN
    -- Contraseña: Admin123 (cambiar en producción)
    INSERT INTO usuarios (id_rol, nombres, apellidos, nombre_usuario, correo, password_hash) 
    VALUES (1, 'Administrador', 'Sistema', 'admin', 'admin@sistema.com', 
            '$2b$10$CAMBIA_ESTE_HASH_POR_UNO_REAL');
    
    PRINT '✅ Usuario admin creado.';
END
GO

IF NOT EXISTS (SELECT * FROM personas WHERE numero_identificacion = '0000000000001')
BEGIN
    INSERT INTO personas (tipo_persona, nombre_completo, numero_identificacion, id_usuario) 
    VALUES ('INDIVIDUAL', 'Administrador Sistema', '0000000000001', 1);
    
    PRINT '✅ Persona vinculada al admin creada.';
END
GO

-- Verificación
PRINT '========================================';
PRINT '✅ FASE 1 COMPLETADA';
PRINT '========================================';