import { baseApi } from '../baseApi';
import { QuestionDto } from '../types';

export const questionsApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getQuizQuestions: builder.query<QuestionDto[], string>({
      query: (quizId) => `quizzes/${quizId}/questions`,
      transformResponse: (response: QuestionDto[]) => 
        response.map(question => ({
          ...question,
          imageUrl: question.imageId 
            ? `http://localhost:5028/api/images/${question.imageId}`
            : undefined
        })),
      providesTags: (result, error, quizId) =>
        result
          ? [...result.map(({ id }) => ({ type: 'Question' as const, id })), 'Question']
          : ['Question']
    }),
  }),
  overrideExisting: false,
});

export const { useGetQuizQuestionsQuery } = questionsApi;