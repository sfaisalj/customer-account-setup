import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email = '';
  password = '';
  firstName = '';
  lastName = '';
  isRegisterMode = false;
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  toggleMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
    this.error = '';
  }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.error = 'Please fill in all required fields';
      return;
    }

    if (this.isRegisterMode && (!this.firstName || !this.lastName)) {
      this.error = 'Please fill in all required fields';
      return;
    }

    this.loading = true;
    this.error = '';

    const authObservable = this.isRegisterMode 
      ? this.authService.register(this.firstName, this.lastName, this.email, this.password)
      : this.authService.login(this.email, this.password);

    authObservable.subscribe({
      next: (success) => {
        this.loading = false;
        if (success) {
          this.router.navigate(['/setup']);
        } else {
          this.error = this.isRegisterMode ? 'Registration failed' : 'Login failed';
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = 'An error occurred. Please try again.';
        console.error('Auth error:', error);
      }
    });
  }
}