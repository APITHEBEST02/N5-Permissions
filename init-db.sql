USE PermissionsDB;
GO

-- Crear tabla PermissionTypes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PermissionTypes')
BEGIN
    CREATE TABLE PermissionTypes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Descripcion NVARCHAR(200) NOT NULL
    );
END
GO

-- Crear tabla Permissions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permissions')
BEGIN
    CREATE TABLE Permissions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NombreEmpleado NVARCHAR(100) NOT NULL,
        ApellidoEmpleado NVARCHAR(100) NOT NULL,
        TipoPermiso INT NOT NULL,
        FechaPermiso DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Permissions_PermissionTypes FOREIGN KEY (TipoPermiso) 
            REFERENCES PermissionTypes(Id)
    );
END
GO

-- Insertar datos iniciales de PermissionTypes si no existen
IF NOT EXISTS (SELECT * FROM PermissionTypes)
BEGIN
    INSERT INTO PermissionTypes (Descripcion) VALUES 
    ('Vacaciones'),
    ('Permiso por Enfermedad'),
    ('Permiso Personal'),
    ('Permiso de Maternidad/Paternidad'),
    ('Permiso de Estudio');
END
GO

PRINT 'Base de datos inicializada correctamente';
GO
