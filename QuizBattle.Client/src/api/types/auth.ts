export type LoginDto = {
  email: string;
  password: string;
};

export type RegisterDto = {
  email: string;
  password: string;
  username: string;
};

export type AuthResponseDto = {
  token: string;
  user: {
    id: string;
    email: string;
    username: string;
  };
};
