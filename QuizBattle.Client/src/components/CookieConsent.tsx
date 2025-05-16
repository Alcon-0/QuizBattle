import { useState, useEffect } from 'react';
import './CookieConsent.css';

interface CookieConsentState {
  necessary: boolean;
  preferences: boolean;
  analytics: boolean;
  marketing: boolean;
}

interface GdprCookieConsentProps {
  onClose: () => void;
}

const GdprCookieConsent = ({ onClose }: GdprCookieConsentProps) => {
  const [showSettings, setShowSettings] = useState(false);
  const [cookies, setCookies] = useState<CookieConsentState>({
    necessary: true,
    preferences: false,
    analytics: false,
    marketing: false
  });

  useEffect(() => {
    const savedConsent = localStorage.getItem('gdprConsent');
    if (savedConsent) {
      setCookies(JSON.parse(savedConsent));
    }
  }, []);

  const saveConsent = (newConsent: CookieConsentState) => {
    const consent = {
      ...newConsent,
      necessary: true // Always force necessary cookies to be true when saving
    };
    setCookies(consent);
    localStorage.setItem('gdprConsent', JSON.stringify(consent));
    window.dispatchEvent(new Event('cookieConsentChanged'));
    onClose(); // Close the popup after saving
  };

  const handleAcceptAll = () => {
    saveConsent({
      necessary: true,
      preferences: true,
      analytics: true,
      marketing: true
    });
  };

  const handleSaveSettings = () => {
    saveConsent(cookies);
  };

  const handleDecline = () => {
    const rejectedConsent = {
      necessary: false,
      preferences: false,
      analytics: false,
      marketing: false
    };
    setCookies(rejectedConsent);
    localStorage.setItem('gdprConsent', JSON.stringify(rejectedConsent));
    window.dispatchEvent(new Event('cookieConsentChanged'));
    onClose(); // Close the popup after declining
  };

  return (
    <div className="cookie-consent-banner">
      <div className="cookie-content">
        <h4>We Value Your Privacy</h4>
        <p>
          We use cookies to secure your authentication (required) and improve our service.
          <button
            className="settings-button"
            onClick={() => setShowSettings(!showSettings)}
          >
            {showSettings ? 'Hide Settings' : 'Customize Settings'}
          </button>
        </p>

        {showSettings && (
          <div className="cookie-settings">
            <div className="cookie-category">
              <label>
                <input
                  type="checkbox"
                  checked={cookies.necessary}
                  disabled
                />
                Necessary Cookies (Always On)
                <p className="category-description">
                  Required for user authentication and account security
                </p>
              </label>
            </div>

            <div className="cookie-category">
              <label>
                <input
                  type="checkbox"
                  checked={cookies.preferences}
                  onChange={(e) => setCookies({ ...cookies, preferences: e.target.checked })}
                />
                Preferences Cookies
              </label>
            </div>

            <div className="cookie-actions">
              <button onClick={handleDecline} className="decline-btn">
                Reject All
              </button>
              <button onClick={handleSaveSettings} className="save-btn">
                Save Preferences
              </button>
              <button onClick={handleAcceptAll} className="accept-btn">
                Accept All
              </button>
            </div>
          </div>
        )}

        {!showSettings && (
          <div className="cookie-actions">
            <button onClick={handleDecline} className="decline-btn">
              Reject All
            </button>
            <button onClick={() => setShowSettings(true)} className="settings-btn">
              Customize Settings
            </button>
            <button onClick={handleAcceptAll} className="accept-btn">
              Accept All
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default GdprCookieConsent;