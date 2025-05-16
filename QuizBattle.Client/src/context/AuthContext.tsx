import { createContext, useContext, ReactNode, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../hooks/redux';
import { setCredentials, logout } from '../store/authSlice';
import { AuthResponseDto } from '../api/types/auth';

type AuthContextType = {
  user: { id: string; email: string; username: string } | null;
  isAuthenticated: boolean;
  login: (data: AuthResponseDto) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const dispatch = useAppDispatch();
  const { user, isAuthenticated, token } = useAppSelector((state) => state.auth);

  const login = (data: AuthResponseDto) => dispatch(setCredentials(data));
  const handleLogout = () => dispatch(logout());

  useEffect(() => {
    if (token) {
      const user = localStorage.getItem('user');
      if (user) {
        dispatch(setCredentials({ user: JSON.parse(user), token }));
      }
    }
  }, [dispatch, token]);

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, login, logout: handleLogout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};