import React, { useState } from 'react';
import {
  Container,
  AppBar,
  Toolbar,
  Typography,
  Box,
  Tabs,
  Tab,
  ThemeProvider,
  createTheme,
  CssBaseline
} from '@mui/material';
import RequestPermissionForm from './components/RequestPermissionForm';
import PermissionsList from './components/PermissionsList';
import ModifyPermissionForm from './components/ModifyPermissionForm';
import PermissionTypesList from './components/PermissionTypesList';

//  CREAR TEMA PERSONALIZADO
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2', // Azul
    },
    secondary: {
      main: '#dc004e', // Rojo
    },
  },
});

function App() {
  //  ESTADOS
  const [activeTab, setActiveTab] = useState(0); // 0 = Solicitar, 1 = Listar
  const [selectedPermission, setSelectedPermission] = useState(null);
  const [refreshList, setRefreshList] = useState(0);
  
  //  HANDLERS 
  const handleTabChange = (event, newValue) => {
    setActiveTab(newValue);
    setSelectedPermission(null); 
  };
  
  const handleEdit = (permission) => {
    setSelectedPermission(permission);
    setActiveTab(3); // Tab de modificar ahora es el índice 3
  };
  
  const handleModifySuccess = () => {
    setSelectedPermission(null);
    setActiveTab(1); // Volver a lista
    setRefreshList(prev => prev + 1); // Trigger refresh
  };
  
  const handleModifyCancel = () => {
    setSelectedPermission(null);
    setActiveTab(1); // Volver a lista
  };
  
  //  RENDERIZADO
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      
      
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div">
            Sistema de Gestión de Permisos - N5
          </Typography>
        </Toolbar>
      </AppBar>
      
      
      <Container maxWidth="lg">
        <Box sx={{ mt: 4 }}>
          
          <Tabs 
            value={activeTab} 
            onChange={handleTabChange}
            centered
          >
            <Tab label="Solicitar Permiso" />
            <Tab label="Lista de Permisos" />
            <Tab label="Tipos de Permisos" />
            {selectedPermission && <Tab label="Modificar Permiso" />}
          </Tabs>
          
          
          <Box sx={{ mt: 3 }}>
            {activeTab === 0 && <RequestPermissionForm />}
            
            {activeTab === 1 && (
              <PermissionsList 
                key={refreshList} 
                onEdit={handleEdit} 
              />
            )}
            
            {activeTab === 2 && <PermissionTypesList />}
            
            {activeTab === 3 && selectedPermission && (
              <ModifyPermissionForm
                permission={selectedPermission}
                onSuccess={handleModifySuccess}
                onCancel={handleModifyCancel}
              />
            )}
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}

export default App;