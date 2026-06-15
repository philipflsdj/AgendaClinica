import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PatientService } from '../../core/services/patient.service';
import { Patient } from '../../core/models/types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-patients',
  standalone: true,
  imports: [ReactiveFormsModule, MatIconModule],
  templateUrl: './patients.component.html'
})
export class PatientsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private patientService = inject(PatientService);

  patients = signal<Patient[]>([]);
  showForm = signal(false);
  loading = signal(false);
  editingId = signal<string | null>(null);

patientForm = this.fb.nonNullable.group({
  nomeCompleto: ['', Validators.required],
  documento: ['', Validators.required],
  email: ['', [Validators.required, Validators.email]],
  telefone: ['', Validators.required],
});
  ngOnInit() {
    this.loadPatients();
  }

  loadPatients() {
    this.patientService.getPatients().subscribe(data => this.patients.set(data));
  }

  openForm() {
    this.editingId.set(null);
    this.patientForm.reset();
    this.showForm.set(true);
  }

  closeForm() {
    this.showForm.set(false);
    this.editingId.set(null);
    this.patientForm.reset();
  }

  editPatient(patient: Patient) {
    this.editingId.set(patient.id);
    this.patientForm.patchValue({
      nomeCompleto: patient.nomeCompleto,
      documento: patient.documento,
      email: patient.email,
      telefone: patient.telefone
    });
    this.showForm.set(true);
  }

  deletePatient(id: string) {
    if (confirm('Tem certeza que deseja remover este paciente?')) {
      this.patientService.deletePatient(id).subscribe({
        next: () => this.loadPatients(),
        error: () => alert('Erro ao remover paciente.')
      });
    }
  }

  onSubmit() {
    if (this.patientForm.valid) {
      this.loading.set(true);
      
      const payload = this.patientForm.getRawValue();
      const id = this.editingId();
      
      const request$ = id 
        ? this.patientService.updatePatient(id, payload)
        : this.patientService.createPatient(payload);

      request$.subscribe({
        next: () => {
          this.loading.set(false);
          this.closeForm();
          this.loadPatients();
        },
        error: () => {
          this.loading.set(false);
          alert('Erro ao salvar paciente.');
        }
      });
    }
  }
}
