import { useLoginMutation } from '../../api/auth/authApi';
import { useAppDispatch } from '../../hooks/redux';
import { useNavigate, useLocation } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { hashPassword } from '../../utils/hashUtils';
import './AuthForm.css';
import { setCredentials } from '../../store/authSlice';

const LoginForm = () => {
  const [login] = useLoginMutation();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || '/';

  const validationSchema = Yup.object({
    email: Yup.string()
      .email('Invalid email address')
      .required('Email is required'),
    password: Yup.string()
      .required('Password is required')
  });

  const formik = useFormik({
    initialValues: {
      email: '',
      password: ''
    },
    validationSchema,
    onSubmit: async (values, { setSubmitting, setStatus }) => {
      try {
        const hashedPassword = hashPassword(values.password);

        const response = await login({
          email: values.email,
          password: hashedPassword
        }).unwrap();

        dispatch(setCredentials(response));
        navigate(from, { replace: true });
      } catch (err) {
        setStatus(err.data?.message || 'Login failed. Please check your credentials.');
      } finally {
        setSubmitting(false);
      }
    }
  });

  return (
    <form className="auth-form" onSubmit={formik.handleSubmit}>
      {formik.status && <div className="form-error">{formik.status}</div>}
      
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
          autoComplete="current-password"
        />
        {formik.touched.password && formik.errors.password ? (
          <div className="input-error-message">{formik.errors.password}</div>
        ) : null}
      </div>
      
      <button 
        type="submit" 
        className="submit-btn"
        disabled={formik.isSubmitting}
      >
        {formik.isSubmitting ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
};

export default LoginForm;