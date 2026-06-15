INSERT INTO users (
    id,
    name,
    email,
    password_hash,
    role,
    created_at
)
VALUES (
    '11111111-1111-1111-1111-111111111111',
    'Admin',
    'admin@clinica.com',
    '$2a$11$zGZ7Y8Z3hW0aoAvfZRDrmOAMgrMPU.3zF1QzV5Z6XwE5v9y4qQteK',
    'Admin',
    NOW()
);

INSERT INTO patients (
    id,
    full_name,
    document,
    email,
    phone,
    created_at
)
VALUES 
(
    '22222222-2222-2222-2222-222222222222',
    'João da Silva',
    '12345678900',
    'joao@email.com',
    '11999999999',
    NOW()
);

INSERT INTO health_professionals (
    id,
    full_name,
    specialty,
    registration_number,
    created_at
)
VALUES
(
    '33333333-3333-3333-3333-333333333333',
    'Dra. Ana Souza',
    'Cardiologia',
    'CRM-SP-123456',
    NOW()
);

INSERT INTO appointments (
    id,
    patient_id,
    professional_id,
    start_at,
    end_at,
    status,
    created_at
)
VALUES
(
    '44444444-4444-4444-4444-444444444444',
    '22222222-2222-2222-2222-222222222222',
    '33333333-3333-3333-3333-333333333333',
    '2026-06-15 09:00:00',
    '2026-06-15 09:30:00',
    'SCHEDULED',
    NOW()
);