import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../hooks/redux';
import { logout } from '../../store/authSlice';
import './Layout.css';
import Button from '../common/Button';

const Header: React.FC = () => {
  const { isAuthenticated, user } = useAppSelector((state) => state.auth);
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(logout());
    navigate('/login');
  };

  return (
    <header className="app-header">
      <div className="header-container">
        <Link to="/" className="logo-link">
          <h1 className="app-title">QuizBattle</h1>
        </Link>
        <nav className="nav-links">
          {isAuthenticated ? (
            <nav className="nav-bar">
              <div className="nav-right">
                <span className="welcome-message">Welcome, {user?.username}</span>
                <Button onClick={handleLogout}>Logout</Button>
              </div>
            </nav>
          ) : (
            <>
              <Link to="/login" className='nav-link'>Login</Link>
              <Link to="/register" className='nav-link'>Register</Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
};

export default Header;
