import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import RegisterForm from '../components/Auth/RegisterForm';
import { useAppSelector } from '../hooks/redux';
import './AuthPage.css';

const RegisterPage = () => {
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
        <h1 className="auth-title">Register</h1>
        <RegisterForm />
        <p className="auth-footer">
          Already have an account?{' '}
          <a href="/login" className="auth-link">
            Login here
          </a>
        </p>
      </div>
    </div>
  );
};

export default RegisterPage;