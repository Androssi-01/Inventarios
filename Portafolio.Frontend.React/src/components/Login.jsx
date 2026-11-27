import { useState } from 'react';
import axiosClient from '../api/axiosClient';

const DEMO = { email: 'demo@portafolio.com', password: 'Demo123!' };

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const [cargando, setCargando] = useState(false);
    const DEMO = { email: 'demo@portafolio.com', password: 'Demo123!' };


    const handleLogin = async (e) => {
        e.preventDefault();
        setError(null);
        setCargando(true);

        try {

            const response = await axiosClient.post('/auth/login', { email, password });
            localStorage.setItem('accessToken', response.data.token);

            // Recargamos la página para que App.jsx detecte que ya hay sesión
            window.location.reload();
        } catch (err) {
            setError('Credenciales incorrectas. Intenta de nuevo.');
            setCargando(false);
        }
    };

    return (
        <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-5">
                    <div className="card shadow-lg border-0 mt-5">
                        <div className="card-body p-5">
                            <h3 className="text-center mb-4 fw-bold text-primary">Iniciar Sesión</h3>

                            {error && <div className="alert alert-danger py-2">{error}</div>}

                            <form onSubmit={handleLogin}>
                                <div className="mb-3">
                                    <label className="form-label fw-semibold">Correo Electrónico</label>
                                    <input type="email" className="form-control"
                                        value={email} onChange={(e) => setEmail(e.target.value)} required />
                                </div>
                                <div className="mb-4">
                                    <label className="form-label fw-semibold">Contraseña</label>
                                    <input type="password" className="form-control"
                                        value={password} onChange={(e) => setPassword(e.target.value)} required />
                                </div>
                                <button type="submit" className="btn btn-primary w-100 py-2 fw-bold" disabled={cargando}>
                                    {cargando ? 'Entrando...' : 'Entrar al Sistema'}
                                </button>

                                <button type="button" className="btn btn-outline-secondary w-100 mt-2"
                                    onClick={() => { setEmail(DEMO.email); setPassword(DEMO.password); }}>
                                    Usar cuenta de demostración
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}