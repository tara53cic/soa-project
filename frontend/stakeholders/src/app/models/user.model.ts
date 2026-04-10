export interface Role {
  id: number;
  name: string;
}

export interface User {
  id: number;
  username: string;
  email: string;
  profilePicture?: string | null;
  biography?: string | null;
  motto?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  role: Role;
  blocked: boolean;
}
