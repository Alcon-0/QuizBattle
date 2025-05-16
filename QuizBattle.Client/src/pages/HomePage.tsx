import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import MainMenu from '../components/MainMenu/MainMenu';
import { useAppSelector } from '../hooks/redux';

const HomePage = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useAppSelector((state) => state.auth);

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  const handleQuizSelect = (quizId: string) => {
    navigate(`/quiz/${quizId}`);
  };

  if (!isAuthenticated) return null;

  return (
    <div className="home-page">
      <MainMenu onQuizSelect={handleQuizSelect} />
    </div>
  );
};

export default HomePage;