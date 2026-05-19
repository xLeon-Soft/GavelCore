-- Ver tablas creadas
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Ver registros por tabla
SELECT 'roles' AS tabla, COUNT(*) AS registros FROM roles
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
ORDER BY tabla;