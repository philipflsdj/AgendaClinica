import {
  AngularNodeAppEngine,
  createNodeRequestHandler,
  isMainModule,
  writeResponseToNodeResponse,
} from '@angular/ssr/node';
import express from 'express';
import {join} from 'node:path';

const browserDistFolder = join(import.meta.dirname, '../browser');

const app = express();
const angularApp = new AngularNodeAppEngine();

app.use(express.json());

// Mock DB
const patients: { id: string; name: string; cpf: string; email: string; phone: string }[] = [
  { id: '1', name: 'João Silva', cpf: '111.111.111-11', email: 'joao@email.com', phone: '11999999999' }
];
const professionals: { id: string; name: string; specialty: string; crm: string }[] = [
  { id: '1', name: 'Dr. Marcos Costa', specialty: 'Cardiologia', crm: '12345-SP' },
  { id: '2', name: 'Dra. Ana Costa', specialty: 'Dermatologia', crm: '54321-SP' }
];
const appointments: { id: string; patientId: string; professionalId: string; date: string; status: string }[] = [];

app.post('/api/auth/login', (req, res) => {
  const { email, password } = req.body;
  // Dummy check
  if (email && password) {
    res.json({ token: 'mock-jwt-token-123456', user: { id: 'admin1', name: 'Admin', email } });
  } else {
    res.status(401).json({ message: 'Credenciais inválidas' });
  }
});

app.get('/api/patients', (req, res) => res.json(patients));
app.post('/api/patients', (req, res) => {
  const p = { id: Date.now().toString(), ...req.body };
  patients.push(p);
  res.json(p);
});

app.put('/api/patients/:id', (req, res) => {
  const id = req.params.id;
  const index = patients.findIndex(p => p.id === id);
  if (index !== -1) {
    patients[index] = { ...patients[index], ...req.body };
    res.json(patients[index]);
  } else {
    res.status(404).json({ message: 'Paciente não encontrado' });
  }
});

app.delete('/api/patients/:id', (req, res) => {
  const id = req.params.id;
  const index = patients.findIndex(p => p.id === id);
  if (index !== -1) {
    const deleted = patients.splice(index, 1);
    res.json(deleted[0]);
  } else {
    res.status(404).json({ message: 'Paciente não encontrado' });
  }
});

app.get('/api/professionals', (req, res) => res.json(professionals));

app.post('/api/professionals', (req, res) => {
  const p = { id: Date.now().toString(), ...req.body };
  professionals.push(p);
  res.json(p);
});

app.put('/api/professionals/:id', (req, res) => {
  const id = req.params.id;
  const index = professionals.findIndex(p => p.id === id);
  if (index !== -1) {
    professionals[index] = { ...professionals[index], ...req.body };
    res.json(professionals[index]);
  } else {
    res.status(404).json({ message: 'Profissional não encontrado' });
  }
});

app.delete('/api/professionals/:id', (req, res) => {
  const id = req.params.id;
  const index = professionals.findIndex(p => p.id === id);
  if (index !== -1) {
    const deleted = professionals.splice(index, 1);
    res.json(deleted[0]);
  } else {
    res.status(404).json({ message: 'Profissional não encontrado' });
  }
});

app.get('/api/appointments', (req, res) => {
  const enriched = appointments.map(a => ({
    ...a,
    patient: patients.find(p => p.id === a.patientId),
    professional: professionals.find(p => p.id === a.professionalId)
  }));
  res.json(enriched);
});

app.post('/api/appointments', (req, res) => {
  const { patientId, professionalId, date } = req.body; // date in ISO format
  const aptDate = new Date(date);
  
  // Basic Validations
  const day = aptDate.getDay();
  const hour = aptDate.getHours();
  const minutes = aptDate.getMinutes();

  if (day === 0 || day === 6) {
    res.status(400).json({ message: 'Atendimento apenas de segunda a sexta.' });
    return;
  }
  if (hour < 8 || hour >= 18) {
    res.status(400).json({ message: 'Atendimento das 08:00 às 18:00.' });
    return;
  }
  if (minutes !== 0 && minutes !== 30) {
    res.status(400).json({ message: 'Consultas devem ser em blocos de 30 minutos (XX:00 ou XX:30).' });
    return;
  }

  const dateString = aptDate.toISOString().split('T')[0];

  // Regras: 1 paciente só pode ter 1 consulta por profissional por dia
  const patientConflict = appointments.find(a => 
    a.patientId === patientId && 
    a.professionalId === professionalId && 
    a.date.startsWith(dateString)
  );
  if (patientConflict) {
    res.status(400).json({ message: 'Paciente já possui consulta com este profissional neste dia.' });
    return;
  }

  // Regras: 1 profissional só pode atender 1 consulta por horário
  const profConflict = appointments.find(a => 
    a.professionalId === professionalId && 
    a.date === date
  );
  if (profConflict) {
    res.status(400).json({ message: 'Profissional já possui consulta neste horário.' });
    return;
  }

  const apt = { id: Date.now().toString(), patientId, professionalId, date, status: 'SCHEDULED' };
  appointments.push(apt);
  res.json(apt);
});

/**
 * Serve static files from /browser
 */
app.use(
  express.static(browserDistFolder, {
    maxAge: '1y',
    index: false,
    redirect: false,
  }),
);

/**
 * Handle all other requests by rendering the Angular application.
 */
app.use((req, res, next) => {
  angularApp
    .handle(req)
    .then((response) =>
      response ? writeResponseToNodeResponse(response, res) : next(),
    )
    .catch(next);
});

/**
 * Start the server if this module is the main entry point, or it is ran via PM2.
 * The server listens on the port defined by the `PORT` environment variable, or defaults to 4000.
 */
if (isMainModule(import.meta.url) || process.env['pm_id']) {
  const port = process.env['PORT'] || 4000;
  app.listen(port, (error) => {
    if (error) {
      throw error;
    }

    console.log(`Node Express server listening on http://localhost:${port}`);
  });
}

/**
 * Request handler used by the Angular CLI (for dev-server and during build) or Firebase Cloud Functions.
 */
export const reqHandler = createNodeRequestHandler(app);
