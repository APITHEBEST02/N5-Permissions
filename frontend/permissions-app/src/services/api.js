import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', 
  headers: {
    'Content-Type': 'application/json'
  }
});


export const permissionsApi = {
  
  getAll: async () => {
    const response = await api.get('/permissions');
    return response.data;
  },
  
  
  request: async (data) => {
    const response = await api.post('/permissions/request', data);
    return response.data;
  },
  
  
  modify: async (id, data) => {
    const response = await api.put(`/permissions/modify/${id}`, data);
    return response.data;
  }
};

export default api;
