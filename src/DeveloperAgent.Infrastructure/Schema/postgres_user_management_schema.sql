-- PostgreSQL schema for User Management and Audit Trails
-- Tables: users, roles, user_roles, refresh_tokens, audit_trails

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(256) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    display_name VARCHAR(200),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at_utc TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    updated_at_utc TIMESTAMP WITH TIME ZONE,
    last_login_at_utc TIMESTAMP WITH TIME ZONE
);

CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(512),
    created_at_utc TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE user_roles (
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

CREATE TABLE refresh_tokens (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    token TEXT NOT NULL,
    expires_at_utc TIMESTAMP WITH TIME ZONE NOT NULL,
    revoked_at_utc TIMESTAMP WITH TIME ZONE,
    created_at_utc TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE audit_trails (
    id BIGSERIAL PRIMARY KEY,
    entity_type VARCHAR(200) NOT NULL,
    entity_id VARCHAR(200),
    action VARCHAR(50) NOT NULL,
    actor_user_id INTEGER REFERENCES users(id) ON DELETE SET NULL,
    timestamp_utc TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    changes JSONB,
    ip_address VARCHAR(45)
);

CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_audit_trails_entity ON audit_trails(entity_type, entity_id);

-- Example seed
INSERT INTO roles (name, description) VALUES ('Administrator', 'System administrator with full privileges');
