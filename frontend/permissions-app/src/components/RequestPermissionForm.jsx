import React, { useState, useEffect } from 'react';
import {
  Box,
  TextField,
  Button,
  Typography,
  Paper,
  Alert,
  MenuItem,
  CircularProgress
} from '@mui/material';
import { permissionsApi } from '../services/api';

function RequestPermissionForm() {
  // ====== ESTADOS ======
  const [formData, setFormData] = useState({
    nombreEmpleado: '',
    apellidoEmpleado: '',
    tipoPermiso: '',
    fechaPermiso: ''
  });
  
  // Estado para mostrar mensajes
  const [message, setMessage] = useState({ type: '', text: '' });
  
  // Estado para el loading
  const [loading, setLoading] = useState(false);
  
  // Estado para los tipos de permiso
  const [tiposPermiso, setTiposPermiso] = useState([]);
  const [loadingTypes, setLoadingTypes] = useState(true);
  
  // Cargar tipos de permiso al montar el componente
  useEffect(() => {
    const fetchPermissionTypes = async () => {
      try {
        const response = await fetch('http://localhost:5000/api/permissiontypes');
        if (!response.ok) throw new Error('Error al cargar tipos');
        const data = await response.json();
        setTiposPermiso(data);
      } catch (error) {
        setMessage({
          type: 'error',
          text: 'Error al cargar los tipos de permiso'
        });
      } finally {
        setLoadingTypes(false);
      }
    };
    
    fetchPermissionTypes();
  }, []);
  
  // MANEJAR CAMBIOS EN LOS INPUTS
  const handleChange = (event) => {
    const { name, value } = event.target;
    
    setFormData({
      ...formData,    
      [name]: value  
    });
  };
  
  //  MANEJAR ENVÍO DEL FORMULARIO 
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
      
      // Preparar datos para enviar
      const dataToSend = {
        nombreEmpleado: formData.nombreEmpleado,
        apellidoEmpleado: formData.apellidoEmpleado,
        tipoPermiso: parseInt(formData.tipoPermiso),
        fechaPermiso: formData.fechaPermiso
      };
      
      // Llamar a la API
      const result = await permissionsApi.request(dataToSend);
      
      // Mostrar mensaje de éxito
      setMessage({
        type: 'success',
        text: `¡Permiso solicitado exitosamente! ID: ${result.id}`
      });
      
      // Limpiar formulario
      setFormData({
        nombreEmpleado: '',
        apellidoEmpleado: '',
        tipoPermiso: '',
        fechaPermiso: ''
      });
      
    } catch (error) {
      console.error('Error:', error);
      setMessage({
        type: 'error',
        text: 'Error al solicitar el permiso. Intenta nuevamente.'
      });
    } finally {
      setLoading(false);
    }
  };
  
  // RENDERIZADO (LO QUE SE VE EN PANTALLA)
  return (
    <Paper elevation={3} sx={{ p: 4, maxWidth: 600, margin: '0 auto', mt: 4 }}>
      <Typography variant="h5" component="h2" gutterBottom>
        Solicitar Permiso
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
        
    
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          size="large"
          disabled={loading}
          sx={{ mt: 3 }}
        >
          {loading ? 'Enviando...' : 'Solicitar Permiso'}
        </Button>
      </Box>
    </Paper>
  );
}

export default RequestPermissionForm;