import { Injectable, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  private readonly apiUrl = `${environment.apiUrl}`;
  private readonly tokenKey = 'auth_token';

  private readonly isBrowser = isPlatformBrowser(this.platformId);

  private tokenSignal = signal<string | null>(this.loadToken());

  isAuthenticated = computed(() => !!this.tokenSignal());

  login(request: { email: string; password: string }) {
    return this.http.post<any>(`${this.apiUrl}/login`, request).pipe(
      tap(response => {
        const token = response.data?.token;

        if (token) {
          this.saveToken(token);
        }
      })
    );
  }

  private saveToken(token: string): void {
    if (!this.isBrowser) return;

    localStorage.setItem(this.tokenKey, token);
    this.tokenSignal.set(token);
  }

  private loadToken(): string | null {
    if (!this.isBrowser) return null;

    return localStorage.getItem(this.tokenKey);
  }

  getToken(): string | null {
    return this.tokenSignal() || this.loadToken();
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem(this.tokenKey);
    }

    this.tokenSignal.set(null);
    this.router.navigate(['/login']);
  }
}