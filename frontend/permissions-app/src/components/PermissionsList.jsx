import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  CircularProgress,
  Alert,
  IconButton,
  Tooltip
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import { permissionsApi } from '../services/api';

function PermissionsList({ onEdit }) {
  //  ESTADOS 
  const [permissions, setPermissions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  //  CARGAR PERMISOS AL MONTAR EL COMPONENTE 
  useEffect(() => {
    loadPermissions();
  }, []); 
  
  const loadPermissions = async () => {
    try {
      setLoading(true);
      setError('');
      
      const data = await permissionsApi.getAll();
      setPermissions(data);
      
    } catch (err) {
      console.error('Error:', err);
      setError('Error al cargar los permisos');
    } finally {
      setLoading(false);
    }
  };
  
  //  FORMATEAR FECHA 
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES');
  };
  
  //  RENDERIZADO 
  return (
    <Paper elevation={3} sx={{ p: 4, mt: 4 }}>
      <Typography variant="h5" component="h2" gutterBottom>
        Lista de Permisos
      </Typography>
      
     
      {loading && (
        <Box display="flex" justifyContent="center" p={4}>
          <CircularProgress />
        </Box>
      )}
      
      
      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      
      
      {!loading && !error && (
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell><strong>ID</strong></TableCell>
                <TableCell><strong>Nombre</strong></TableCell>
                <TableCell><strong>Apellido</strong></TableCell>
                <TableCell><strong>Tipo</strong></TableCell>
                <TableCell><strong>Fecha</strong></TableCell>
                <TableCell><strong>Acciones</strong></TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {permissions.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    No hay permisos registrados
                  </TableCell>
                </TableRow>
              ) : (
                permissions.map((permission) => (
                  <TableRow key={permission.id}>
                    <TableCell>{permission.id}</TableCell>
                    <TableCell>{permission.nombreEmpleado}</TableCell>
                    <TableCell>{permission.apellidoEmpleado}</TableCell>
                    <TableCell>{permission.tipoPermiso}</TableCell>
                    <TableCell>{formatDate(permission.fechaPermiso)}</TableCell>
                    <TableCell>
                      <Tooltip title="Editar">
                        <IconButton 
                          color="primary"
                          onClick={() => onEdit(permission)}
                        >
                          <EditIcon />
                        </IconButton>
                      </Tooltip>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Paper>
  );
}

export default PermissionsList;