import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loginForm = this.fb.group({
    email: ['admin@clinica.com', [Validators.required, Validators.email]],
    password: ['123456', [Validators.required]],
  });

  loading = signal(false);
  errorMsg = signal('');

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMsg.set('');

    const request = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    };

    this.authService.login(request).subscribe({
      next: (response) => {
        this.loading.set(false);

        console.log('LOGIN RESPONSE:', response);
        console.log('TOKEN SALVO:', localStorage.getItem('auth_token'));

        this.router.navigate(['/appointments']);
      },
      error: (err) => {
        this.loading.set(false);
        console.error('LOGIN ERROR:', err);
        this.errorMsg.set('Credenciais inválidas ou erro no servidor.');
      }
    });
  }
}