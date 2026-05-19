-- =========================================
-- VERIFICACIÓN FINAL DEL SISTEMA
-- =========================================
USE proyecto_derecho_informatico_v3;
GO

PRINT '========================================';
PRINT 'VERIFICACIÓN DEL SISTEMA';
PRINT '========================================';

-- 1. Tablas creadas
SELECT '📊 TABLAS CREADAS' AS verificacion;
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- 2. Llaves foráneas
SELECT '🔗 LLAVES FORÁNEAS' AS verificacion;
SELECT 
    fk.name AS nombre_fk,
    OBJECT_NAME(fk.parent_object_id) AS tabla,
    OBJECT_NAME(fk.referenced_object_id) AS referencia
FROM sys.foreign_keys fk
ORDER BY tabla;

-- 3. Índices
SELECT '📑 ÍNDICES CREADOS' AS verificacion;
SELECT 
    idx.name AS indice,
    OBJECT_NAME(idx.object_id) AS tabla
FROM sys.indexes idx
WHERE idx.name LIKE 'IX_%'
ORDER BY tabla;

-- 4. Conteo de registros
SELECT '📈 REGISTROS POR TABLA' AS verificacion;
SELECT 
    'roles' AS tabla, COUNT(*) AS registros FROM roles
UNION ALL
SELECT 'usuarios', COUNT(*) FROM usuarios
UNION ALL
SELECT 'personas', COUNT(*) FROM personas
UNION ALL
SELECT 'categorias_plantilla', COUNT(*) FROM categorias_plantilla
UNION ALL
SELECT 'plantillas', COUNT(*) FROM plantillas
UNION ALL
SELECT 'plantilla_variables', COUNT(*) FROM plantilla_variables
UNION ALL
SELECT 'estados_documento', COUNT(*) FROM estados_documento
UNION ALL
SELECT 'documentos_juridicos', COUNT(*) FROM documentos_juridicos
UNION ALL
SELECT 'documento_partes', COUNT(*) FROM documento_partes
UNION ALL
SELECT 'documento_variables_valores', COUNT(*) FROM documento_variables_valores
UNION ALL
SELECT 'documento_historial_estados', COUNT(*) FROM documento_historial_estados
UNION ALL
SELECT 'citas', COUNT(*) FROM citas
UNION ALL
SELECT 'auditoria', COUNT(*) FROM auditoria
ORDER BY tabla;

PRINT '========================================';
PRINT '✅ VERIFICACIÓN COMPLETADA';
PRINT '========================================';