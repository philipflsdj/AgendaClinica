import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Professional } from '../models/types';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProfessionalService {
  private http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/profissionais-saude`;

  getProfessionals() {
    return this.http.get<Professional[]>(this.apiUrl);
  }

  getProfessionalById(id: string) {
    return this.http.get<Professional>(`${this.apiUrl}/${id}`);
  }

  createProfessional(professional: Omit<Professional, 'id'>) {
    return this.http.post<Professional>(this.apiUrl, professional);
  }

  updateProfessional(id: string, professional: Partial<Professional>) {
    return this.http.put<Professional>(`${this.apiUrl}/${id}`, professional);
  }

  deleteProfessional(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}