import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

export const routes: Routes = [
  { 
    path: 'login', 
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) 
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/layout/layout.component').then(m => m.LayoutComponent),
    children: [
      { path: '', redirectTo: 'appointments', pathMatch: 'full' },
      { 
        path: 'appointments', 
        loadComponent: () => import('./pages/appointments/appointments.component').then(m => m.AppointmentsComponent) 
      },
      { 
        path: 'patients', 
        loadComponent: () => import('./pages/patients/patients.component').then(m => m.PatientsComponent) 
      },
      {
        path: 'professionals',
        loadComponent: () => import('./pages/professionals/professionals.component').then(m => m.ProfessionalsComponent)
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
