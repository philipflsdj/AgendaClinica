import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AppointmentService } from '../../core/services/appointment.service';
import { PatientService } from '../../core/services/patient.service';
import { ProfessionalService } from '../../core/services/professional.service';
import { Appointment, Patient, Professional } from '../../core/models/types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, DatePipe, MatIconModule],
  templateUrl: './appointments.component.html'
})
export class AppointmentsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private appointmentService = inject(AppointmentService);
  private patientService = inject(PatientService);
  private professionalService = inject(ProfessionalService);

  appointments = signal<Appointment[]>([]);
  patients = signal<Patient[]>([]);
  professionals = signal<Professional[]>([]);
  
  selectedProfessionalId = signal<string | null>(null);
  currentDate = signal<string>(new Date().toISOString().split('T')[0]);

  isWeekend = computed(() => {
    const date = new Date(this.currentDate() + 'T12:00:00'); // set mid-day to avoid TZ shifts
    const day = date.getDay();
    return day === 0 || day === 6;
  });

  dailyAppointments = computed(() => {
    const date = this.currentDate();
    return this.appointments()
      .filter(a => a.inicioEm.startsWith(date))
      .sort((a, b) => new Date(a.inicioEm).getTime() - new Date(b.inicioEm).getTime());
  });

  agendaSlotsData = computed(() => {
    const profId = this.selectedProfessionalId();
    if (!profId) return [];
    
    const date = this.currentDate();
    const profApts = this.appointments().filter(a => a.profissionalId === profId && a.inicioEm.startsWith(date));
    
    const slots = [];
    for(let h=8; h<18; h++){
       slots.push(`${h.toString().padStart(2, '0')}:00`);
       slots.push(`${h.toString().padStart(2, '0')}:30`);
    }
    
    return slots.map(time => {
       const apt = profApts.find(a => a.inicioEm.includes(`T${time}`));
       return { time, appointment: apt, isFree: !apt };
    });
  });

getProfessionalName(id: string) {
  return this.professionals().find(p => p.id === id)?.nomeCompleto || '';
}

getPatientName(id: string) {
  return this.patients().find(p => p.id === id)?.nomeCompleto || '';
}

getPatientDocument(id: string) {
  return this.patients().find(p => p.id === id)?.documento || '';
}

  openSchedule(time: string) {
    this.editingId.set(null);
    this.appointmentForm.reset({ pacienteId: '', profissionalId: '', inicioEm: '' });
    this.appointmentForm.patchValue({
      profissionalId: this.selectedProfessionalId() || '',
      inicioEm: `${this.currentDate()}T${time}`
    });
    this.showForm.set(true);
  }

  openForm() {
    this.editingId.set(null);
    this.appointmentForm.reset({ pacienteId: '', profissionalId: '', inicioEm: '' });
    this.showForm.set(true);
  }

  closeForm() {
    this.showForm.set(false);
    this.editingId.set(null);
    this.errorMsg.set('');
    this.appointmentForm.reset({ pacienteId: '', profissionalId: '', inicioEm: '' });
  }

  editAppointment(apt: Appointment) {
    this.editingId.set(apt.id);
    this.appointmentForm.patchValue({
      pacienteId: apt.pacienteId,
      profissionalId: apt.profissionalId,
      inicioEm: apt.inicioEm
    });
    this.showForm.set(true);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  deleteAppointment(id: string) {
    if (confirm('Tem certeza que deseja cancelar esta consulta?')) {
      this.appointmentService.deleteAppointment(id).subscribe({
        next: () => this.loadAppointments(),
        error: () => alert('Erro ao cancelar agendamento.')
      });
    }
  }

  showForm = signal(false);
  loading = signal(false);
  errorMsg = signal('');
  editingId = signal<string | null>(null);

  appointmentForm = this.fb.nonNullable.group({
    pacienteId: ['', Validators.required],
    profissionalId: ['', Validators.required],
    inicioEm: ['', Validators.required],
  });

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.patientService.getPatients().subscribe(data => this.patients.set(data));
    this.professionalService.getProfessionals().subscribe(data => this.professionals.set(data));
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentService.getAppointments().subscribe(data => this.appointments.set(data));
  }

  onSubmit() {
    if (this.appointmentForm.valid) {
      this.loading.set(true);
      this.errorMsg.set('');
      
      const payload = this.appointmentForm.getRawValue();
      const id = this.editingId();
      
      const request$ = id
        ? this.appointmentService.updateAppointment(id, payload)
        : this.appointmentService.createAppointment(payload);

      request$.subscribe({
        next: () => {
          this.loading.set(false);
          this.closeForm();
          this.loadAppointments();
        },
        error: (err) => {
          this.loading.set(false);
          this.errorMsg.set(err?.error?.message || 'Erro ao realizar agendamento.');
        }
      });
    }
  }
}
