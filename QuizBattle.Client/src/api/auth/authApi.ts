import { baseApi } from '../baseApi';
import { LoginDto, RegisterDto, AuthResponseDto } from '../types/auth';

export const authApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    register: builder.mutation<void, RegisterDto>({
      query: (credentials) => ({
        url: 'auth/register',
        method: 'POST',
        body: credentials,
      }),
    }),
    login: builder.mutation<AuthResponseDto, LoginDto>({
      query: (credentials) => ({
        url: 'auth/login',
        method: 'POST',
        body: credentials,
      }),
      transformResponse: (response: {
        token: string;
        id: string;
        expiration: string;
        username: string;
        email: string;
      }): AuthResponseDto => ({
        token: response.token,
        user: {
          id: response.id, 
          email: response.email,
          username: response.username,
        },
      }),
    }),
  }),
});

export const { useRegisterMutation, useLoginMutation } = authApi;