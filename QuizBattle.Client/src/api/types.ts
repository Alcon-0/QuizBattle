export interface QuizDto {
  id: Guid;
  title: string;
  description: string;
  imageId?: string;
  imageUrl?: string;
  questions: QuestionDto[];
}

export interface QuestionDto {
  id: Guid;
  text: string;
  options: AnswerOptionDto[];
  correctAnswerIndex: number;
  imageId?: string;
  imageUrl?: string;
}

export interface CreateQuizDto {
  title: string;
  description: string;
  coverImageId?: string;
}

export interface AnswerOptionDto {
  id: number;
  text: string;
}

export interface CreateQuestionDto {
  text: string;
  options: AnswerOptionDto[];
  correctAnswerIndex: number;
  imageId?: string;
}

export interface UpdateQuestionDto {
  text: string;
  options: AnswerOptionDto[];
  correctAnswerIndex: number;
  imageId?: string;
}

export interface Quiz {
  id: Guid;
  title: string;
  description: string;
  coverImageUrl?: string;
  questions: Question[];
}

export interface Question {
  id: Guid;
  text: string;
  options: string[];
  correctAnswer: number;
  imageUrl?: string;
}

export type Guid = string;