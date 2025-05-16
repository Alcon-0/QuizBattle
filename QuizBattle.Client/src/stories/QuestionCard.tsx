import React from 'react';
import { QuestionDto } from '../api/types/quiz';
import OptionButton from '../components/Quiz/OptionButton';
import './Question.css';

interface QuestionCardProps {
  question: QuestionDto;
  selectedOption: number | null;
  onOptionSelect: (index: number) => void;
}

const QuestionCard: React.FC<QuestionCardProps> = ({ 
  question, 
  selectedOption, 
  onOptionSelect 
}) => {
  return (
    <div className="question-container">
      <h3 className="question-text">{question.text}</h3>
      
      {question.imageUrl && (
        <div className="image-container">
          <img 
            src={question.imageUrl} 
            alt={`Illustration for question`}
            className="question-image"
            onError={(e) => {
              const target = e.target as HTMLImageElement;
              target.style.display = 'none';
            }}
          />
        </div>
      )}

      <div className="options-container">
        {question.options.map((option, index) => (
          <OptionButton
            key={option.id}
            text={option.text}
            index={index}
            correctIndex={question.correctAnswerIndex}
            selectedOption={selectedOption}
            onSelect={onOptionSelect}
          />
        ))}
      </div>
    </div>
  );
};

export default QuestionCard;