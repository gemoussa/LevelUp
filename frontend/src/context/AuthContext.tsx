// AuthContext.tsx
import React, { createContext, useContext, useState } from 'react';

interface AuthContextProps {
  userId: string | number | null;
  token: string | null;
  isAuthenticated: boolean;
  setUserId: (userId: string | number) => void;
  setToken: (token: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [userId, setUserId] = useState<string | number | null>(localStorage.getItem('userId'));
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));

  const isAuthenticated = !!userId && !!token;

  const handleSetUserId = (id: string | number) => {
    const stringId = id.toString();
    setUserId(stringId);
    localStorage.setItem('userId', stringId);
  };

  const handleSetToken = (authToken: string) => {
    setToken(authToken);
    localStorage.setItem('token', authToken);
  };

  const handleLogout = () => {
    setUserId(null);
    setToken(null);
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
  };

  return (
    <AuthContext.Provider value={{ userId, token, isAuthenticated, setUserId: handleSetUserId, setToken: handleSetToken, logout: handleLogout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
