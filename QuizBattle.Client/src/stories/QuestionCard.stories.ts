import type { Meta, StoryObj } from '@storybook/react';
import QuestionCard from './QuestionCard';
import { QuestionDto } from '../api/types/quiz';

const meta: Meta<typeof QuestionCard> = {
  title: 'Components/QuestionCard',
  component: QuestionCard,
  tags: ['autodocs'],
  argTypes: {
    selectedOption: {
      control: { type: 'number' },
      description: 'Index of currently selected option'
    },
    onOptionSelect: {
      action: 'optionSelected',
      description: 'Callback when option is selected'
    }
  },
};

export default meta;

type Story = StoryObj<typeof QuestionCard>;

// Sample question data
const baseQuestion: QuestionDto = {
  id: '1',
  text: 'What is the capital of France?',
  options: [
    { id: 1, text: 'London' },
    { id: 2, text: 'Paris' },
    { id: 3, text: 'Berlin' },
    { id: 4, text: 'Madrid' }
  ],
  correctAnswerIndex: 1,
  imageUrl: ''
};

const imageQuestion: QuestionDto = {
  ...baseQuestion,
    imageUrl: 'https://images.unsplash.com/photo-1431274172761-fca41d930114?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxleHBsb3JlLWZlZWR8MXx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=600&q=60',
  text: 'Which famous landmark is shown in the picture?'
};

// Base Story
export const Default: Story = {
  args: {
    question: baseQuestion,
    selectedOption: null,
  },
};

// With Image Story
export const WithImage: Story = {
  args: {
    question: imageQuestion,
    selectedOption: null,
  },
  parameters: {
    controls: { exclude: ['question'] }
  }
};

// With Selected Option Story
export const WithSelection: Story = {
  args: {
    question: baseQuestion,
    selectedOption: 1, // Paris is selected
  },
};

// With Correct Answer Selected Story
export const CorrectAnswerSelected: Story = {
  args: {
    question: baseQuestion,
    selectedOption: 1, // Paris is correct
  },
};

// With Wrong Answer Selected Story
export const WrongAnswerSelected: Story = {
  args: {
    question: baseQuestion,
    selectedOption: 0, // London is wrong
  },
};

// Mobile View Story
export const MobileView: Story = {
  args: {
    question: imageQuestion,
    selectedOption: null,
  },
  parameters: {
    viewport: {
      defaultViewport: 'mobile1',
    },
  },
};