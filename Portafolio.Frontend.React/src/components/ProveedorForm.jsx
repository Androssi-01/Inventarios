import { useState, useEffect } from 'react';
import axios from 'axios';

export default function ProveedorForm({ onProveedorCreado, proveedorEditando, onCancelarEdicion }) {
    const [formData, setFormData] = useState({
        nombreEmpresa: '',
        contacto: '',
        email: '',
        telefono: '',
        activo: true
    });

    const [error, setError] = useState(null);

    useEffect(() => {
        if (proveedorEditando) {
            setFormData({
                nombreEmpresa: proveedorEditando.nombreEmpresa,
                contacto: proveedorEditando.contacto,
                email: proveedorEditando.email,
                telefono: proveedorEditando.telefono,
                activo: proveedorEditando.activo

            });
        } else {
            setFormData({
                nombreEmpresa: '', contacto: '', email: '', telefono: '', activo: true
            });
        }
    }, [proveedorEditando]);


    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'checkbox' ? checked : (type === 'number' ? Number(value) : value)
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            if (proveedorEditando) {

                const updateData = {
                    nombreEmpresa: formData.nombreEmpresa,
                    contacto: formData.contacto,
                    email: formData.email,
                    telefono: formData.telefono,
                    activo: formData.activo
                };

                await axios.put(`http://localhost:5205/api/proveedores/${proveedorEditando.id}`, updateData);
            } else {

                await axios.post('http://localhost:5205/api/proveedores', formData);
            }

            setFormData({ nombreEmpresa: '', contacto: '', email: '', telefono: 0, activo: true });
            onProveedorCreado();
            if (onCancelarEdicion) onCancelarEdicion();

        } catch (err) {
            if (err.response && err.response.data && err.response.data.errors) {
                const mensajes = Object.values(err.response.data.errors).flat().join(', ');
                setError(mensajes);
            } else {
                setError('Error al guardar el proveedor.');
            }
        }
    };

    return (
        <div className="card shadow-sm mb-5 border-0">
            <div className="card-body p-4 bg-light rounded">
                <h4 className="card-title mb-4 text-success fw-bold">
                    {proveedorEditando ? '✏️ Editar Proveedor' : '📦 Crear Proveedor'}
                </h4>

                {error && <div className="alert alert-danger py-2">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">Nombre de la Empresa</label>
                            <input type="text" name="nombreEmpresa" className="form-control shadow-sm" value={formData.nombreEmpresa} onChange={handleChange} required />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">Contacto</label>
                            <input type="text" name="contacto" className="form-control shadow-sm" value={formData.contacto} onChange={handleChange} required />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">Email</label>
                            <input type="email" name="email" className="form-control shadow-sm" value={formData.email} onChange={handleChange} required />
                        </div>
                        <div className="col-md-6 mb-3">
                            <label className="form-label fw-semibold">Teléfono</label>
                            <input type="text" name="telefono" className="form-control shadow-sm" value={formData.telefono} onChange={handleChange} required />
                        </div>
                    </div>

                    {proveedorEditando && (
                        <div className="mb-3 form-check">
                            <input type="checkbox" name="activo" className="form-check-input" checked={formData.activo} onChange={handleChange} id="activoCheck" />
                            <label className="form-check-label" htmlFor="activoCheck">Proveedor Activo</label>
                        </div>
                    )}

                    <div className="mt-2">
                        <button type="submit" className="btn btn-success px-4 me-2">
                            {proveedorEditando ? 'Actualizar' : 'Guardar'}
                        </button>
                        {proveedorEditando && (
                            <button type="button" className="btn btn-secondary" onClick={onCancelarEdicion}>
                                Cancelar
                            </button>
                        )}
                    </div>
                </form>
            </div>
        </div>
    );
}