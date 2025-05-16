import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { QuestionDto } from '../../api/types/quiz';
import QuestionCard from './QuestionCard';
import QuizResults from './QuizResults';
import './QuizComponent.css';

interface QuizComponentProps {
  questions: QuestionDto[];
  quizId: string;
  quizTitle?: string;
}

const QuizComponent: React.FC<QuizComponentProps> = ({ 
  questions, 
  quizId,
  quizTitle = 'Quiz'
}) => {
  const navigate = useNavigate();
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [timeLeft, setTimeLeft] = useState(15);
  const [selectedOption, setSelectedOption] = useState<number | null>(null);
  const [score, setScore] = useState(0);
  const [quizCompleted, setQuizCompleted] = useState(false);

  const currentQuestion = questions[currentQuestionIndex];

  useEffect(() => {
    setTimeLeft(15);
    setSelectedOption(null);

    const timer = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) {
          clearInterval(timer);
          handleNextQuestion();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [currentQuestionIndex]);

  const handleOptionSelect = (optionIndex: number) => {
    if (selectedOption === null) {
      setSelectedOption(optionIndex);
      setTimeout(() => {
        handleNextQuestion(optionIndex);
      }, 500);
    }
  };

  const handleNextQuestion = (selectedOptionIndex?: number) => {
    if (selectedOptionIndex !== undefined && 
        selectedOptionIndex === currentQuestion.correctAnswerIndex) {
      setScore(score + 1);
    }

    if (currentQuestionIndex < questions.length - 1) {
      setCurrentQuestionIndex(currentQuestionIndex + 1);
    } else {
      setQuizCompleted(true);
    }
  };

  const handleQuizComplete = () => {
    navigate('/', {
      state: {
        fromQuiz: true,
        quizId,
        score,
        totalQuestions: questions.length
      }
    });
  };

  if (quizCompleted) {
    return (
      <QuizResults 
        score={score} 
        totalQuestions={questions.length}
      />
    );
  }

  return (
    <div className="quiz-container">
      <div className="quiz-header">
        <h2>{quizTitle}</h2>
        <div className="meta-info">
          <span>Question {currentQuestionIndex + 1} of {questions.length}</span>
          <span className="timer">⏱ {timeLeft}s</span>
          <span className="score">Score: {score}</span>
        </div>
      </div>

      <QuestionCard 
        question={currentQuestion}
        selectedOption={selectedOption}
        onOptionSelect={handleOptionSelect}
      />
    </div>
  );
};

export default QuizComponent;