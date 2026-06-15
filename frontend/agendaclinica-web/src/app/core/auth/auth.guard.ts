import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = () => {
  const platformId = inject(PLATFORM_ID);
  const router = inject(Router);
  const authService = inject(AuthService);

  if (!isPlatformBrowser(platformId)) {
    return true;
  }

  const token = authService.getToken();

  if (token) {
    return true;
  }

  return router.createUrlTree(['/login']);
};