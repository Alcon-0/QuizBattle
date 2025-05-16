import React from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../common/Button';
import './QuizResults.css';

interface QuizResultsProps {
  score: number;
  totalQuestions: number;
  quizId?: string;
}

const QuizResults: React.FC<QuizResultsProps> = ({ 
  score, 
  totalQuestions,
  quizId 
}) => {
  const navigate = useNavigate();

  const handleComplete = () => {
    navigate('/', { 
      state: { 
        fromQuiz: true,
        quizId,
        score 
      } 
    });
  };

  return (
    <div className="completed-container">
      <h2>Quiz Completed!</h2>
      <p className="score-text">
        Your score: <span className="score-highlight">{score}</span> out of {totalQuestions}
      </p>
      <Button 
        variant="primary"
        onClick={handleComplete}
      >
        Back to Main Menu
      </Button>
    </div>
  );
};

export default QuizResults;