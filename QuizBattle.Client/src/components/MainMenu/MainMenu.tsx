import React from 'react';
import { useGetQuizzesQuery } from '../../api';
import Button from '../common/Button';
import LoadingSpinner from '../common/LoadingSpinner';
import './MainMenu.css';

interface MainMenuProps {
  onQuizSelect: (quizId: string) => void;
}

const MainMenu: React.FC<MainMenuProps> = ({ onQuizSelect }) => {
  const { data: quizzes, isLoading, isError } = useGetQuizzesQuery();

  if (isLoading) {
    return (
      <div className="loading-container">
        <LoadingSpinner />
        <p>Loading quizzes...</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="error-container">
        <p>Failed to load quizzes</p>
        <Button variant="secondary" onClick={() => window.location.reload()}>
          Retry
        </Button>
      </div>
    );
  }

  return (
    <div className="main-menu-container">
      <h1 className="main-menu-title">Quiz Game</h1>
      <div className="quiz-list">
        {quizzes?.map((quiz) => (
          <div 
            key={quiz.id} 
            className="quiz-card"
            onClick={() => onQuizSelect(quiz.id)}
            role="button"
            tabIndex={0}
            onKeyDown={(e) => e.key === 'Enter' && onQuizSelect(quiz.id)}
          >
            {quiz.imageUrl && (
              <img 
                src={quiz.imageUrl} 
                alt={`${quiz.title} cover`} 
                className="quiz-cover"
              />
            )}
            
            <h3>{quiz.title}</h3>
            <p>{quiz.description}</p>
          </div>
        ))}
      </div>
    </div>
  );

};

export default MainMenu;