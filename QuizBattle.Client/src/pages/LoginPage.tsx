import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/Auth/LoginForm';
import { useAppSelector } from '../hooks/redux';
import './AuthPage.css';

const LoginPage = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useAppSelector((state) => state.auth);

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/');
    }
  }, [isAuthenticated, navigate]);

  return (
    <div className="auth-page">
      <div className="auth-container">
        <h1 className="auth-title">Login</h1>
        <LoginForm />
        <p className="auth-footer">
          Don't have an account?{' '}
          <a href="/register" className="auth-link">
            Register here
          </a>
        </p>
      </div>
    </div>
  );
};

export default LoginPage;