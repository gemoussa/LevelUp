import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import axiosInstance from './api/axiosInstance'; // Ensure this path is correct
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/common.css';
import GirlReadingImage from './GirlReadingImage.jpeg';
import Loading from './Loading';
import { useAuth } from '../context/AuthContext'; // Ensure this path is correct
import axios from 'axios';

const Login: React.FC = () => {
  const { setUserId, setToken } = useAuth();
  const [username, setUsername] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [rememberMe, setRememberMe] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await axiosInstance.post('/User/login', {
        username,
        password,
      });

      console.log('API Response:', response.data); // Log the response data for debugging

      // Extract userId and token from the response
      const { userId, token } = response.data;
      if (userId && token) {
        // Update the userId and token in AuthContext
        setUserId(userId);
        setToken(token); // Pass only the token

        // Handle token and userId storage based on rememberMe
        const storage = rememberMe ? localStorage : sessionStorage;
        storage.setItem('userId', userId.toString());
        storage.setItem('token', token);

        // Navigate to the landing page after successful login
        navigate('/landing');
      } else {
        throw new Error('Invalid login response: Missing userId or token');
      }
    } catch (error) {
      setLoading(false);
      if (axios.isAxiosError(error)) {
        console.error('Axios error:', error.response?.data);
        setError(error.response?.data.message || 'Incorrect username or password');
      } else if (error instanceof Error) {
        console.error('Error:', error.message);
        setError(error.message || 'An unexpected error occurred');
      } else {
        console.error('Unknown error:', error);
        setError('An unknown error occurred');
      }
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="common-background">
      <div className="container-fluid">
        <div className="left-column">
          <div className="form-container">
            <h2 className="text-center">Login</h2>
            {error && <div className="alert alert-danger text-center">{error}</div>}
            <form onSubmit={handleLogin}>
              <div className="mb-3">
                <label htmlFor="username" className="form-label">Username:</label>
                <input
                  type="text"
                  className="form-control"
                  id="username"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
                />
              </div>
              <div className="mb-3">
                <label htmlFor="password" className="form-label">Password:</label>
                <input
                  type="password"
                  className="form-control"
                  id="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>
              <div className="form-check mb-3">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id="rememberMe"
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
                <label className="form-check-label" htmlFor="rememberMe">Remember Me</label>
              </div>
              <button type="submit" className="btn btn-primary w-100">Login</button>
            </form>
            <div className="text-center mt-3">
              <p>Don't have an account? <Link to="/signup" className="text-link">Sign up</Link></p>
            </div>
          </div>
        </div>
        <div className="right-column">
          <img src={GirlReadingImage} alt="Girl Reading" className="img-fluid" />
        </div>
      </div>
    </div>
  );
};

export default Login;
