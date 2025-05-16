// pages/Quiz/QuizPage.tsx
import { useParams } from 'react-router-dom';
import QuizComponent from '../components/Quiz/QuizComponent';
import { useGetQuizQuestionsQuery, useGetQuizByIdQuery } from '../api';
import { useAppSelector } from '../hooks/redux';
import LoadingSpinner from '../components/common/LoadingSpinner';

const QuizPage = () => {
  const { quizId } = useParams<{ quizId: string }>();
  const { isAuthenticated } = useAppSelector((state) => state.auth);
  
  const { data: quizData } = useGetQuizByIdQuery(quizId || '', {
    skip: !quizId || !isAuthenticated
  });
  
  const { data: questions = [], isLoading, error } = useGetQuizQuestionsQuery(
    quizId || '', 
    { skip: !quizId || !isAuthenticated }
  );

  if (!isAuthenticated) return null;
  if (!quizId) return <div>Quiz ID is missing</div>;
  if (isLoading) return <LoadingSpinner />;
  if (error) return <div>Error loading quiz</div>;

  return (
    <QuizComponent 
      questions={questions}
      quizId={quizId}
      quizTitle={quizData?.title || 'Quiz'}
    />
  );
};

export default QuizPage;