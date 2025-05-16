import React from 'react';
import './OptionButton.css';

interface OptionButtonProps {
  text: string;
  index: number;
  correctIndex: number;
  selectedOption: number | null;
  onSelect: (index: number) => void;
}

const OptionButton: React.FC<OptionButtonProps> = ({
  text,
  index,
  correctIndex,
  selectedOption,
  onSelect
}) => {
  const isCorrect = selectedOption !== null && index === correctIndex;
  const isIncorrect = selectedOption === index && index !== correctIndex;
  const isDisabled = selectedOption !== null;

  let buttonClass = 'option-button';
  if (isCorrect) {
    buttonClass += ' correct-option';
  } else if (isIncorrect) {
    buttonClass += ' incorrect-option';
  }

  return (
    <button
      onClick={() => onSelect(index)}
      disabled={isDisabled}
      className={buttonClass}
      aria-label={`Option ${index + 1}: ${text}`}
    >
      {text}
    </button>
  );
};

export default OptionButton;
