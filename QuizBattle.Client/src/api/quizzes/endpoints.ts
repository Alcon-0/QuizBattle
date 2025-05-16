import { baseApi } from '../baseApi';
import { QuizDto, CreateQuizDto } from '../types';

export const quizzesApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getQuizzes: builder.query<QuizDto[], void>({
      query: () => 'quizzes',
      transformResponse: (response: QuizDto[]) => 
        response.map(quiz => ({
          ...quiz,
          imageUrl: quiz.imageId 
            ? `http://localhost:5028/api/images/${quiz.imageId}`
            : ''
        })),
      providesTags: ['Quiz'],
    }),
    getQuizById: builder.query<QuizDto, string>({
      query: (id) => `quizzes/${id}`,
    }),
    createQuiz: builder.mutation<QuizDto, CreateQuizDto>({
      query: (body) => ({
        url: 'quizzes',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['Quiz'],
    }),
  }),
  overrideExisting: false,
});

export const { 
  useGetQuizzesQuery, 
  useGetQuizByIdQuery, 
  useCreateQuizMutation 
} = quizzesApi;