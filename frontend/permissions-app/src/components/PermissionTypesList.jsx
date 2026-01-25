import React, { useState, useEffect } from 'react';
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  Typography,
  Box,
  CircularProgress,
  Alert
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import PermissionTypeForm from './PermissionTypeForm';

const PermissionTypesList = () => {
  const [permissionTypes, setPermissionTypes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [openForm, setOpenForm] = useState(false);

  const fetchPermissionTypes = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:5000/api/permissiontypes');
      
      if (!response.ok) {
        throw new Error('Error al cargar los tipos de permiso');
      }

      const data = await response.json();
      setPermissionTypes(data);
      setError('');
    } catch (err) {
      setError(err.message || 'Error al cargar los tipos de permiso');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPermissionTypes();
  }, []);

  const handleSuccess = () => {
    fetchPermissionTypes();
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h5" component="h2">
          Tipos de Permisos
        </Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => setOpenForm(true)}
        >
          Nuevo Tipo
        </Button>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell><strong>ID</strong></TableCell>
              <TableCell><strong>Descripción</strong></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {permissionTypes.length === 0 ? (
              <TableRow>
                <TableCell colSpan={2} align="center">
                  No hay tipos de permisos registrados
                </TableCell>
              </TableRow>
            ) : (
              permissionTypes.map((type) => (
                <TableRow key={type.id}>
                  <TableCell>{type.id}</TableCell>
                  <TableCell>{type.descripcion}</TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <PermissionTypeForm
        open={openForm}
        onClose={() => setOpenForm(false)}
        onSuccess={handleSuccess}
      />
    </Box>
  );
};

export default PermissionTypesList;
