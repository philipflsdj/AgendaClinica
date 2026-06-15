import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Patient } from '../models/types';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/pacientes`;

  getPatients() {
    return this.http.get<Patient[]>(this.apiUrl);
  }

  getPatientById(id: string) {
    return this.http.get<Patient>(`${this.apiUrl}/${id}`);
  }

  createPatient(patient: Omit<Patient, 'id'>) {
    return this.http.post<Patient>(this.apiUrl, patient);
  }

  updatePatient(id: string, patient: Partial<Patient>) {
    return this.http.put<Patient>(`${this.apiUrl}/${id}`, patient);
  }

  deletePatient(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}