export interface Patient {
  id: string;
  nomeCompleto: string;
  documento: string;
  email: string;
  telefone: string;
  criadoEm?: string;
}

export interface Professional {
  id: string;
  nomeCompleto: string;
  especialidade: string;
  numeroRegistro: string;
}

export interface Appointment {
  id: string;
  pacienteId: string;
  profissionalId: string;
  inicioEm: string;
  fimEm: string;
  status: 'SCHEDULED' | 'COMPLETED' | 'CANCELED';

  paciente?: Patient;
  profissional?: Professional;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message: string;
  errors: string[];
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface AuthData {
  token: string;
  expiration?: string;
  user?: {
    id: string;
    name: string;
    email: string;
  };
}