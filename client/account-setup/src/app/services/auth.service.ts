import { Injectable } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  emailAddress: string;
  role: string;
}

export interface LoginResponse {
  login: {
    success: boolean;
    token?: string;
    user?: User;
    error?: string;
  };
}

export interface RegisterResponse {
  register: {
    success: boolean;
    token?: string;
    user?: User;
    error?: string;
  };
}

const LOGIN_MUTATION = gql`
  mutation Login($email: String!, $password: String!) {
    login(email: $email, password: $password) {
      success
      token
      user {
        id
        firstName
        lastName
        emailAddress
        role
      }
      error
    }
  }
`;

const REGISTER_MUTATION = gql`
  mutation Register($firstName: String!, $lastName: String!, $email: String!, $password: String!) {
    register(firstName: $firstName, lastName: $lastName, email: $email, password: $password) {
      success
      token
      user {
        id
        firstName
        lastName
        emailAddress
        role
      }
      error
    }
  }
`;

const GET_ME_QUERY = gql`
  query GetMe {
    getMe {
      id
      firstName
      lastName
      emailAddress
      role
    }
  }
`;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apollo: Apollo) {
    // Check for existing token on service initialization
    const token = this.getToken();
    if (token) {
      this.loadCurrentUser();
    }
  }

  login(email: string, password: string): Observable<boolean> {
    return this.apollo.mutate<LoginResponse>({
      mutation: LOGIN_MUTATION,
      variables: { email, password }
    }).pipe(
      map(result => {
        if (result.data?.login.success && result.data.login.token) {
          this.setToken(result.data.login.token);
          this.currentUserSubject.next(result.data.login.user!);
          return true;
        } else {
          console.error('Login failed:', result.data?.login.error);
          return false;
        }
      })
    );
  }

  register(firstName: string, lastName: string, email: string, password: string): Observable<boolean> {
    return this.apollo.mutate<RegisterResponse>({
      mutation: REGISTER_MUTATION,
      variables: { firstName, lastName, email, password }
    }).pipe(
      map(result => {
        if (result.data?.register.success && result.data.register.token) {
          this.setToken(result.data.register.token);
          this.currentUserSubject.next(result.data.register.user!);
          return true;
        } else {
          console.error('Registration failed:', result.data?.register.error);
          return false;
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  private loadCurrentUser(): void {
    this.apollo.query<{ getMe: User }>({
      query: GET_ME_QUERY
    }).subscribe({
      next: (result) => {
        if (result.data?.getMe) {
          this.currentUserSubject.next(result.data.getMe);
        }
      },
      error: (error) => {
        console.error('Failed to load current user:', error);
        this.logout();
      }
    });
  }

  private setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }
}