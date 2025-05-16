import { useParams, useNavigate } from 'react-router-dom';
import QuizStart from '../components/Quiz/QuizStart';
import QuizComponent from '../components/Quiz/QuizComponent';
import { useGetQuizQuestionsQuery, useGetQuizByIdQuery } from '../api';
import { useState } from 'react';

const QuizPage = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const [quizStarted, setQuizStarted] = useState(false);
  const navigate = useNavigate();
  
  const { data: quizData } = useGetQuizByIdQuery(quizId || '', {
    skip: !quizId,
  });
  
  const { data: questions = [] } = useGetQuizQuestionsQuery(quizId || '', {
    skip: !quizId,
  });

  const handleStartQuiz = () => {
    setQuizStarted(true);
  };

  const handleBack = () => {
    navigate('/');
  };

  const handleQuizComplete = () => {
    navigate('/');
  };

  if (!quizId) {
    return <div>Quiz ID is missing</div>;
  }

  if (!quizStarted) {
    return (
      <QuizStart 
        quizTitle={quizData?.title || ''}
        questionCount={questions.length}
        onStart={handleStartQuiz}
        onBack={handleBack}
      />
    );
  }

  return (
    <QuizComponent 
      questions={questions}
      quizTitle={quizData?.title || ''}
      onComplete={handleQuizComplete}
    />
  );
};

export default QuizPage;