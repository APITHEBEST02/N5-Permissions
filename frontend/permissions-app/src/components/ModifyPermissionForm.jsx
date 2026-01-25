import React, { useState, useEffect } from 'react';
import {
  Box,
  TextField,
  Button,
  Typography,
  Paper,
  Alert,
  MenuItem
} from '@mui/material';
import { permissionsApi } from '../services/api';

function ModifyPermissionForm({ permission, onSuccess, onCancel }) {
  // ESTADOS
  const [formData, setFormData] = useState({
    nombreEmpleado: '',
    apellidoEmpleado: '',
    tipoPermiso: '',
    fechaPermiso: ''
  });
  
  const [message, setMessage] = useState({ type: '', text: '' });
  const [loading, setLoading] = useState(false);
  
  // Tipos de permiso
  const tiposPermiso = [
    { id: 1, descripcion: 'Vacaciones' },
    { id: 2, descripcion: 'Permiso Médico' },
    { id: 3, descripcion: 'Permiso Personal' }
  ];
  
  // CARGAR DATOS DEL PERMISO AL RECIBIR PROP
  useEffect(() => {
    if (permission) {
      setFormData({
        nombreEmpleado: permission.nombreEmpleado,
        apellidoEmpleado: permission.apellidoEmpleado,
        tipoPermiso: permission.tipoPermiso,
        fechaPermiso: permission.fechaPermiso.split('T')[0] 
      });
    }
  }, [permission]); 
  
  // HANDLERS
  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };
  
  const handleSubmit = async (event) => {
    event.preventDefault();
    
    if (!formData.nombreEmpleado || !formData.apellidoEmpleado || !formData.tipoPermiso || !formData.fechaPermiso) {
      setMessage({
        type: 'error',
        text: 'Por favor completa todos los campos'
      });
      return;
    }
    
    try {
      setLoading(true);
      setMessage({ type: '', text: '' });
      
      const dataToSend = {
        id: permission.id,
        nombreEmpleado: formData.nombreEmpleado,
        apellidoEmpleado: formData.apellidoEmpleado,
        tipoPermiso: parseInt(formData.tipoPermiso),
        fechaPermiso: formData.fechaPermiso
      };
      
      await permissionsApi.modify(permission.id, dataToSend);
      
      setMessage({
        type: 'success',
        text: '¡Permiso modificado exitosamente!'
      });
      
      // Notificar al componente padre
      setTimeout(() => {
        onSuccess();
      }, 1500);
      
    } catch (error) {
      console.error('Error:', error);
      setMessage({
        type: 'error',
        text: 'Error al modificar el permiso. Intenta nuevamente.'
      });
    } finally {
      setLoading(false);
    }
  };
  
  // RENDERIZADO
  return (
    <Paper elevation={3} sx={{ p: 4, maxWidth: 600, margin: '0 auto', mt: 4 }}>
      <Typography variant="h5" component="h2" gutterBottom>
        Modificar Permiso #{permission?.id}
      </Typography>
      
      {message.text && (
        <Alert severity={message.type} sx={{ mb: 2 }}>
          {message.text}
        </Alert>
      )}
      
      <Box component="form" onSubmit={handleSubmit} noValidate>
        <TextField
          fullWidth
          label="Nombre del Empleado"
          name="nombreEmpleado"
          value={formData.nombreEmpleado}
          onChange={handleChange}
          margin="normal"
          required
          disabled={loading}
        />
        
        <TextField
          fullWidth
          label="Apellido del Empleado"
          name="apellidoEmpleado"
          value={formData.apellidoEmpleado}
          onChange={handleChange}
          margin="normal"
          required
          disabled={loading}
        />
        
        <TextField
          fullWidth
          select
          label="Tipo de Permiso"
          name="tipoPermiso"
          value={formData.tipoPermiso}
          onChange={handleChange}
          margin="normal"
          required
          disabled={loading}
        >
          {tiposPermiso.map((tipo) => (
            <MenuItem key={tipo.id} value={tipo.id}>
              {tipo.descripcion}
            </MenuItem>
          ))}
        </TextField>
        
        <TextField
          fullWidth
          label="Fecha del Permiso"
          name="fechaPermiso"
          type="date"
          value={formData.fechaPermiso}
          onChange={handleChange}
          margin="normal"
          required
          disabled={loading}
          InputLabelProps={{
            shrink: true,
          }}
        />
        
        <Box sx={{ display: 'flex', gap: 2, mt: 3 }}>
          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            disabled={loading}
          >
            {loading ? 'Guardando...' : 'Guardar Cambios'}
          </Button>
          
          <Button
            variant="outlined"
            color="secondary"
            fullWidth
            onClick={onCancel}
            disabled={loading}
          >
            Cancelar
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}

export default ModifyPermissionForm;