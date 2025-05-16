import React from 'react';
import { Link } from 'react-router-dom';
import './Layout.css';

const Header: React.FC = () => {
  return (
    <header className="app-header">
      <div className="header-container">
        <Link to="/" className="logo-link">
          <h1 className="app-title">QuizBattle</h1>
        </Link>
      </div>
    </header>
  );
};

export default Header;