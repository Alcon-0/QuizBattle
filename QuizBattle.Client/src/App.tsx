import { Outlet } from 'react-router-dom';
import Header from './components/Layout/Header';
import CookieConsent from './components/CookieConsent';
import './styles/App.css';
import { useState, useEffect } from 'react';

const App = () => {
  const [showConsent, setShowConsent] = useState(false);

  useEffect(() => {
    // Check if we need to show consent banner on initial load
    const savedConsent = localStorage.getItem('gdprConsent');
    if (!savedConsent) {
      setShowConsent(true);
    } else {
      const consent = JSON.parse(savedConsent);
      if (consent.necessary === false) {
        setShowConsent(true);
      }
    }

    const handleShowConsent = () => {
      setShowConsent(true);
    };

    const handleHideConsent = () => {
      setShowConsent(false);
    };

    window.addEventListener('showCookieConsent', handleShowConsent);
    window.addEventListener('hideCookieConsent', handleHideConsent);
    
    return () => {
      window.removeEventListener('showCookieConsent', handleShowConsent);
      window.removeEventListener('hideCookieConsent', handleHideConsent);
    };
  }, []);

  return (
    <div className="app">
      <Header />
      {showConsent && <CookieConsent onClose={() => setShowConsent(false)} />}
      <main className="app-content">
        <Outlet />
      </main>
    </div>
  );
};

export default App;