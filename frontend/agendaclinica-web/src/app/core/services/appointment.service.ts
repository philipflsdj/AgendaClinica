import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Appointment } from '../models/types';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/api/consultas`;

  getAppointments() {
    return this.http.get<Appointment[]>(this.apiUrl);
  }

  getAppointmentById(id: string) {
    return this.http.get<Appointment>(`${this.apiUrl}/${id}`);
  }

  createAppointment(appointment: CreateAppointmentRequest) {
    return this.http.post<Appointment>(this.apiUrl, appointment);
  }

  updateAppointment(id: string, appointment: Partial<CreateAppointmentRequest>) {
    return this.http.put<Appointment>(`${this.apiUrl}/${id}`, appointment);
  }

  deleteAppointment(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

export interface CreateAppointmentRequest {
  pacienteId: string;
  profissionalId: string;
  inicioEm: string;
}