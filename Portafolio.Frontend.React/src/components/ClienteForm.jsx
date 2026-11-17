import { useState, useEffect } from 'react';
import axiosClient from '../api/axiosClient';

export default function ClienteForm({ onClienteCreado, clienteEditando, onCancelarEdicion }) {
    const [formData, setFormData] = useState({
        nombre: '',
        email: '',
        telefono: '',
        direccion: '',
        activo: true
    });

    const [error, setError] = useState(null);

    useEffect(() => {
        if (clienteEditando) {
            setFormData({
                nombre: clienteEditando.nombre,
                email: clienteEditando.email,
                telefono: clienteEditando.telefono || '',
                direccion: clienteEditando.direccion || '',
                activo: clienteEditando.activo
            });
        } else {
            setFormData({ nombre: '', email: '', telefono: '', direccion: '', activo: true });
        }
    }, [clienteEditando]);



    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'checkbox' ? checked : value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            if (clienteEditando) {

                await axiosClient.put(`/clientes/${clienteEditando.id}`, formData);
            } else {

                await axiosClient.post('/clientes', formData);
            }

            setFormData({ nombre: '', email: '', telefono: '', direccion: '', activo: true });
            onClienteCreado();
            if (onCancelarEdicion) onCancelarEdicion();

        } catch (err) {
            if (err.response && err.response.data && err.response.data.errors) {
                const mensajes = Object.values(err.response.data.errors).flat().join(', ');
                setError(mensajes);
            } else {
                setError('Error al guardar el cliente.');
            }
        }
    };


    return (
        <div className="card shadow-sm mb-5">
            <div className="card-body p-4">
                <h4 className="card-title mb-4 text-primary">
                    {clienteEditando ? '✏️ Editar Cliente' : '➕ Crear Cliente'}
                </h4>

                {error && <div className="alert alert-danger py-2">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-bold">Nombre</label>
                            <input type="text" name="nombre" className="form-control" value={formData.nombre} onChange={handleChange} required />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-bold">Email</label>
                            <input type="email" name="email" className="form-control" value={formData.email} onChange={handleChange} required disabled={clienteEditando !== null} />
                        </div>
                    </div>

                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-bold">Teléfono</label>
                            <input type="text" name="telefono" className="form-control" value={formData.telefono} onChange={handleChange} />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-bold">Dirección</label>
                            <input type="text" name="direccion" className="form-control" value={formData.direccion} onChange={handleChange} />
                        </div>
                    </div>

                    {clienteEditando && (
                        <div className="mb-3 form-check">
                            <input type="checkbox" name="activo" className="form-check-input" checked={formData.activo} onChange={handleChange} id="activoCheck" />
                            <label className="form-check-label" htmlFor="activoCheck">Cliente Activo</label>
                        </div>
                    )}

                    <div className="mt-2">
                        <button type="submit" className="btn btn-primary px-4 me-2">
                            {clienteEditando ? 'Actualizar' : 'Guardar'}
                        </button>
                        {clienteEditando && (
                            <button type="button" className="btn btn-secondary" onClick={onCancelarEdicion}>
                                Cancelar
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
};
