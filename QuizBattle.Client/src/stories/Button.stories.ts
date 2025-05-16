import type { Meta, StoryObj } from '@storybook/react';
import Button from './Button';

const meta: Meta<typeof Button> = {
  title: 'Components/Button',
  component: Button,
  tags: ['autodocs'],
  argTypes: {
    variant: {
      control: { type: 'select' },
      options: ['primary', 'secondary', 'danger', 'text'],
    },
    type: {
      control: { type: 'select' },
      options: ['button', 'submit', 'reset'],
    },
    disabled: { control: 'boolean' },
    isLoading: { control: 'boolean' },
    onClick: { action: 'clicked' },
  },
};

export default meta;

type Story = StoryObj<typeof Button>;

// Primary Button Stories
export const Primary: Story = {
  args: {
    children: 'Primary Button',
    variant: 'primary',
  },
};

export const PrimaryLoading: Story = {
  args: {
    ...Primary.args,
    isLoading: true,
  },
};

export const PrimaryDisabled: Story = {
  args: {
    ...Primary.args,
    disabled: true,
  },
};

// Secondary Button Stories
export const Secondary: Story = {
  args: {
    children: 'Secondary Button',
    variant: 'secondary',
  },
};

export const SecondaryLoading: Story = {
  args: {
    ...Secondary.args,
    isLoading: true,
  },
};

export const SecondaryDisabled: Story = {
  args: {
    ...Secondary.args,
    disabled: true,
  },
};

// Danger Button Stories
export const Danger: Story = {
  args: {
    children: 'Danger Button',
    variant: 'danger',
  },
};

export const DangerLoading: Story = {
  args: {
    ...Danger.args,
    isLoading: true,
  },
};

export const DangerDisabled: Story = {
  args: {
    ...Danger.args,
    disabled: true,
  },
};

// Text Button Stories
export const Text: Story = {
  args: {
    children: 'Text Button',
    variant: 'text',
  },
};

export const TextDisabled: Story = {
  args: {
    ...Text.args,
    disabled: true,
  },
};

// Additional Variations
export const WithCustomClass: Story = {
  args: {
    children: 'Custom Styled',
    variant: 'primary',
    className: 'custom-button-class',
  },
};

export const SubmitButton: Story = {
  args: {
    children: 'Submit Form',
    variant: 'primary',
    type: 'submit',
  },
};