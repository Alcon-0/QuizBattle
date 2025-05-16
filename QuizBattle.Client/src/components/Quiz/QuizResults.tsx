import React from 'react';
import { Link } from 'react-router-dom';
import Button from '../common/Button';
import './QuizResults.css';

interface QuizResultsProps {
  score: number;
  totalQuestions: number;
  onComplete?: () => void;
}

const QuizResults: React.FC<QuizResultsProps> = ({ 
  score, 
  totalQuestions,
  onComplete 
}) => {
  return (
    <div className="completed-container">
      <h2>Quiz Completed!</h2>
      <p className="score-text">
        Your score: <span className="score-highlight">{score}</span> out of {totalQuestions}
      </p>
      {onComplete ? (
        <Button 
          variant="primary"
          onClick={onComplete}
        >
          Back to Main Menu
        </Button>
      ) : (
        <Link to="/">
          <Button variant="primary">
            Back to Main Menu
          </Button>
        </Link>
      )}
    </div>
  );
};

export default QuizResults;