import { useRegisterMutation } from '../../api/auth/authApi';
import { useNavigate } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { hashPassword } from '../../utils/hashUtils';
import './AuthForm.css';
import { useEffect, useState } from 'react';

const RegisterForm = () => {
  const [register] = useRegisterMutation();
  const navigate = useNavigate();
  const [showConsentBanner, setShowConsentBanner] = useState(false);

  const checkCookieConsent = () => {
    try {
      const consent = JSON.parse(localStorage.getItem('gdprConsent') || '{}');
      return consent.necessary !== false;
    } catch {
      return false;
    }
  };

  const [hasConsent, setHasConsent] = useState(checkCookieConsent());

  useEffect(() => {
    const handleCookieConsentChange = () => {
      setHasConsent(checkCookieConsent());
    };

    window.addEventListener('cookieConsentChanged', handleCookieConsentChange);
    return () => {
      window.removeEventListener('cookieConsentChanged', handleCookieConsentChange);
    };
  }, []);


  const validationSchema = Yup.object({
    username: Yup.string()
      .required('Username is required')
      .min(3, 'Username must be at least 3 characters')
      .max(20, 'Username must be less than 20 characters')
      .matches(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),
    email: Yup.string()
      .email('Invalid email address')
      .required('Email is required'),
    password: Yup.string()
      .required('Password is required')
      .min(8, 'Password must be at least 8 characters')
      .matches(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d])[A-Za-z\d@$!%*?&]{8,}$/,
        'Password must include uppercase, lowercase, number, and special character'
      ),
    confirmPassword: Yup.string()
      .required('Please confirm your password')
      .oneOf([Yup.ref('password')], 'Passwords must match')
  });


  const formik = useFormik({
    initialValues: {
      username: '',
      email: '',
      password: '',
      confirmPassword: ''
    },
    validationSchema,
    onSubmit: async (values, { setSubmitting, setStatus }) => {
      if (!hasConsent) {
        setStatus('You must accept necessary cookies to register');
        setSubmitting(false);
        return;
      }

      try {
        const hashedPassword = hashPassword(values.password);
        await register({
          username: values.username,
          email: values.email,
          password: hashedPassword
        }).unwrap();
        navigate('/login');
      } catch (err) {
        setStatus('Registration failed. Please try again.');
      } finally {
        setSubmitting(false);
      }
    }
  });

  const handleShowConsentBanner = () => {
    window.dispatchEvent(new Event('showCookieConsent'));
  };

  return (
    <div className="form-container">
      <form className="auth-form" onSubmit={formik.handleSubmit}>
        {!hasConsent && (
          <div className="cookie-warning">
            <p>You must accept necessary cookies to register.</p>
            <button
              type="button"
              className="show-consent-btn"
              onClick={handleShowConsentBanner}
            >
              Show Cookie Settings
            </button>
          </div>
        )}

        {formik.status && (
          <div className="form-status-message">
            {formik.status}
          </div>
        )}

        <div className="form-fields">

          <div className="form-group">
            <label className="form-label" htmlFor="username">Username</label>
            <input
              id="username"
              name="username"
              type="text"
              className={`form-input ${formik.touched.username && formik.errors.username ? 'input-error' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.username}
              disabled={!hasConsent || formik.isSubmitting}
            />
            {formik.touched.username && formik.errors.username ? (
              <div className="input-error-message">{formik.errors.username}</div>
            ) : null}
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="email">Email</label>
            <input
              id="email"
              name="email"
              type="email"
              className={`form-input ${formik.touched.email && formik.errors.email ? 'input-error' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.email}
              autoComplete="username"
              disabled={!hasConsent || formik.isSubmitting}
            />
            {formik.touched.email && formik.errors.email ? (
              <div className="input-error-message">{formik.errors.email}</div>
            ) : null}
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="password">Password</label>
            <input
              id="password"
              name="password"
              type="password"
              className={`form-input ${formik.touched.password && formik.errors.password ? 'input-error' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.password}
              autoComplete="new-password"
              disabled={!hasConsent || formik.isSubmitting}
            />
            {formik.touched.password && formik.errors.password ? (
              <div className="input-error-message">{formik.errors.password}</div>
            ) : null}
          </div>

          <div className="form-group">
            <label className="form-label" htmlFor="confirmPassword">Confirm Password</label>
            <input
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              className={`form-input ${formik.touched.confirmPassword && formik.errors.confirmPassword ? 'input-error' : ''}`}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              value={formik.values.confirmPassword}
              autoComplete="new-password"
              disabled={!hasConsent || formik.isSubmitting}
            />
            {formik.touched.confirmPassword && formik.errors.confirmPassword ? (
              <div className="input-error-message">{formik.errors.confirmPassword}</div>
            ) : null}
          </div>
        </div>

        <button
          type="submit"
          className="submit-btn"
          disabled={!hasConsent || formik.isSubmitting || !formik.isValid}
        >
          {formik.isSubmitting ? 'Registering...' : 'Register'}
        </button>
      </form>
    </div>
  );
};

export default RegisterForm;