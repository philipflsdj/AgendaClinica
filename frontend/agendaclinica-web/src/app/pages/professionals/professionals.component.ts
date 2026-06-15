import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProfessionalService } from '../../core/services/professional.service';
import { Professional } from '../../core/models/types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-professionals',
  standalone: true,
  imports: [ReactiveFormsModule, MatIconModule],
  templateUrl: './professionals.component.html'
})
export class ProfessionalsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private professionalService = inject(ProfessionalService);

  professionals = signal<Professional[]>([]);
  showForm = signal(false);
  loading = signal(false);
  editingId = signal<string | null>(null);

  professionalForm = this.fb.nonNullable.group({
    nomeCompleto : ['', Validators.required],
    especialidade : ['', Validators.required],
    numeroRegistro : ['', Validators.required],
  });

  ngOnInit() {
    this.loadProfessionals();
  }

  loadProfessionals() {
    this.professionalService.getProfessionals().subscribe(data => this.professionals.set(data));
  }

  openForm() {
    this.editingId.set(null);
    this.professionalForm.reset();
    this.showForm.set(true);
  }

  closeForm() {
    this.showForm.set(false);
    this.editingId.set(null);
    this.professionalForm.reset();
  }

  editProfessional(prof: Professional) {
    this.editingId.set(prof.id);
    this.professionalForm.patchValue({
      nomeCompleto: prof.nomeCompleto,
      especialidade: prof.especialidade,
      numeroRegistro: prof.numeroRegistro
    });
    this.showForm.set(true);
  }

  deleteProfessional(id: string) {
    if (confirm('Tem certeza que deseja remover este profissional?')) {
      this.professionalService.deleteProfessional(id).subscribe({
        next: () => this.loadProfessionals(),
        error: () => alert('Erro ao remover profissional.')
      });
    }
  }

onSubmit() {
  if (this.professionalForm.invalid) return;

  const professional = this.professionalForm.getRawValue();

  if (this.editingId()) {
    this.professionalService.updateProfessional(this.editingId()!, professional).subscribe({
      next: () => {
        this.loadProfessionals();
        this.closeForm();
      },
      error: (err) => console.error(err)
    });

    return;
  }

  this.professionalService.createProfessional(professional).subscribe({
    next: () => {
      this.loadProfessionals();
      this.closeForm();
    },
    error: (err) => console.error(err)
  });
}
}
