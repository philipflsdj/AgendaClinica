CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DROP TABLE IF EXISTS appointments CASCADE;
DROP TABLE IF EXISTS health_professionals CASCADE;
DROP TABLE IF EXISTS patients CASCADE;
DROP TABLE IF EXISTS users CASCADE;

CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    name VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL,
    password_hash TEXT NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'User',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,

    CONSTRAINT uq_users_email UNIQUE (email)
);

CREATE TABLE patients (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    full_name VARCHAR(150) NOT NULL,
    document VARCHAR(30) NOT NULL,
    email VARCHAR(150) NOT NULL,
    phone VARCHAR(30) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,

    CONSTRAINT uq_patients_document UNIQUE (document),
    CONSTRAINT uq_patients_email UNIQUE (email)
);

CREATE TABLE health_professionals (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    full_name VARCHAR(150) NOT NULL,
    specialty VARCHAR(100) NOT NULL,
    registration_number VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,

    CONSTRAINT uq_health_professionals_registration_number UNIQUE (registration_number)
);

CREATE TABLE appointments (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    patient_id UUID NOT NULL,
    professional_id UUID NOT NULL,
    start_at TIMESTAMP NOT NULL,
    end_at TIMESTAMP NOT NULL,
    status VARCHAR(30) NOT NULL DEFAULT 'SCHEDULED',
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,

    CONSTRAINT fk_appointments_patient
        FOREIGN KEY (patient_id)
        REFERENCES patients(id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_appointments_professional
        FOREIGN KEY (professional_id)
        REFERENCES health_professionals(id)
        ON DELETE RESTRICT,

    CONSTRAINT ck_appointments_status
        CHECK (status IN ('SCHEDULED', 'COMPLETED', 'CANCELED')),

    CONSTRAINT ck_appointments_period
        CHECK (end_at > start_at)
);

CREATE INDEX idx_users_email
ON users(email);

CREATE INDEX idx_patients_document
ON patients(document);

CREATE INDEX idx_patients_email
ON patients(email);

CREATE INDEX idx_health_professionals_registration_number
ON health_professionals(registration_number);

CREATE INDEX idx_appointments_professional_start_at 
ON appointments(professional_id, start_at);

CREATE INDEX idx_appointments_patient_start_at 
ON appointments(patient_id, start_at);

CREATE INDEX idx_appointments_status 
ON appointments(status);