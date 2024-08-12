// App.tsx
import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Landing from './components/Landing';
import Login from './components/Login';
import SignUp from './components/SignUp';
import Features from './components/Features';
import Loading from './components/Loading';
import Template from './components/Template';
import ProfileSetup from './components/ProfileSetup';
import Add from './components/Add';

const ProtectedRoute: React.FC<{ element: JSX.Element }> = ({ element }) => {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  return element;
};

const AppContent: React.FC = () => {
  return (
    <AuthProvider>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/landing" element={<ProtectedRoute element={<Landing />} />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<SignUp />} />
        <Route path="/loading" element={<Loading />} />
        <Route path="/features" element={<ProtectedRoute element={<Features />} />} />
        <Route path="/template" element={<ProtectedRoute element={<Template />} />} />
        <Route path="/add" element={<ProtectedRoute element={<Add />} />} />
        <Route path="/setup-profile" element={<ProtectedRoute element={<ProfileSetup />} />} />
      </Routes>
    </AuthProvider>
  );
};

const App: React.FC = () => {
  return (
 
      <AppContent />
   
  );
};

export default App;