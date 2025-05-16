import { useNavigate } from 'react-router-dom';
import MainMenu from '../components/MainMenu/MainMenu';

const HomePage = () => {
  const navigate = useNavigate();

  const handleQuizSelect = (quizId: string) => {
    navigate(`/quiz/${quizId}`);
  };

  return (
    <div className="home-page">
      <MainMenu onQuizSelect={handleQuizSelect} />
    </div>
  );
};

export default HomePage;