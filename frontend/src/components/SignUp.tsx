
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import axiosInstance from './api/axiosInstance';  
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/common.css';
import GirlReadingImage from './GirlReadingImage.jpeg';
import Loading from './Loading';
import { useAuth } from '../context/AuthContext';

const SignUp: React.FC = () => {
  const [username, setUsername] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const { setUserId, setToken } = useAuth();
  const navigate = useNavigate();

  const handleSignUp = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    if (password !== confirmPassword) {
      setError('Passwords do not match');
      setLoading(false);
      return;
    }

    const passwordError = validatePassword(password);
    if (passwordError) {
      setError(passwordError);
      setLoading(false);
      return;
    }

    try {

      const signupResponse = await axiosInstance.post('/User/signup', {
        username,
        email,
        password
      });
      console.log('Signup response:', signupResponse.data);
      const { userId, token } = signupResponse.data;

    if (!userId || !token) {
      throw new Error('Invalid response from server');
    }
    setUserId(userId.toString());
  setToken(token);

    axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    console.log('Navigation to landing page');
      setLoading(false);
      navigate('/landing');  

    } catch (error) {
      console.error('Signup error:', error);
      setLoading(false);
      if (axios.isAxiosError(error)) {
        const axiosError = error;
        const errorMessage = axiosError.response?.data?.message || 'An error occurred during sign up';
        setError(errorMessage);
      } else if (error instanceof Error) {
        setError(error.message || 'An unexpected error occurred');
      } else {
        setError('An unknown error occurred');
      }
    }
  };


  const validatePassword = (password: string): string | null => {
    if (password.length < 8) {
      return 'Password must be at least 8 characters long';
    }
    if (!/[A-Z]/.test(password)) {
      return 'Password must contain at least one uppercase letter';
    }
    if (!/[a-z]/.test(password)) {
      return 'Password must contain at least one lowercase letter';
    }
    if (!/[0-9]/.test(password)) {
      return 'Password must contain at least one number';
    }
    if (!/[^A-Za-z0-9]/.test(password)) {
      return 'Password must contain at least one special character';
    }
    return null;
  };

  if (loading) {
    return <Loading/>;
  }

  return (
    <div className="common-background">
      <div className="container-fluid">
        <div className="left-column">
          <div className="form-container">
            <h2 className="text-center">Sign Up</h2>
            {error && <div className="alert alert-danger text-center">{error}</div>}
            <form onSubmit={handleSignUp}>
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
                <label htmlFor="email" className="form-label">Email:</label>
                <input
                  type="email"
                  className="form-control"
                  id="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
              <div className="mb-3">
                <label htmlFor="password" className="form-label">Password:</label>
                <input
                  type={showPassword ? 'text' : 'password'}
                  className="form-control"
                  id="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>
              <div className="mb-3">
                <label htmlFor="confirmPassword" className="form-label">Confirm Password:</label>
                <input
                  type={showPassword ? 'text' : 'password'}
                  className="form-control"
                  id="confirmPassword"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  required
                />
              </div>
              <div className="mb-3 form-check">
                <input
                  type="checkbox"
                  className="form-check-input"
                  id="showPassword"
                  checked={showPassword}
                  onChange={(e) => setShowPassword(e.target.checked)}
                />
                <label className="form-check-label" htmlFor="showPassword">Show Password</label>
              </div>
              <div className="text-center">
                <button type="submit" className="btn btn-primary">Sign Up</button>
              </div>
            </form>
            <div className="text-center mt-3">
              <span>Already have an account? <a href="/login" className="text-link">Login here</a></span>
            </div>
          </div>
        </div>
        <div className="right-column">
          <img src={GirlReadingImage} alt="Right Column Image" />
        </div>
      </div>
    </div>
  );
};

export default SignUp;

