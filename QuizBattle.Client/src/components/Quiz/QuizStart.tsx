import React from 'react';
import './QuizStart.css';

interface QuizStartProps {
  quizTitle: string;
  questionCount: number;
  onStart: () => void;
  onBack: () => void;
}

const QuizStart: React.FC<QuizStartProps> = ({ 
  quizTitle, 
  questionCount, 
  onStart, 
  onBack 
}) => {
  return (
    <div className="start-container">
      <h2>{quizTitle}</h2>
      <p className="quiz-info">
        This quiz contains {questionCount} question{questionCount !== 1 ? 's' : ''}.
      </p>
      <div className="button-group">
        <button 
          className="action-button start-button"
          onClick={onStart}
        >
          Start Quiz
        </button>
        <button 
          className="action-button back-button"
          onClick={onBack}
        >
          Back to Menu
        </button>
      </div>
    </div>
  );
};

export default QuizStart;